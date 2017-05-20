using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public enum Biome
{
    Water,
    DeepWater,
    Beach,

    Scorhed,
    Bare,
    Tundra,

    TemperateDesert,
    Shrubland,      // en edes tiedä mikä on Shrubland
    Taiga,

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

    public Transform RenderTarget;

    public enum NoiseMode
    {
        Noise,
        Colored,
        ColoredMoistured
    };

    public NoiseMode DrawMode = NoiseMode.Colored;

    [Header("Mauteet/epätoiminnassa")]
    public float Step = 0.1f;
    public float Exp = 1.0f;
    public float WaterTreshold = 0.2f;
    public float MoistureFrequence = 1.5f;

    [Header("Säädöt")]
    public float Frequency = 1.0f;
    public float offsetX = 0f;
    public float offsetY = 0f;
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

    void Update()
    {
    }

    public void GenerateTileMap(TileMap tileMap)
    {
        int width = tileMap.Width;
        int height = tileMap.Height;
        float[,] elevation = new float[width, height];
        float[,] moisture = new float[width, height];

        GenerateNoiseMap(elevation, width, height);
        GenerateNoiseMap(moisture, width, height);

        for (int x = 0; x < tileMap.Width; x++)

        {
            for (int y = 0; y < tileMap.Height; y++)
            {
                tileMap.GetTileGameObject(x, y).GetComponent<Renderer>().material.color =
                    BiomeToColor(GetBiome(elevation[x, y], moisture[x, y]));
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

    public void InitalizeRenderTarget()
    {
        if (DrawMode == NoiseMode.Colored)
        {
            DrawNoiseMap((e, m) => BiomeToColor(GetBiome(e, m)));
        }
        else if (DrawMode == NoiseMode.Noise)
        {
            DrawNoiseMap((e, m) => Color.Lerp(Color.black, Color.white, e));
        }
        else if (DrawMode == NoiseMode.ColoredMoistured)
        {
        }
    }

    public void DrawNoiseMap(ColoringScheme coloring)
    {
        Texture2D texture = new Texture2D(Width, Heigth);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        float[,] elevation = new float[Width, Heigth];

        float[,] moisture = new float[Width, Heigth];
        GenerateNoiseMap(elevation, Width, Heigth);

        float tmp = offsetX;
        offsetX += 100f;
        GenerateNoiseMap(moisture, Width, Heigth);
        offsetX = tmp;


        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Heigth; y++)
            {
                float e = elevation[x, y];
                float m = moisture[x, y];
                texture.SetPixel(x, y, coloring(e, m));
            }
        }
        texture.Apply();
        _renderer.material.mainTexture = texture;
    }

    // vale seed
    private void GenerateNoiseMap(float[,] noiseMap, int width, int height, float seed = 0f)
    {
        float maxNoiseHeigth = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float xOff = seed;
        for (int x = 0; x < width; x++)
        {
            float yOff = seed;
            for (int y = 0; y < height; y++)
            {
                //float nx = (float)x / width;
                //float ny = (float)y / height;
                float nx = xOff; // offsetX; // TODO: scrollaus
                float ny = yOff; //  + offsetY;

#if true
                float frequence = 1;
                float amplitude = 1;
                float noiseHeigth = 0;

                for (int i = 0; i < Octaves; i++)
                {
                    nx = ((x ) / 100f * frequence + offsetX * frequence);
                    ny = ((y ) / 100f * frequence + offsetY * frequence);

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
                noiseMap[x, y] = noiseHeigth;
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

    public Color BiomeToColor(Biome biome)
    {
        switch (biome)
        {
            case Biome.DeepWater: return DeepWater;
            case Biome.Water: return Water;
            case Biome.Beach: return Beach;
            case Biome.Scorhed: return Scorched;
            case Biome.Bare: return Bare;
            case Biome.Tundra: return Tundra;
            case Biome.TemperateDesert: return TemperateDesert;
            case Biome.Shrubland: return Shrubland;
            case Biome.Taiga: return Taiga;
            case Biome.GrassLand: return Grassland;
            case Biome.TemperateDeciduousForest: return TemperateDeciduousForest;
            case Biome.TemperateRainForest: return TemperateRainForest;
            case Biome.SubtropicalDesert: return SubtropicalSeasonalForest;
            case Biome.TropicalSeasonalForest: return TropicalSeasonalRainForest;
            case Biome.TropicalRainForest: return TropicalRainForest;
            case Biome.Snow: return Snow;
            case Biome.Mountain: return Mountain;

            case Biome.Forest: return Forest;
            case Biome.Jungle: return Jungle;
            case Biome.Savannah: return Savannah;
            case Biome.Desert: return Desert;
            default:
                print(biome);
                throw new ArgumentOutOfRangeException("biome", biome, null);
        }
    }

    public Biome GetBiome(float e, float m) // (elevation, moisture)
    {
        if (e < 0.05f) return Biome.DeepWater;
        if (e < 0.22) return Biome.Water;
        if (e < 0.26) return Biome.Beach;

        // if (e < 0.40f) return ;
        if (e < 0.50f)
        {
            if (m < 0.20f) return Biome.Desert;
            // if (m > 0.70) return Biome.Jungle;
            return Biome.GrassLand;
        }
        // if (e < 0.60f) return;
        if (e < 0.70f)
        {
            // makes me moist
            return Biome.Forest;
        }

        if (e < 0.80f) return Biome.Bare;
        if (e < 0.90f) return Biome.Mountain;
        // High mountain
        return Biome.Snow;
    }

    // fuckdudp
    public Biome GetBiome2(float e, float m) // (elevation, moisture)
    {
        if (e < 0.004f) return Biome.DeepWater;
        if (e < 0.1f) return Biome.Water;
        // ReSharper disable once PossibleNullReferenceException
        if (e < 0.12f) return Biome.Beach;

        if (e > 2.9f)
        {
            if (m < 0.2f) return Biome.Scorhed;
            if (m < 0.5f) return Biome.Bare;
            if (m < 0.95f) return Biome.Tundra;
            return Biome.Snow;
        }
        if (e > 1.6f)
        {
            return Biome.Mountain;
        }

        if (m < 0.33) // Häck
            return Biome.Jungle;
        if (e > 0.3f)
        {
            return Biome.Forest;
        }

        if (m < 0.1) return Biome.Water;
        if (m < 0.2) return Biome.Beach;
        if (m < 0.3) return Biome.Forest;
        if (m < 0.5) return Biome.Jungle;
        if (m < 0.7) return Biome.Savannah;
        if (m < 0.8) return Biome.Desert;
        return Biome.GrassLand;
    }
}
