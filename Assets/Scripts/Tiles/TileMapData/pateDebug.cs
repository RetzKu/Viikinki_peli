using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pateDebug : MonoBehaviour
{

    // Use this for initialization
    Rigidbody2D body;
    public GameObject door;

    private TileMap tilemap;
    private bool _tilemapActive = true;

    void Start()
    {
        tilemap = FindObjectOfType<TileMap>();
    }

    void Update()
    {


       
    }
}
