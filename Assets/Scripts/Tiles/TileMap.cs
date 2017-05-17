using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public int Width;
    public int Heigth;
    public Sprite GrassSprite;

    private Dictionary<Tile, GameObject> _tileGameObjects; // Tällä saatas takas maailmassa oleva GameObject
    private Tile[,] _tiles;


    void Start()
    {
        _tileGameObjects = new Dictionary<Tile, GameObject>(Heigth * Width);
        _tiles = new Tile[Heigth, Width];

        GameObject parent = new GameObject("Tiles");

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Heigth; y++)
            {
                _tiles[x, y] = new Tile(x, y);

                GameObject tileObject = new GameObject("(" + x + "," + y + ")");
                tileObject.transform.parent = parent.transform;
                tileObject.transform.position = new Vector3(x, y, 0);

                SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = GrassSprite;

                _tileGameObjects.Add(_tiles[x, y], tileObject);
            }
        }
    }

    void Update()
    {
        // Dunno ? 
    }
}
