﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Invalid,
    Water,
    DeepWater,
    Mountain,

    CollisionTiles,
    Beach,

    //Scorhed,
    //Bare,
    //Tundra,
    
    //TemperateDesert,
    //Shrubland,      // en edes tiedä mikä on Shrubland
    //Taiga,          // näiden tilalle hassuja biome/tileTypejä

    GrassLand, // norm caps
    //TemperateDeciduousForest,
    //TemperateRainForest,

    //SubtropicalDesert,
    //TropicalSeasonalForest,
    //TropicalRainForest,

    Forest,
    Jungle,
    Savannah,
    Desert,
    Snow
}

// ei käytössä atm
public class Tile
{
    public TileType Type;
    public int X;
    public int Y;

    public Tile(int x, int y)
    {
        Type = TileType.Invalid;
        this.X = x;
        Y = y;
    }
}
