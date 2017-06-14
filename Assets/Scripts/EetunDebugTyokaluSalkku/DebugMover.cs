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

public class DebugMover : MonoBehaviour, ITestPlayer
{
    public float Speed = 1f;
    public float DebugViewRange = 16f; // range per suunta 8f

    private Rigidbody2D body;
    private Vector2 vel;

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
        body = GetComponent<Rigidbody2D>();

        tilemap = FindObjectOfType<TileMap>();

        ChunkOffsets = new Vec2(0, 0);
    }

    void Update ()
    {
        vel = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        vel *= Speed;
        body.MovePosition(body.position + vel);

        tilemap.UpdateTilemap(this, _viewRange);
    }
}
