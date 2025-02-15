﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;


// TODO: Texture2D voisi olla minimappers

public class Perlin : MonoBehaviour
{
    public delegate Color ColoringScheme(float elevation, float moisture);

    private Dictionary<TileType, TilemapObjectSpawnSettings> TileSpawnSettings = new Dictionary<TileType, TilemapObjectSpawnSettings>((int)TileType.Max);

    public SingleInstanceSpawnSettings SingleInstanceSpawnSettings;
    private int _currentSpawn = 0;

    // Sampler for Trees 
    // private PoissonDiscSampler sampler;
    private int samplerWidth = Chunk.CHUNK_SIZE;
    private int samplerHeigth = Chunk.CHUNK_SIZE;
    // private int samplerRadius = 2;
    public float MaxObjectOffset = 0.3f;

    #region Fields
    public enum NoiseMode
    {
        Noise,
        Colored,
        ColoredMoistured,
        ObjectMode
    };

    public Transform RenderTarget;
    public NoiseMode DrawMode = NoiseMode.Colored;

    [Header("Mauteet/epätoiminnassa")]
    public float Step = 0.1f;
    public float Exp = 1.0f;
    public float WaterTreshold = 0.2f;
    public float MoistureFrequence = 1.5f;

    [Header("Säädöt")]
    public float Frequency = 1.0f;
    public float OffsetX = 0f;
    public float OffsetY = 0f;
    public int Width = 256;
    public int Heigth = 256;
    public bool ConstantGenerate = false;

    [Header("test")]
    public int Octaves;
    public float Persistance;
    public float Lacunarity;
    [Space(1)]

    [Header("Järjettömät")]
    public Color Scorched;
    public Color Bare;
    public Color Tundra;
    public Color TemperateDesert;
    public Color Shrubland;
    public Color Taiga;
    public Color TemperateDeciduousForest;  // TODO: keksi hassuja tiilien nimiä
    public Color TemperateRainForest;
    public Color SubtropicalSeasonalForest;
    public Color TropicalSeasonalRainForest;
    public Color TropicalRainForest;

    [Space(1)]
    [Header("Järkevät")]
    public Color Water;
    public Color DeepWater;
    public Color Grassland;
    public Color Beach;
    public Color Mountain;
    public Color Forest;
    public Color Jungle;
    public Color Savannah;
    public Color Snow;
    public Color Desert;

    private Renderer _renderer;


    [Space(1)]
    [Header("Big Map settings")]
    public int BigMapWidth = 3;
    public int BigMapHeight = 3;

    #endregion

    private GameObject trees;

    void Awake()
    {
        trees = new GameObject("trees");

        InitSettings();
    }

    private int[] _spawnsLeft;
    void Start()
    {
        _spawnsLeft = new int[SingleInstanceSpawnSettings.Spawns.Count];
        DoorHackGameObject = DoorPrefab;
        if (RenderTarget == null)
        {
            Debug.LogError("Please set the renderTarget (Quad)");
        }
        _renderer = RenderTarget.GetComponent<Renderer>();
        InitalizeRenderTarget();

        // sampler = new PoissonDiscSampler(samplerWidth, samplerHeigth, samplerRadius);


        // inline shuffle!  XDDDD
        for (int i = 0; i < SingleInstanceSpawnSettings.Spawns.Count; i++)
        {
            _spawnsLeft[i] = SingleInstanceSpawnSettings.Spawns[i].Count;
            var temp = SingleInstanceSpawnSettings.Spawns[i];
            int randomIndex = Random.Range(i, SingleInstanceSpawnSettings.Spawns.Count);
            SingleInstanceSpawnSettings.Spawns[i] = SingleInstanceSpawnSettings.Spawns[randomIndex];
            SingleInstanceSpawnSettings.Spawns[randomIndex] = temp;
        }
    }



    void InitSettings()
    {
        var tiles = (Resources.LoadAll<TilemapObjectSpawnSettings>("TileSettings"));

        for (int i = 0; i < tiles.Length; i++)
        {
            TileSpawnSettings[tiles[i].Type] = tiles[i];
            ImplementedTileSettings[(int)tiles[i].Type] = true;
        }
    }

