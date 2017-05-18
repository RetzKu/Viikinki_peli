using System;
using System.Collections;
using System.Collections.Generic;
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
    #region Fields

    public Transform RenderTarget;

    public int   Width          = 256;
    public int   Heigth         = 256;
    public float Step           = 0.1f;
    public float Frequency      = 1.0f;
    public float Exp            = 1.0f;
    public float WaterTreshold  = 0.2f;

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

    public void GenerateTileMap(TileMap tileMap)
    {
        int width = tileMap.Width;
        int height = tileMap.Height;
        float[,] elevation = new float[width, height];
        float[,] moisture = new float[width, height];
            
        GenerateNoiseMap(elevation, moisture, width, height);

        for (int x = 0; x < tileMap.Width; x++)
        {
            for (int y = 0; y < tileMap.Height; y++)
            {
                float e = elevation[x, y];
                float m = moisture[x, y];
                tileMap.GetTileGameObject(x, y).GetComponent<Renderer>().material.color = BiomeToColor(GetBiome2(e, m));
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
        Texture2D texture = new Texture2D(Width, Heigth);        
        float[,] elevation = new float[Width, Heigth];
        float[,] moisture = new float[Width, Heigth];
        GenerateNoiseMap(elevation, moisture, Width, Heigth);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Heigth; y++)
            {
                float e = elevation[x, y];
                float m = moisture[x, y];
                // Color color = new Color(e, e, e);
                Color color = BiomeToColor(GetBiome2(e, m));
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        _renderer.material.mainTexture = texture;
    }

    private void GenerateNoiseMap(float [,] elevation, float[,] moisture, int width, int height)
    {
        Texture2D random = new Texture2D(width, height);

        // float[,] elevation = new float[width, height];
        // float[,] moisture = new float[width, height];
        float randMoffset = 10f;

        float xOff = 0.0f;
        for (int x = 0; x < width; x++)
        {
            float yOff = 0.0f;
            for (int y = 0; y < height; y++)
            {
                //float nx = (float)x / width;
                //float ny = (float)y / height;
                float nx = xOff;
                float ny = yOff;

                float e = 1 * Mathf.PerlinNoise(nx * Frequency, ny * Frequency) +
                          0.5f * Mathf.PerlinNoise(2 * nx, 2 * ny)
                          + 0.25f * Mathf.PerlinNoise(4 * nx, 4 * ny);
                elevation[x, y] = Mathf.Pow(e, Exp);

                nx += randMoffset;
                ny += randMoffset;

                float m = 1 * Mathf.PerlinNoise(nx * Frequency, ny * Frequency) +
                          0.5f * Mathf.PerlinNoise(2 * nx, 2 * ny)
                          + 0.25f * Mathf.PerlinNoise(4 * nx, 2 * ny);
                moisture[x, y] = m; // Mathf.Pow(m, Exp);

                yOff += 0.01f;
            }
            xOff += 0.01f;
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

    // Ei käytössä tällä hetkellä
    public Biome GetBiome(float e, float m) // (elevation, moisture)
    {
        if (e < 0.1f) return Biome.Water;
        else if (e < 0.2f) return Biome.Beach;

        if (e > 2.8f)
        {
            if (m < 0.1f) return Biome.Scorhed;
            if (m < 0.2f) return Biome.Bare;
            if (m < 0.5f) return Biome.Tundra;
            return Biome.Snow;
        }

        if (e > 0.9f)
        {
            return Biome.Mountain;
        }

        //if (e > 0.9f)
        //{
        //    if (m < 0.33f) return Biome.TemperateDesert;
        //    if (m < 0.66f) return Biome.Shrubland;
        //    return Biome.Taiga;
        //}

        if (e > 0.3f)
        {
            if (m < 0.16f) return Biome.TemperateDesert;
            if (m < 0.50f) return Biome.GrassLand;
            if (m < 0.83f) return Biome.TemperateDeciduousForest;
            return Biome.TemperateRainForest;
        }

        if (m < 0.16f) return Biome.SubtropicalDesert;
        if (m < 0.33f) return Biome.GrassLand;
        if (m < 0.66f) return Biome.TropicalSeasonalForest;
        return Biome.TemperateRainForest;
    }

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

        //if (e > 0.3f)
        //{
        //    return Biome.Forest;
        //}

        if (m < 0.1) return Biome.Water;
        if (m < 0.2) return Biome.Beach;
        if (m < 0.3) return Biome.Forest;
        if (m < 0.5) return Biome.Jungle;
        if (m < 0.7) return Biome.Savannah;
        if (m < 0.8) return Biome.Desert;
        return Biome.GrassLand;
    }
}
