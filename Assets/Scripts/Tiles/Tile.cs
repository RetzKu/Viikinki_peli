﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Invalid,

    DeepWater,
    Water,
    Mountain,

    CaveWall, 

    CollisionTiles,



    CaveFloor,
    CaveDoor,
    Beach,
    //Scorhed,
    //Bare,
    //Tundra,
    //TemperateDesert,
    //Shrubland,      // en edes tiedä mikä on Shrubland
    //Taiga,          // näiden tilalle hassuja biome/tileTypejä

    Forest,
    GrassLand,

    Jungle,
    Savannah,
    Desert,
    Snow,
    Max
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
