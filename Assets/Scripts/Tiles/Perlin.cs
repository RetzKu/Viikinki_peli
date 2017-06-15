using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public enum TileType
{
    Invalid,   
    Water,
    DeepWater,
    Beach,

    Scorhed,
    Bare,
    Tundra,

    TemperateDesert,
    Shrubland,      // en edes tiedä mikä on Shrubland
    Taiga,          // näiden tilalle hassuja biome/tileTypejä

    GrassLand,
    TemperateDeciduousForest,
    TemperateRainForest,

    SubtropicalDesert,
    TropicalSeasonalForest,
    TropicalRainForest,

    Forest,
    Jungle,
    Savannah,
    Desert,
    Mountain,

    Snow
}

// TODO: Paljon hienosäätöä biomet voisi vaihtaa samaksi Tile enumiksi, mapin scrollaus, tuolla Texture2D voisi tehdä minimapin

public class Perlin : MonoBehaviour
{
    public delegate Color ColoringScheme(float elevation, float moisture);

    #region Fields
    public enum NoiseMode
    {
        Noise,
        Colored,
        ColoredMoistured
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


    [Space(1)] [Header("Big Map settings")]
    public int BigMapWidth = 3;
    public int BigMapHeight = 3;

    #endregion

    void Start()
    {
        if (RenderTarget == null)
        {
            Debug.LogError("Please set the renderTarget (Quad)");
        }
        _renderer = RenderTarget.GetComponent<Renderer>();
        InitalizeRenderTarget();


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
            DrawNoiseMap((e, m) => BiomeToColor(GetBiome(e, m)), _renderer);
        }
        else if (DrawMode == NoiseMode.Noise)
        {
            DrawNoiseMap((e, m) => Color.Lerp(Color.black, Color.white, e), _renderer);
        }
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

    // vale seed

    private void GenerateNoiseMap(float[,] noiseMap, int width, int height, float seed = 0f)
    {
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

                for (int i = 0; i < Octaves; i++)
                {
                    nx = ((y) / 100f * frequence + OffsetY * frequence);
                    ny = ((x) / 100f * frequence + OffsetX * frequence);

                    float perlin = Mathf.PerlinNoise(nx, ny) * 2 - 1;
                    noiseHeigth += perlin * amplitude;
                    amplitude *= Persistance;
                    frequence *= Lacunarity;
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

        // offsetX *= Chunk.CHUNK_SIZE;
        // offsetY *= Chunk.CHUNK_SIZE;
        // TODO: KORJAA API ihmisen luettavaksi
        OffsetX = .20f * (float)offsetX;
        OffsetY = .20f * (float)offsetY;
      

        float[,] elevation = new float[chunkSize, chunkSize];
        float[,] moisture = new float[chunkSize, chunkSize];

        GenerateNoiseMap(elevation, chunkSize, chunkSize);
        GenerateNoiseMap(moisture, chunkSize, chunkSize);

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                chunk.GetGameObject(x, y).GetComponent<Renderer>().material.color =
                    BiomeToColor(GetBiome(elevation[y, x], moisture[y, x]));
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
            case TileType.Scorhed: return Scorched;
            case TileType.Bare: return Bare;
            case TileType.Tundra: return Tundra;
            case TileType.TemperateDesert: return TemperateDesert;
            case TileType.Shrubland: return Shrubland;
            case TileType.Taiga: return Taiga;
            case TileType.GrassLand: return Grassland;
            case TileType.TemperateDeciduousForest: return TemperateDeciduousForest;
            case TileType.TemperateRainForest: return TemperateRainForest;
            case TileType.SubtropicalDesert: return SubtropicalSeasonalForest;
            case TileType.TropicalSeasonalForest: return TropicalSeasonalRainForest;
            case TileType.TropicalRainForest: return TropicalRainForest;
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
        if (e < 0.05f) return TileType.DeepWater;
        if (e < 0.22) return TileType.Water;
        if (e < 0.26) return TileType.Beach;

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

        if (e < 0.80f) return TileType.Bare;
        if (e < 0.90f) return TileType.Mountain;
        // High mountain
        return TileType.Snow;
    }
}
