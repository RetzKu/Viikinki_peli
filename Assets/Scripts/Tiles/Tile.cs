using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Empty,
    Grass,
    Sea, 
    Pate
}

public class Tile
{
    public TileType Type;
    public int X;
    public int Y;

    public Tile(int x, int y)
    {

        Type = TileType.Empty;
        X = x;
        Y = y;
    }
}
