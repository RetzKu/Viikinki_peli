using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Chunk 
{
    public bool debugDrawChunk = false;
    public static int CHUNK_SIZE = 20;

    public Sprite GrassSprite;

    public TileType[,] _tiles;
    private GameObject[,] _tileGameObjects;
   
    // TODO: // ainoastaa center chunk on oikeassa chunkissa atm
    public int offsetX;
    public int offsetY;

    FileInfo f;

    public void Init(int chunkOffsetX, int chunkOffsetY, Transform tilemap)
    {
        _tiles = new TileType[CHUNK_SIZE, CHUNK_SIZE];
        _tileGameObjects = new GameObject[CHUNK_SIZE, CHUNK_SIZE];


        GrassSprite = Resources.Load<Sprite>("Dummy_Tile");
        // GrassSprite.

        chunkOffsetX *= CHUNK_SIZE;
        chunkOffsetY *= CHUNK_SIZE;

        GameObject parent = new GameObject("CHUNKERS!");
        parent.transform.parent = tilemap;

        for (int y = 0; y < CHUNK_SIZE; y++)
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                _tiles[y, x] = TileType.Invalid;

                GameObject tileObject = new GameObject("(" + y + "," + x + ")");
                tileObject.transform.parent = parent.transform;
                tileObject.transform.position = new Vector3(x + chunkOffsetX, y + chunkOffsetY, 0);
                tileObject.layer = 9;


                SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = GrassSprite;
                spriteRenderer.sortingLayerName = "TileMap";

                _tileGameObjects[y, x] = tileObject;

                var collider = tileObject.AddComponent<BoxCollider2D>();
                
                collider.enabled = false;
            }
        }
        offsetX = chunkOffsetX;
        offsetY = chunkOffsetY;
    }

    public string path = "saveFile.data";
    public void Save()
    {
        //StreamWriter w;
        //if (!f.Exists)
        //{
        //    w = f.CreateText();
        //}
        //else
        //{
        //    f.Delete();
        //    w = f.CreateText();
        //}
        //w.WriteLine(_tiles[0,0]);
        //w.Close();
        //  var test = (byte)_tiles[0,0];
        //File.WriteAllBytes("test.data", (byte[])_tiles);

        //TileType[] array = new TileType[10];

        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        BinaryWriter bw = new BinaryWriter(fs);
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                bw.Write((int)_tiles[i, j]);
            }
        }
        bw.Close();
        fs.Close();
        //File.WriteAllBytes("ads", (int[,])_tiles); 
    }

    public void Load()
    {
        using (BinaryReader b = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            int pos = 0;
            int length = (int)b.BaseStream.Length;
            while (pos < length)
            {
                int v = b.ReadInt32();
                Debug.Log(v);
                pos += sizeof(int);
            }
        }
    }


    public void disableChunkCollision()
    {
        foreach (var go in _tileGameObjects)
        {
            go.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void MoveChunk(int x, int y)
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

    public void SetTile(int x, int y, TileType type)
    {
        _tiles[y, x] = type;
    }

    public GameObject GetGameObject(int x, int y) 
    {
        return _tileGameObjects[y, x];
    }

    public void OnDrawGizmos()
    {
    }
}
