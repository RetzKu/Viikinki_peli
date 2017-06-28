using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Assertions.Comparers;


// TODO: Paljon hienosäätöä biomet voisi vaihtaa samaksi Tile enumiksi, mapin scrollaus, tuolla Texture2D voisi tehdä minimapin

public class Perlin : MonoBehaviour
{
    public delegate Color ColoringScheme(float elevation, float moisture);

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

    void Start()
    {
        if (RenderTarget == null)
        {
            Debug.LogError("Please set the renderTarget (Quad)");
        }
        _renderer = RenderTarget.GetComponent<Renderer>();
        InitalizeRenderTarget();

        trees = new GameObject("trees");
    }

    public void GenerateWorldTextureMap(int x, int y, float startX, float startY)
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
        int height = tileMap.Heigth;
        float[,] elevation = new float[width, height];
        float[,] moisture = new float[width, height];

        GenerateNoiseMap(elevation, width, height);
        GenerateNoiseMap(moisture, width, height);

        for (int y = 0; y < tileMap.Width; y++)
        {
            for (int x = 0; x < tileMap.Heigth; x++)
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
            DrawNoiseMap((e, m) => BiomeToColor(GetBiome(e, m)), _renderer);
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
            bool[,] trees = PlaceTrees(blueNoise, 100, 100, noiseMap, noiseMap);
            DrawNoiseMap((elevation, moisture) => Color.Lerp(Color.black, Color.blue, elevation), _renderer, trees, 100, 100);
        }
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


    public bool[,] PlaceTrees(float[,] noiseMap, int sizeX, int sizeY, float[,] elevation, float[,] moisture)
    {
        int R = ObjectRValue;
        bool[,] treeArray = new bool[sizeY, sizeX];

        // Blue noise olisi aina isompi Kuin R????
        // int blueNoiseSafeSpace = 15;

        for (int yc = 0; yc < sizeY; yc++)
        {
            for (int xc = 0; xc < sizeX; xc++)
            {
                double max = 0;
                R = GetBiomeTreeRate(GetBiome(elevation[yc, xc], moisture[yc, xc]));

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
            return 2;
        }
        else if (type == TileType.Water)
        {
            return 10;
        }
        return 6;
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

    private void GenerateNoiseMap(float[,] noiseMap, int width, int height, float seed = 0f, int octaves = 0, float persistance = 0f, float lacuranity = 0f)
    {
        if (octaves == 0)
        {
            octaves = this.Octaves; float[,] blueNoise = GenerateBlueNoise(100, 100);
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
                noiseMap[y, x] = noiseHeigth;
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

    public void GenerateChunk(Chunk chunk, int offsetX, int offsetY) // chunkin offsetit 0,0:sta
    {
        int chunkSize = Chunk.CHUNK_SIZE;

        // TODO: KORJAA API ihmisen luettavaksi
        OffsetX = .20f * (float)offsetX;
        OffsetY = .20f * (float)offsetY;

        float[,] elevation = new float[chunkSize, chunkSize];
        float[,] moisture = new float[chunkSize, chunkSize];

        GenerateNoiseMap(elevation, chunkSize, chunkSize);
        GenerateNoiseMap(moisture, chunkSize, chunkSize);

        float[,] blueNoise = GenerateBlueNoise(20, 20);
        // todo: hyper-optimization 
        bool[,] trees = PlaceTrees(blueNoise, 20, 20, elevation, moisture);
        // spawn trees ?? 

        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                if (trees[y, x])
                {
                    Vector3 spawnPosition = new Vector3(offsetX * 20 + x, offsetY * 20 + y);
                    var go = Instantiate(TreeTrefab, this.trees.transform);                     // instantiate biome spesifiseste drop changeillä?
                    go.transform.position = spawnPosition;
                }
            }
        }

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                GameObject go = chunk.GetGameObject(x, y);
                TileType type = GetBiome(elevation[y, x], moisture[y, x]);
                go.GetComponent<Renderer>().material.color = BiomeToColor(GetBiome(elevation[y, x], moisture[y, x]));

                if (TileMap.Collides(type)) // disable atm TileMap.cs
                {
                    Collider2D body = go.GetComponent<Collider2D>();
                    body.enabled = true;
                }

                chunk.SetTile(x, y, type);
            }
        }
        offsetX = 0;
        OffsetY = 0;
    }

    public GameObject TreeTrefab;
    public void GenerateChunk(TileType[,] tiles, GameObject[,] gameObjects, int offsetX, int offsetY, int startX, int startY) // chunkin offsetit 0,0:sta
    {
        int chunkSize = Chunk.CHUNK_SIZE;

        // TODO: KORJAA API ihmisen luettavaksi
        OffsetX = .20f * (float)offsetX;
        OffsetY = .20f * (float)offsetY;

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

                if (TileMap.Collides(type)) // disable atm TileMap.cs
                {
                    Collider2D body = go.GetComponent<Collider2D>();
                    body.enabled = true;
                }
                tiles[y, x] = type;
            }
        }
        offsetX = 0;
        OffsetY = 0;
    }

    //public void GenerateTileMap(TileMap tileMap)
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

    public BiomeSettings settings;
    public TileType GetBiomeWSettings(float e, float m )
    {
        foreach(var elevationsArray in settings.elevations)
        {
            for(int i = 0; i < elevationsArray.startElevation)
            {

            }
        }

        return TileType.GrassLand;
    }



    // XD
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
