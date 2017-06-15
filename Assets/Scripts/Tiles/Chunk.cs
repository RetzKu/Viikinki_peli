using System.Collections.Generic;
using UnityEngine;

public class Chunk 
{
    public bool debugDrawChunk = false;
    public static int CHUNK_SIZE = 20;

    public Sprite GrassSprite;


    private TileType[,] _tiles;
    private GameObject[,] _tileGameObjects;

    private int offsetX;
    private int offsetY;

    public void Init(int chunkOffsetX, int chunkOffsetY)
    {
        // _tileGameObjects = new Dictionary<TileType, GameObject>(CHUNK_SIZE * CHUNK_SIZE);
        _tiles = new TileType[CHUNK_SIZE, CHUNK_SIZE];
        _tileGameObjects = new GameObject[CHUNK_SIZE, CHUNK_SIZE];


        GrassSprite = Resources.Load<Sprite>("Dummy_Tile");
        // GrassSprite.

        chunkOffsetX *= CHUNK_SIZE;
        chunkOffsetY *= CHUNK_SIZE;

        GameObject parent = new GameObject("CHUNKERS!");
        for (int y = 0; y < CHUNK_SIZE; y++)
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                _tiles[y, x] = TileType.Invalid;

                GameObject tileObject = new GameObject("(" + y + "," + x + ")");
                tileObject.transform.parent = parent.transform;
                tileObject.transform.position = new Vector3( x + chunkOffsetX, y + chunkOffsetY, 0);

                SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = GrassSprite;
                spriteRenderer.sortingLayerName = "TileMap";
                // spriteRenderer.shader

                _tileGameObjects[y, x] = tileObject;
                // _tileGameObjects.Add(_tiles[y, x], tileObject);
            }
        }
        offsetX = chunkOffsetX;
        offsetY = chunkOffsetY;
    }

    public void moveChunk(int x, int y)
    {
        int dtOffsetX = CHUNK_SIZE * x;
        int dtOffsetY = CHUNK_SIZE * y;

        foreach(var go in _tileGameObjects)
        {
            go.transform.position = new Vector3(go.transform.position.x + dtOffsetX, go.transform.position.y + dtOffsetY, go.transform.position.z);
        }

        offsetX = dtOffsetX;
        offsetY = dtOffsetY;
    }

    // näitä kutsuu todenkäköisesti vain TileMap class jonka kautta kaikki kommunikaatio
    // chunkeille tehdään!
    public TileType GetTile(int x, int y)
    {
        return _tiles[y, x];
    }

    public GameObject GetGameObject(int x, int y) 
    {
        return _tileGameObjects[y, x];
    }

    public void OnDrawGizmos()
    {
       
    }
}
