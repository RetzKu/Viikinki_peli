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

    void Start()
    {
        _tileGameObjects = new Dictionary<Tile, GameObject>(Height * Width);
        _tiles = new Tile[Height, Width];

        GameObject parent = new GameObject("Tiles");

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

                _tileGameObjects.Add(_tiles[x, y], tileObject);
            }
        }
    }

    void Update()
    {
        // Dunno ? 
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
