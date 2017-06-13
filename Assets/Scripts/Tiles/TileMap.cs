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

    public bool TilemapDebug = false;

    void Start()
    {
        _tileGameObjects = new Dictionary<Tile, GameObject>(Height * Width);
        _tiles = new Tile[Height, Width];

        GameObject parent = new GameObject("Tiles");

        if (TilemapDebug)
        {

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _tiles[x, y] = new Tile(x, y);

                    GameObject tileObject = new GameObject("(" + x + "," + y + ")");
                    tileObject.transform.parent = parent.transform;
                    tileObject.transform.position = new Vector3(x, y, 0);

                    SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = GrassSprite;
                    spriteRenderer.sortingLayerName = "TileMap";

                    _tileGameObjects.Add(_tiles[x, y], tileObject);
                }
            }
        }

        _perlinGenerator = GetComponent<Perlin>();
        _chunks = new Chunk[3, 3]; // ehkä, ehkä ei

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                _chunks[y, x] = new Chunk();
                _chunks[y, x].Init(x, y);
                GenerateChunk(x, y); // ei vällii
            }
        }
    }

    void Update()
    {
        // Dunno ? 
    }

    // if out of chunk check  in player aka           % chunkSize or something
    // chunk Stuff ????
    // player gets Chunk coordinates ? 
    public void UpdateTilemap<T>(T player) where T : MonoBehaviour  // temp
    {
        // calculate current Chunk coords
        // Player.transform.position.x 

        // TODO: missä on origo? 
        int chunkOffsetX = (int)(player.transform.position.x / Chunk.CHUNK_SIZE);
        int chunkOffsetY = (int)(player.transform.position.y / Chunk.CHUNK_SIZE);

        Debug.LogFormat("player chunk X:{0}, Y:{1}", chunkOffsetX, chunkOffsetY);
    }


    void GenerateChunk(int offsetX, int offsetY) // mitkä ???
    {
        // ota yhteys Perliiiniin        
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

        return _tiles[x, y];
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
