using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//      load/generate Chunks of world

public class Chunk 
{
    public bool debugDrawChunk = false;
    public static int CHUNK_SIZE = 20;

    public Sprite GrassSprite;

    private Tile[,] _tiles;
    private Dictionary<Tile, GameObject> _tileGameObjects;

    private int offsetX;
    private int offsetY;

    public void Init(int chunkOffsetX, int chunkOffsetY)
    {
        _tileGameObjects = new Dictionary<Tile, GameObject>(CHUNK_SIZE * CHUNK_SIZE);
        _tiles = new Tile[CHUNK_SIZE, CHUNK_SIZE];
        GrassSprite = Resources.Load<Sprite>("Dummy_Tile");
        // GrassSprite.

        chunkOffsetX *= CHUNK_SIZE;
        chunkOffsetY *= CHUNK_SIZE;

        GameObject parent = new GameObject("CHUNKERS!");
        for (int y = 0; y < CHUNK_SIZE; y++)
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                _tiles[y, x] = new Tile(x, y);

                GameObject tileObject = new GameObject("(" + y + "," + x + ")");
                tileObject.transform.parent = parent.transform;
                tileObject.transform.position = new Vector3( x + chunkOffsetX, y + chunkOffsetY, 0);

                SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = GrassSprite;
                spriteRenderer.sortingLayerName = "TileMap";
                // spriteRenderer.shader

                _tileGameObjects.Add(_tiles[y, x], tileObject);
            }
        }

        offsetX = chunkOffsetX;
        offsetY = chunkOffsetY;
    }

    // näitä kutsuu todenkäköisesti vain TileMap class jonka kautta kaikki kommunikaatio
    // chunkeille tehdään!
    public Tile GetTile(int x, int y)
    {
        return _tiles[y, x];
    }

    public GameObject GetGameObject(int x, int y) // hiaman hitaampi kuin Tile varitaatio (jota ei ole olemassa(vielä))
    {
        return _tileGameObjects[GetTile(x, y)];
    }

    public void OnDrawGizmos()
    {
       
    }
}
