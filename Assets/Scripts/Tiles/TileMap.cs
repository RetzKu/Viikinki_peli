using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public int Width;
    public int Height;
    public Sprite GrassSprite; // Note(Eetu): Spritet kannattaa varmaan eroitella toiseen scriptiin (ehkä?)

    private Dictionary<Tile, GameObject> _tileGameObjects;  // Tällä saatas takas maailmassa oleva GameObject
    private Tile[,] _tiles;                                 // En tiiä tuntuu mausteikkaalta ratkaisulta(hyvältä)
    private Perlin _perlinGenerator;

    public Chunk[,] _chunks; // TODO: object pool
    public bool TilemapDebug = true;
    private bool running = false;

    void Start()
    {
        _tileGameObjects = new Dictionary<Tile, GameObject>(Height * Width);    // TODO: widht height rikki atm
        _tiles = new Tile[Height, Width];

        GameObject parent = new GameObject("Tiles");

        if (TilemapDebug)
        {
            for (int y = 0; y < Width; y++)
            {
                for (int x = 0; x < Height; x++)
                {
                    _tiles[y, x] = new Tile(x, y);

                    GameObject tileObject = new GameObject("(" + y + "," + x + ")");
                    tileObject.transform.parent = parent.transform;
                    tileObject.transform.position = new Vector3(y, x, 0);

                    SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = GrassSprite;
                    spriteRenderer.sortingLayerName = "TileMap";

                    _tileGameObjects.Add(_tiles[y, x], tileObject);
                }
            }
        }

        running = true;
        _perlinGenerator = GetComponent<Perlin>();
        _chunks = new Chunk[3, 3];

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                _chunks[y, x] = new Chunk();        // tmp
                _chunks[y, x].Init(x, y);
            }
        }

        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 2; x++)
            {
                GenerateChunk(x, y); // ei vällii?
            }
        }
    }

    void OnDrawGizmos()
    {
        if (running)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    int halfChunk = Chunk.CHUNK_SIZE / 2;
                    Vector3 center = new Vector3(halfChunk + x * Chunk.CHUNK_SIZE - 0.5f, halfChunk + y * Chunk.CHUNK_SIZE - 0.5f);
                    Vector3 bounds = new Vector3(Chunk.CHUNK_SIZE, Chunk.CHUNK_SIZE);
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(center, bounds);
                }
            }
        }
    }

    // oleetaan että viewrange on aina pienempi kuin chunk
    public void UpdateTilemap<T>(T player, int viewRange) where T : MonoBehaviour, ITestPlayer  // tmp  
    {
        // calculate current Chunk coords
        int chunkOffsetX = (int)(player.transform.position.x / Chunk.CHUNK_SIZE);
        int chunkOffsetY = (int)(player.transform.position.y / Chunk.CHUNK_SIZE);

        if (chunkOffsetX != player.ChunkOffsets.X || chunkOffsetY != player.ChunkOffsets.Y)
        {
            Debug.LogFormat("player chunk changed to({0}:{1})", chunkOffsetX, chunkOffsetY);



            int chunkDtX = chunkOffsetX - player.ChunkOffsets.X;
            int chunkDtY = chunkOffsetY - player.ChunkOffsets.Y;

            player.ChunkOffsets.X = chunkOffsetX;
            player.ChunkOffsets.Y = chunkOffsetY;

            if (chunkDtX < 0)
            {
                for (int i = -1; i < 2; i++)    // -1
                {
                    GenerateChunk(chunkOffsetX - 1, chunkOffsetY + i);
                    Debug.LogFormat("gen: {0}:{1}", chunkOffsetX - 1, chunkOffsetY + i);
                }
            }
            else if (chunkDtX > 0)
            {
                for (int i = -1; i < 2; i++)    // -1
                {
                    GenerateChunk(chunkOffsetX + 1, chunkOffsetY + i);
                    Debug.LogFormat("gen: {0}:{1}", chunkOffsetX + 1, chunkOffsetY + i);
                }
            }

            if (chunkDtY < 0)
            {
                for (int i = -1; i < 2; i++)    // -1
                {
                    GenerateChunk(chunkOffsetX + i, chunkOffsetY - 1);
                }
            }
            else if (chunkDtY > 0)
            {
                for (int i = -1; i < 2; i++)    // -1
                    GenerateChunk(chunkOffsetX + i, chunkOffsetY + 1);
            }
        }
        // onko viewiin tullut uusi chunk
    }

    void GenerateChunk(int offsetX, int offsetY) // mitkä ???
    {
        // ota yhteys Perliiiniin     
        // _chunks[offsetY, offsetX].Init(offsetX, offsetY);   
        _perlinGenerator.GenerateChunk(_chunks[offsetY, offsetX], offsetX, offsetY);
    }

    void LoadChunk()
    {
        // just gen   
    }

    void SaveChunk()
    {
        // nothings
    }


    public Tile GetTile(float x, float y)
    {
        return GetTile((int)x, (int)y);
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || x > Width || y < 0 || y > Height)           // Note(Eetu): nyt aluksi jotai mutta sitten kun perlin noise saa liikkumaan 
            Debug.LogError("TileMap::GetTile Out of bounds");    // niin kenttä voisi teoriassa olla rajaton

        return _tiles[y, x];
    }

    public GameObject GetTileGameObject(Tile tile)
    {
        GameObject gameObject;
        if (_tileGameObjects.TryGetValue(tile, out gameObject))
        {
            return gameObject;
        }
        else
        {
            Debug.LogError("No GameObject Exist");
        }
        return gameObject;
    }

    public GameObject GetTileGameObject(int x, int y)
    {
        GameObject gameObject;
        if (_tileGameObjects.TryGetValue(GetTile(x, y), out gameObject))
        {
            return gameObject;
        }
        else
        {
            Debug.LogError("No GameObject Exist");
        }
        return gameObject;
    }
}
