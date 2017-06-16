using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Vec2
{
    public int X;
    public int Y;
    public Vec2(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public interface ITestPlayer
{
   Vec2 ChunkOffsets { get; set; }
}

public class ChunkMover : MonoBehaviour, ITestPlayer
{
    public float DebugViewRange = 16f; // range per suunta 8f
    private TileMap tilemap;
    public Vec2 ChunkOffsets { get; set; }
    private int _viewRange = 8;


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(DebugViewRange, DebugViewRange, DebugViewRange));
    }

    void Start()
    {
        tilemap = FindObjectOfType<TileMap>();
        ChunkOffsets = TileMap.GetChunkOffset (transform.position.x, transform.position.y);
    }

    void Update ()
    {
        tilemap.UpdateTilemap(this, _viewRange);
    }
}
