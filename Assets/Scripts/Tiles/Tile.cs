using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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