    private bool[] ImplementedTileSettings = new bool[(int)TileType.Max];
    private bool IsImplementedSetting(TileType type)
    {
        return ImplementedTileSettings[(int)type];
    }

    public void voidGenerateWorldTextureMap(int x, int y, float startX, float startY)
    {
        float startOffX = OffsetX; // TODO: fix 
        float startOffY = OffsetY;
        int w = 200, h = 200;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                float[,] noiseMap = new float[w, h];

                GenerateNoiseMap(noiseMap, w, h);

                var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
                go.transform.position = new Vector3(startX + i, startY - j);
                var r = go.GetComponent<Renderer>();
                r.material.shader = Shader.Find("Unlit/Texture");
                DrawNoiseMap((e, m) => BiomeToColor(GetBiome(e, m)), r);

                OffsetY -= 2.5f;
            }
            OffsetX += 2.5f;
            OffsetY = startOffY;
        }
        OffsetX = startOffX;
        OffsetY = startOffY;
    }

    public void GenerateTileMap(TileMap tileMap)
    {
        int width = tileMap.Width;
        int height = tileMap.Height;
        float[,] elevation = new float[width, height];
        float[,] moisture = new float[width, height];

        GenerateNoiseMap(elevation, width, height);
        GenerateNoiseMap(moisture, width, height);

        for (int y = 0; y < tileMap.Width; y++)
        {
            for (int x = 0; x < tileMap.Height; x++)
            {
                tileMap.GetTileGameObject(y, x).GetComponent<Renderer>().material.color =
                    BiomeToColor(GetBiome(elevation[y, x], moisture[y, x]));
            }
        }
    }

    private Texture2D Init(int width, int height)
    {
        Texture2D random = new Texture2D(width, height);

        for (float x = 0; x < width * Step; x += Step)
        {
            for (float y = 0; y < height * Step; y += Step)
            {
                float p = Mathf.PerlinNoise(x, y);
                Color color = new Color(p, p, p);
                random.SetPixel((int)(x / Step), (int)(y / Step), color);
            }
        }

        random.Apply();
        return random;
    }

    private Color DefaultColoring(float e, float m)
    {
        return Color.Lerp(Color.black, Color.white, e);
    }

    public void InitalizeRenderTarget()
    {
        if (DrawMode == NoiseMode.Colored)
        {
            DrawNoiseMap((e, m) => BiomeToColor(GetBiomeWSettings(e, m)), _renderer);
        }
        else if (DrawMode == NoiseMode.Noise)
        {
            DrawNoiseMap((e, m) => Color.Lerp(Color.black, Color.white, e), _renderer);
        }
        else if (DrawMode == NoiseMode.ColoredMoistured)
        {
            float[,] blueNoise = GenerateBlueNoise(100, 100);
            DrawNoiseMap((elevation, moisture) => Color.Lerp(Color.black, Color.blue, elevation), _renderer, blueNoise,
                100, 100);
        }
        else if (DrawMode == NoiseMode.ObjectMode)
        {
            float[,] blueNoise = GenerateBlueNoise(100, 100);

            // TODO: fix visualization once needed
            float[,] noiseMap = new float[115, 115];
            GenerateNoiseMap(noiseMap, 115, 115);
            bool[,] trees = PlaceTrees(blueNoise, 100, 100, GetTestTileTypeArray(100, 100));
            DrawNoiseMap((elevation, moisture) => Color.Lerp(Color.black, Color.blue, elevation), _renderer, trees, 100, 100);
        }
    }

    TileType[,] GetTestTileTypeArray(int x, int y)
    {
        TileType[,] value = new TileType[y, x];

        for (int yIndex = 0; yIndex < y; yIndex++)
        {
            for (int xIndex = 0; xIndex < x; xIndex++)
            {
                value[yIndex, xIndex] = TileType.GrassLand;
            }
        }

        return value;
    }

    public int blueNoiseOctaves = 2;
    public float blueNoiseLacunarity = 0f;
    public float blueNoisePersistance = 0f;
    public int ObjectRValue = 2;

    public float[,] GenerateBlueNoise(int sizeX, int sizeY)
    {
        float[,] noiseMap = new float[sizeY, sizeY];
        GenerateNoiseMap(noiseMap, sizeX, sizeY, 0f, blueNoiseOctaves, blueNoisePersistance, blueNoiseLacunarity);
        return noiseMap;
    }


    public bool[,] PlaceTrees(float[,] noiseMap, int sizeX, int sizeY, TileType[,] types)
    {
        int R = ObjectRValue;
        bool[,] treeArray = new bool[sizeY, sizeX];

        // int blueNoiseSafeSpace = 15;
        for (int yc = 0; yc < sizeY; yc++)
        {
            for (int xc = 0; xc < sizeX; xc++)
            {
                double max = 0;
                R = GetBiomeTreeRate(types[yc, xc]);

                for (int yn = yc - R; yn <= yc + R; yn++)
                {
                    for (int xn = xc - R; xn <= xc + R; xn++)
                    {
                        if (xn < 0 || yn < 0 || xn >= sizeX || yn >= sizeY)
                            continue;

                        double e = noiseMap[yn, xn];
                        if (e > max)
                        {
                            max = e;
                        }
                    }
                }

                if (noiseMap[yc, xc] == max)
                {
                    treeArray[yc, xc] = true;
                }
            }
        }
        return treeArray;
    }


    int GetBiomeTreeRate(TileType type)
    {
        if (type == TileType.GrassLand)
        {
            return 1;
        }
        else if (type == TileType.Water)
        {
            return 10;
        }
        return 6;
    }

    public Texture2D GenMiniMap(int w, int h)
    {
        Width = w;
        Heigth = h;
        Texture2D tex = GeneraTextureMap((e, m) => BiomeToColor(GetBiomeWSettings(e, m)));
        Heigth = Width = 250; // fuck 
        return tex;
    }

    // TODO: REMOVE
    public Texture2D GeneraTextureMap(ColoringScheme coloring)
    {
        Texture2D texture = new Texture2D(Width, Heigth);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        float[,] elevation = new float[Width, Heigth];

        float[,] moisture = new float[Width, Heigth];
        GenerateNoiseMap(elevation, Width, Heigth);

        float tmp = OffsetX;
        OffsetX += 100f;
        GenerateNoiseMap(moisture, Width, Heigth);
        OffsetX = tmp;

        for (int y = 0; y < Width; y++)
        {
            for (int x = 0; x < Heigth; x++)
            {
                float e = elevation[y, x];
                float m = moisture[y, x];
                texture.SetPixel(x, y, coloring(e, m));
            }
        }
        texture.Apply();
        return texture;
    }

    public void DrawNoiseMap(ColoringScheme coloring, Renderer renderer)
    {
        Texture2D texture = new Texture2D(Width, Heigth);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        float[,] elevation = new float[Width, Heigth];

        float[,] moisture = new float[Width, Heigth];
        GenerateNoiseMap(elevation, Width, Heigth);

        float tmp = OffsetX;
        OffsetX += 100f;
        GenerateNoiseMap(moisture, Width, Heigth);
        OffsetX = tmp;

        for (int y = 0; y < Width; y++)
        {
            for (int x = 0; x < Heigth; x++)
            {
                float e = elevation[y, x];
                float m = moisture[y, x];
                texture.SetPixel(x, y, coloring(e, m));
            }
        }
        texture.Apply();
        renderer.material.mainTexture = texture;
    }

    public void DrawNoiseMap(ColoringScheme coloring, Renderer renderer, float[,] map, int sizeX, int sizeY)
    {
        Texture2D texture = new Texture2D(sizeX, sizeY);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                float e = map[y, x];
                texture.SetPixel(x, y, coloring(e, 0));
            }
        }
        texture.Apply();
        renderer.material.mainTexture = texture;
    }

    public void DrawNoiseMap(ColoringScheme coloring, Renderer renderer, bool[,] map, int sizeX, int sizeY)
    {
        Texture2D texture = new Texture2D(sizeX, sizeY);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                bool e = map[y, x];
                Color color = e ? Color.black : Color.white;
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        renderer.material.mainTexture = texture;
    }

    public float a = 0.05f;
    public float b = 1.0f;
    public float c = 1.5f;
    public float CenterX = 1.5f;
    public float CenterY = 1.5f;
    private float _cx;
    private float _cy;

    private float Distance(float nx, float ny)
    {
        return 2 * Mathf.Max(Mathf.Abs(_cx - nx), Mathf.Abs(_cy - ny));
    }


    public void RandomIsland(int offsetX, int offsetY)
    {
        float offX = .17f * (float)offsetX; // Random.Range(1f, 1500f);
        float offY = .17f * (float)offsetY; // Random.Range(1f, 1500f);
        // OffsetX = .17f * (float)offsetX;
        // OffsetY = .17f * (float)offsetY;

        _cx = offX;
        _cy = offY;
        CenterX = offX + 1.5f;
        CenterY = offY + 1.5f;

        OffsetX = _cx;
        OffsetY = _cy;
        // move players
    }

    // abc
    // 37              26                         85
    /*
     e = e + a - b*d^c, where d is the distance from the center (scaled to 0-1).
     Another option would be e = (e + a) * (1 - b*d^c).
     The constant a pushes everything up, b pushes the edges down, and c controls how quick the drop off is.
     a = pushes everything up
     b = pushes edges down 
     c = controls drop off
     */


    private bool _first = true;
    private void GenerateNoiseMap(float[,] noiseMap, int width, int height, float seed = 0f, int octaves = 0, float persistance = 0f, float lacuranity = 0f)
    {
        if (_first)
        {
            _first = false;
            _cx = OffsetY;
            _cy = OffsetY;
        }
        _cx = CenterX;
        _cy = CenterY;


        if (octaves == 0)
        {
            octaves = this.Octaves;
        }
        if (lacuranity == 0f)
        {
            lacuranity = this.Lacunarity;
        }
        if (persistance == 0f)
        {
            persistance = this.Persistance;
        }

        float maxNoiseHeigth = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float xOff = seed;



        for (int y = 0; y < width; y++)
        {
            float yOff = seed;
            for (int x = 0; x < height; x++)
            {
                //float nx = (float)x / width;
                //float ny = (float)y / height;
                float nx = xOff; // OffsetX; // TODO: scrollaus
                float ny = yOff; //  + OffsetY;

#if true
                float frequence = 1;
                float amplitude = 1;
                float noiseHeigth = 0;

                for (int i = 0; i < octaves; i++)
                {
                    nx = ((y) / 100f * frequence + OffsetY * frequence);
                    ny = ((x) / 100f * frequence + OffsetX * frequence);

                    float perlin = Mathf.PerlinNoise(nx, ny) * 2 - 1;
                    noiseHeigth += perlin * amplitude;
                    amplitude *= persistance;
                    frequence *= lacuranity;
                }
                if (noiseHeigth > maxNoiseHeigth)
                {
                    maxNoiseHeigth = noiseHeigth;
                }
                else if (noiseHeigth < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeigth;
                }
#if true
                noiseMap[y, x] = noiseHeigth;

                noiseMap[y, x] = noiseMap[y, x] + a - b * Mathf.Pow(Distance(OffsetX + x * 0.01f, OffsetY + y * 0.01f), c);
                // e = e + a - b * d ^ c
#else
#endif
#else
                float e = 1 * Mathf.PerlinNoise(nx * Frequency, ny * Frequency) +
                          0.5f * Mathf.PerlinNoise(2 * nx, 2 * ny)
                          + 0.25f * Mathf.PerlinNoise(4 * nx, 4 * ny);
                elevation[x, y] = Mathf.Pow(e, Exp);

                nx += randMoffset;
                ny += randMoffset;

                float m = 0.75f * Mathf.PerlinNoise(nx * MoistureFrequence, ny * MoistureFrequence)
                          + 0.25f * Mathf.PerlinNoise(2 * nx, 2 * ny);
                moisture[x, y] = m; // Mathf.Pow(m, Exp);
#endif
                yOff += 0.008f;
            }
            xOff += 0.008f;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(-1, 1, noiseMap[x, y]);
            }
        }
    }



    public TilemapObjectSpawnSettings SpawnSettings;
    public void SpawnObject(Vector3 spawnPosition, Transform parent, TileType type, Chunk chunk, int x, int y)
    {

        if (!IsImplementedSetting(type))
        {
            type = TileType.GrassLand; // atm kaikki grasslandikisi jos ei löydy
        }

        // choose object to spawn
        TilemapObjectSpawnSettings setting;
        if (TileSpawnSettings.TryGetValue(type, out setting))
        {
            // var setting = TileSpawnSettings[type];

            if (setting.SpawnableObjects.Length != 0)
            {
                float roll = Random.Range(0f, 100f);
                float totalCount = 0;

                for (int i = 0; i < setting.SpawnableObjects.Length; i++)
                {
                    float spawnRate = setting.SpawnableObjects[i].SpawnRate;

                    GameObject prefab = setting.SpawnableObjects[i].ObjectPrefab;
                    if (totalCount + spawnRate >= roll)
                    {
                        var go = ObjectPool.instance.GetObjectForType(prefab.name, false); // ??????????????????

                        go.transform.localScale *= Random.Range(0.90f, 1.10f);

                        float z = ZlayerManager.GetZFromY(spawnPosition); // eessä olevat puut eteen ja takana taakse
                        go.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, z);

                        go.GetComponent<Resource>().Init(false);

                        chunk.AddObject(x, y, go);

                        break; // ei montaa samaan kohtaan
                    }
                    totalCount += spawnRate;
                }
            }
        }
    }

    // spawn ratet:
    // 100
    //  - 20
    //   - lisää ?
    //   - lisää ? 
    //  - 40
    //   - lisää ?

    bool[,] GenerateObjectsPosition(float[,] objectNoise) // suurempi kuin R
    {
        bool[,] result = new bool[samplerHeigth, samplerWidth];

        List<Vector2> samples = PoissonDiscSampler.Instance.GetThreadedSamples();

        foreach (Vector2 sample in samples) // 20, 20 alueelta pisteitä muuta indekseiksi
        {
            result[(int)sample.x, (int)sample.y] = true;   // myohemmin generaatio voisi suoraan olla tähän
        }
        return result;
    }

    public void GenerateChunk(Chunk chunk, int offsetX, int offsetY) // chunkin offsetit 0,0:sta
    {
        int chunkSize = Chunk.CHUNK_SIZE;

        // TODO: KORJAA API ihmisen luettavaksi
        OffsetX = .17f * (float)offsetX; // yolo
        OffsetY = .17f * (float)offsetY;

        float[,] elevation = new float[chunkSize, chunkSize];
        float[,] moisture = new float[chunkSize, chunkSize];
        TileType[,] types = new TileType[chunkSize, chunkSize];

        GenerateNoiseMap(elevation, chunkSize, chunkSize);
        GenerateNoiseMap(moisture, chunkSize, chunkSize);

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                GameObject go = chunk.GetGameObject(x, y);
                TileType type = GetBiomeWSettings(elevation[y, x], moisture[y, x]);
                types[y, x] = type;

                // TileType type = GetBiome(elevation[y, x], moisture[y, x]);
                // go.GetComponent<Renderer>().material.color = BiomeToColor(type);

                if (TileMap.Collides(type)) // disable atm IileMap.cs
                {
                    Collider2D body = go.GetComponent<Collider2D>();
                    body.enabled = true;
                }
                chunk.SetTile(x, y, type);
            }
        }


        bool[,] trees = GenerateObjectsPosition(moisture);

        for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
        {
            for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
            {
                if (trees[y, x])
                {
                    Vector3 spawnPosition = new Vector3(offsetX * Chunk.CHUNK_SIZE + x + Random.Range(-MaxObjectOffset, MaxObjectOffset), offsetY * Chunk.CHUNK_SIZE + y + Random.Range(-MaxObjectOffset, MaxObjectOffset));
                    SpawnObject(spawnPosition, this.trees.transform, types[y, x], chunk, x, y);
                }
            }
        }

        if (!TrySpawnRares(chunk))
        {
            TryToSpawnCaveDoors();
        }

        OffsetX = 0;
        OffsetY = 0;
    }

    [Header("every  doorspawnrate/per chunk")]
    public int DoorSpawnRate = 10;
    public GameObject DoorPrefab;

    public static GameObject DoorHackGameObject;
    public static bool CanPlaceDoor = false;
    public void TryToSpawnCaveDoors()
    {
        if (0 == Random.Range(0, DoorSpawnRate))
        {
            CanPlaceDoor = true;
        }
    }


    private readonly int SPAWN_MAX_TRIES = 5;
    private bool AllSpawnded = false;
    public bool SpawnRares(Chunk chunk) // Spawned y/n
    {
        if (_currentSpawn >= SingleInstanceSpawnSettings.Spawns.Count || AllSpawnded)
        {
            return false;
        }

        if (0 == Random.Range(0, SingleInstanceSpawnSettings.SpawnRate))
        {
            int tries = 0;
            bool Success = false;

            do
            {
                int x = Random.Range(0, Chunk.CHUNK_SIZE);
                int y = Random.Range(0, Chunk.CHUNK_SIZE);

                if (chunk.AreaClear(x, y))
                {
                    if (chunk.GetTileOnTileGameObject(x, y) == null)
                    {
                        bool searchingForSpawn = true;
                        int index = Random.Range(0, SingleInstanceSpawnSettings.Spawns.Count);
                        int startIndex = index;

                        while (searchingForSpawn)
                        {
                            if (_spawnsLeft[index] > 0)
                            {
                                var go = Instantiate(SingleInstanceSpawnSettings.Spawns[index].Spawn);
                                _spawnsLeft[index]--;

                                go.transform.position = chunk.GetGameObject(x, y).transform.position;
                                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, ZlayerManager.GetZFromY(go.transform.position));

                                chunk.AddObject(x, y, go);
                                Success = true;
                                Debug.Log("rare spawned!");
                                // _currentSpawn++;

                                searchingForSpawn = false;
                                break;
                            }

                            index++;
                            if (index == startIndex)
                            {
                                AllSpawnded = true;
                                break;
                            }

                            if (index == SingleInstanceSpawnSettings.Spawns.Count)
                            {
                                index = 0;
                            }
                        }
                    }
                }
                tries++;
            } while (tries < SPAWN_MAX_TRIES && !Success);
            return Success;
        }
        return false;
    }


    public int ChooseIndex(int size)
    {
        int index = Random.Range(0, size);
        int startIndex = index;

        while (true)
        {
            if (_spawnsLeft[startIndex] > 0)
            {
                return startIndex;
            }

            startIndex++;

            if (startIndex == size)
            {
                startIndex = 0;
            }

            if (startIndex == index)
            {
                break;
            }
        }

        return -1;
    }

    public bool TrySpawnRares(Chunk chunk)
    {
        if (AllSpawnded)
            return false;

        // choose index
        int index = ChooseIndex(SingleInstanceSpawnSettings.Spawns.Count);

        if (index == -1)
        {
            AllSpawnded = true;
        }

        if (0 == Random.Range(0, SingleInstanceSpawnSettings.SpawnRate))
        {
            int tries = 0;
            do
            {
                int x = Random.Range(0, Chunk.CHUNK_SIZE);
                int y = Random.Range(0, Chunk.CHUNK_SIZE);

                if (chunk.AreaClear(x, y))
                {
                    if (chunk.GetTileOnTileGameObject(x, y) == null)
                    {
                        var go = Instantiate(SingleInstanceSpawnSettings.Spawns[index].Spawn);
                        _spawnsLeft[index]--;

                        go.transform.position = chunk.GetGameObject(x, y).transform.position;
                        go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, ZlayerManager.GetZFromY(go.transform.position));

                        chunk.AddObject(x, y, go);
                        Debug.Log("rare spawned!");

                        return true;
                    }
                }
                tries++;
            } while (tries < SPAWN_MAX_TRIES);
        }
        return false;
    }

    public void GenerateChunk(TileType[,] tiles, GameObject[,] gameObjects, int offsetX, int offsetY, int startX, int startY) // chunkin offsetit 0,0:sta
    {
        int chunkSize = Chunk.CHUNK_SIZE;

        // TODO: KORJAA API ihmisen luettavaksi
        OffsetX = .17f * (float)offsetX;
        OffsetY = .17f * (float)offsetY;

        float[,] elevation = new float[chunkSize, chunkSize];
        float[,] moisture = new float[chunkSize, chunkSize];

        GenerateNoiseMap(elevation, chunkSize, chunkSize);
        GenerateNoiseMap(moisture, chunkSize, chunkSize);

        for (int y = startY, chunkY = 0; y < startY + chunkSize - 1; y++, chunkY++)
        {
            for (int x = startX, chunkX = 0; x < startX + chunkSize - 1; x++, chunkX++)
            {
                GameObject go = gameObjects[y, x];
                TileType type = GetBiome(elevation[chunkY, chunkX], moisture[chunkY, chunkX]);
                go.GetComponent<Renderer>().material.color = BiomeToColor(GetBiome(elevation[chunkY, chunkX], moisture[chunkY, chunkX]));

                if (TileMap.Collides(type)) // disable atm ITileMap.cs
                {
                    Collider2D body = go.GetComponent<Collider2D>();
                    body.enabled = true;

                }
                tiles[y, x] = type;
            }
        }
        OffsetX = 0;
        OffsetY = 0;
    }


    private const int _neigboursLength = 8;
    private Vec2[] neighbours = new Vec2[_neigboursLength]
    {
        new Vec2(-1, 1), new Vec2(0, 1), new Vec2(1, 1),
        new Vec2(1, 0), new Vec2(-1, 0),
        new Vec2(-1, -1), new Vec2(0, -1), new Vec2(-1, -1),
    };

    public void PlaceBase(Chunk chunk)
    {
        bool success = false;
        int baseX = 0, baseY = 0;
        for (int y = 1; y < Chunk.CHUNK_SIZE - 1; y += 2)
        {
            for (int x = 1; x < Chunk.CHUNK_SIZE - 1; x += 2)
            {
                int neighbourCount = 0;
                for (int i = 0; i < neighbours.Length; i++)
                {
                    var n = neighbours[i];
                    if (chunk.GetTile(n.Y, n.X) == TileType.GrassLand)
                    {
                        neighbourCount++;
                    }
                }
                if (neighbourCount == _neigboursLength)
                {
                    success = true;
                    baseY = y;
                    baseX = x;
                    break;
                }
            }
        }

        // add base
        if (success)
        {
            print("Base location " + baseX + " " + baseY);
            chunk.AddObject(baseX, baseY, ObjectPool.instance.GetObjectForType("Campfire_fire", false));
        }
    }

    //public void GenerateTileMap(ITileMap tileMap)
    //{
    //    int width = tileMap.Width;
    //    int height = tileMap.Height;
    //    float[,] elevation = new float[width, height];
    //    float[,] moisture = new float[width, height];

    //    GenerateNoiseMap(elevation, width, height);
    //    GenerateNoiseMap(moisture, width, height);

    //    for (int x = 0; x < tileMap.Width; x++)

    //    {
    //        for (int y = 0; y < tileMap.Height; y++)
    //        {
    //            tileMap.GetTileGameObject(x, y).GetComponent<Renderer>().material.color =
    //                BiomeToColor(GetBiome(elevation[x, y], moisture[x, y]));
    //        }
    //    }
    //}

    public Color BiomeToColor(TileType biome)
    {
        switch (biome)
        {
            case TileType.DeepWater: return DeepWater;
            case TileType.Water: return Water;
            case TileType.Beach: return Beach;
            //case TileType.Scorhed: return Scorched;
            //case TileType.Bare: return Bare;
            //case TileType.Tundra: return Tundra;
            //case TileType.TemperateDesert: return TemperateDesert;
            //case TileType.Shrubland: return Shrubland;
            //case TileType.Taiga: return Taiga;
            case TileType.GrassLand: return Grassland;
            //case TileType.TemperateDeciduousForest: return TemperateDeciduousForest;
            //case TileType.TemperateRainForest: return TemperateRainForest;
            //case TileType.SubtropicalDesert: return SubtropicalSeasonalForest;
            //case TileType.TropicalSeasonalForest: return TropicalSeasonalRainForest;
            //case TileType.TropicalRainForest: return TropicalRainForest;
            case TileType.Snow: return Snow;
            case TileType.Mountain: return Mountain;
            case TileType.Forest: return Forest;
            case TileType.Jungle: return Jungle;
            case TileType.Savannah: return Savannah;
            case TileType.Desert: return Desert;
            default:
                print(biome);
                throw new ArgumentOutOfRangeException("biome", biome, null);
        }
    }

    public TileType GetBiome(float e, float m) // (elevation, moisture)
    {
        if (e < 0.16f) return TileType.DeepWater;
        if (e < 0.22f) return TileType.Water;
        if (e < 0.26f) return TileType.Beach;

        // if (e < 0.40f) return ;
        if (e < 0.50f)
        {
            if (m < 0.20f) return TileType.Desert;
            if (m > 0.70f) return TileType.Jungle;
            return TileType.GrassLand;
        }
        // if (e < 0.60f) return;
        if (e < 0.70f)
        {
            // makes me moist
            // ehkä Tänne metsä Tyypi?? mutta silloint vuorien rinteet on aina metsää
            // eli ehkä molempiin  "grassland"(0.5f) pitäisi laitta metsä + normi tiiliä
            return TileType.Forest;
        }

        //if (e < 0.80f) return TileType.Bare;
        if (e < 0.90f) return TileType.Mountain;
        // High mountain
        return TileType.Snow;
    }


    // sama asia kuin ylempänä
    public BiomeSettings settings;
    public TileType GetBiomeWSettings(float e, float m)
    {
        foreach (BiomeSettings.ElevationData elevationsArray in settings.Elevations)
        {
            for (int i = 0; i < elevationsArray.StartElevation; i++) // järjestykseesä pienin suurint
            {
                // 0.16f
                if (e < elevationsArray.StartElevation)
                {
                    foreach (var tileData in elevationsArray.Tiles)
                    {
                        // moisturechecking
                        if (m < tileData.StartMoisture)
                        {
                            return tileData.Type;
                        }
                    }
                }
            }
        }

        return TileType.GrassLand; // place holder olisi kiva
    }

    // XD:
    //void WriteBiomeDataToFile(BiomeSettings settings) // settings todo:
    //{
    //    string data = "";
    //    data += GetEnumName();
    //    // Kirjoita ehdot;
    //    for(int i = 0; i < settings.elevations.Length; i++)
    //    {
    //        float Elevation = settings.elevations[i].startElevation;
    //        WriteElevationIf(Elevation);
    //    }

    //    data += EndName();
    //    System.IO.File.WriteAllText("eetulähettääterveisensä.txt", data);
    //}

    //string GetEnumName()
    //{
    //    return "public TileType GetBiome(float e, float m) {";
    //}

    //string EndName()
    //{
    //    return "\n}";
    //}

    //string WriteElevationIf(float elevation)
    //{
    //    string str = "if ( e" + elevation.ToString() + " < " + elevation.ToString() + " ) \n{";
    //    return elevation.ToString();
    //}
}
