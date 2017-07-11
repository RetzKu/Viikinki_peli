using System.IO;
using System.Reflection.Emit;
using UnityEngine;

// must change start once scrolling
public class View<T>
{
    private T[,] _array;
    public int _startX;
    public int _startY;

    public int Size;
    public View(T[,] array, int startX, int startY, int size)
    {
        this._array = array;
        _startX = startX;
        _startY = startY;
        Size = size;
    }

    public T this[int y, int x]
    {
        get
        {
            if (x < 0 || Size >= x)
            {
                return _array[_startY + y, _startX + x];
            }
            else
            {
                Debug.LogError("viewin ulkopuolella " + x + " " + y);
            }
            return _array[10, 10];
        }
        set
        {
            // error check
            _array[_startY + y, _startX + x] = value;
        }
    }
}

public class Chunk      // sub array
{
    public static readonly int CHUNK_SIZE = 17;
    public static TileType[,] Tiles;
    public static GameObject[,] TileGameObjects;

    public int offsetX;
    public int offsetY;
    FileInfo f;

    public View<TileType> TilemapTilesView;
    public View<GameObject> GameObjectView;

    public static Sprite GrassSprite;

    public static void SwapViews(Chunk a, Chunk b)
    {
        View<TileType> temp = a.TilemapTilesView;
        a.TilemapTilesView = b.TilemapTilesView;
        b.TilemapTilesView = temp;
    

        //View<GameObject> tmp = a.GameObjectView;
        //a.GameObjectView = b.GameObjectView;
        //b.GameObjectView = tmp;
    }

    public void SetView(int startIndexX, int startIndexY)
    {
        TilemapTilesView._startX = startIndexX;
        TilemapTilesView._startY = startIndexY;

        // GameObjectView._startX = startIndexX;
        // GameObjectView._startY = startIndexY;
    }

    public void Init(int chunkOffsetX, int chunkOffsetY, Transform tilemap, TileType[,] tiles, GameObject[,] gameobjects, int viewStartXIndex, int viewStartYIndex)
    {
        // Debug.Log("x: " + viewStartXIndex);
        TilemapTilesView = new View<TileType>(tiles, viewStartXIndex, viewStartYIndex, CHUNK_SIZE); // 0 1 2    // SETVIEW;
        GameObjectView   = new View<GameObject>(gameobjects, viewStartXIndex, viewStartYIndex, CHUNK_SIZE);

        chunkOffsetX *= CHUNK_SIZE;
        chunkOffsetY *= CHUNK_SIZE;

        GameObject parent = new GameObject("CHUNKERS!");
        parent.transform.parent = tilemap;

        for (int y = 0; y < CHUNK_SIZE; y++)
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                TilemapTilesView[y, x] = TileType.Invalid;

                GameObject tileObject = new GameObject("(" + y + "," + x + ")");
                tileObject.transform.parent = parent.transform;
                tileObject.transform.position = new Vector3(x + chunkOffsetX, y + chunkOffsetY, 0);
                tileObject.layer = 9;

                GameObjectView[y, x] = tileObject;

                SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = GrassSprite; // GrassSprite[Random.Range(0, GrassSprite.Length)];                        // HUOM SPRITE CONTROLLER!!!!!
                spriteRenderer.sortingLayerName = "TileMap";


                var collider = tileObject.AddComponent<BoxCollider2D>();
                collider.enabled = false;
            }
        }
        offsetX = chunkOffsetX;
        offsetY = chunkOffsetY;
    }

    private TileType[,] GetArray()
    {
        TileType[,] value = new TileType[CHUNK_SIZE, CHUNK_SIZE];
        for (int i = 0; i < CHUNK_SIZE; i++)
        {
            for (int j = 0; j < CHUNK_SIZE; j++)
            {
                value[i, j] = TilemapTilesView[i, j];
            }
        }
        return value;
    }

    private void SetTypes(TileType[,] types)
    {
        for (int i = 0; i < CHUNK_SIZE; i++)
        {
            for (int j = 0; j < CHUNK_SIZE; j++)
            {
                TilemapTilesView[i, j] = types[i, j];
            }
        }
    }

    public string GetPath()
    {
        return "chunkdata" + offsetX.ToString() + "_" + offsetY.ToString();
    }

    public void Load()
    {
         SetTypes(TestWriter.Load(GetPath()));
    }

    public void Save()
    {
        TestWriter.Save(GetArray(), GetPath());
    }

    public void DisableChunkCollision()
    {
        for (int y = 0; y < GameObjectView.Size; y++)
        {
            for (int x = 0; x < GameObjectView.Size; x++)
            {
                GameObjectView[y, x].GetComponent<Collider2D>().enabled = false;
            }
        }
    }


    public void MoveChunk(int x, int y)
    {
        int dtOffsetX = CHUNK_SIZE * x;
        int dtOffsetY = CHUNK_SIZE * y;

        for (int yy = 0; yy < CHUNK_SIZE; yy++)
        {
            for (int xx = 0; xx < CHUNK_SIZE; xx++)
            {
                var go = GameObjectView[yy, xx];
                go.transform.position = new Vector3(go.transform.position.x + dtOffsetX, go.transform.position.y + dtOffsetY);
            }
        }
    }

    // näitä kutsuu todenkäköisesti vain TileMap class jonka kautta kaikki kommunikaatio
    // chunkeille tehdään! 
    public TileType GetTile(int x, int y)
    {
        return TilemapTilesView[y, x];
    }

    public void SetTile(int x, int y, TileType type)
    {
        TilemapTilesView[y, x] = type;
    }

    public GameObject GetGameObject(int x, int y)
    {
        return GameObjectView[y, x];
    }

    public void OnDrawGizmos()
    {
    }
}
