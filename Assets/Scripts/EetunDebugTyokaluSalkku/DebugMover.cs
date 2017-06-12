using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DebugMover : MonoBehaviour
{
    public float Speed = 1f;
    public float DebugViewRange = 16f; // range per suunta 8f

    private Rigidbody2D body;
    private Vector2 vel;

    private TileMap tilemap;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(DebugViewRange, DebugViewRange, DebugViewRange));
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        tilemap = FindObjectOfType<TileMap>();
    }

	void Update ()
    {
        vel = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        vel *= Speed;
        body.MovePosition(body.position + vel);

        tilemap.UpdateTilemap(this);
    }

    void FixedUpdate()
    {
    }
}
