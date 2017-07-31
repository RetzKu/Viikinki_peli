using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour, ITileMap
{
    public Sprite GrassSprite; // Note(Eetu): Spritet kannattaa varmaan eroitella toiseen scriptiin (ehkä?)

    private Perlin _perlinGenerator;
    public Chunk[,] _chunks;

    public static readonly int TotalWidth = Chunk.CHUNK_SIZE * 3;
    public static readonly int TotalHeight = Chunk.CHUNK_SIZE * 3;
    private int ChunkSize = Chunk.CHUNK_SIZE;

    public TileType[,] Tiles = new TileType[TotalHeight, TotalWidth]; // todo: w h laskeminen koosta
    public GameObject[,] TileGameObjects = new GameObject[TotalHeight, TotalWidth];

    public int Width { get; set; }  
    public int Height { get; set; }  



    [Header("kayta")]
    public bool tilemapPrototypeLayout = false;
    public float tilemapGenerationOffsetX = 0;
    public float tilemapGenerationOffsetY = 0;

    public bool useChunkSprites;
    public Sprite[] chunkTestSprites;

    private bool running = false;
    [Header("Sydeemit")]
    public TileSpriteController SpriteController;

    public Color tint;
    private Color last;

    void Start()
    {
        Width = TotalWidth;
        Height = TotalHeight;

        SpriteController = FindObjectOfType<TileSpriteController>();

        _perlinGenerator = GetComponent<Perlin>();
        _chunks = new Chunk[3, 3];

        Chunk.TileGameObjects = TileGameObjects;
        Chunk.Tiles = Tiles;

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                _chunks[y, x] = new Chunk();

                int viewIndexX = x * Chunk.CHUNK_SIZE;
                int viewIndexY = y * Chunk.CHUNK_SIZE;

                _chunks[y, x].Init(x, y, this.transform, Tiles, TileGameObjects, viewIndexX, viewIndexY);
            }
        }

        int initHeight = 3;
        int initWidth = 3;
        if (tilemapPrototypeLayout)
        {
            initHeight = 3;
            initWidth = 3;
        }

        for (int y = 0; y < initHeight; y++)
        {
            for (int x = 0; x < initWidth; x++)
            {
                GenerateChunk(x, y); // ei vällii?      // vanhaa koodia? 
            }
        }

        for (int x = 0; x < TotalWidth; x++)
        {
            TileGameObjects[0, x].GetComponent<SpriteRenderer>().sprite = GrassSprite;
            TileGameObjects[TotalHeight - 1, x].GetComponent<SpriteRenderer>().sprite = GrassSprite;
            TileGameObjects[x, 0].GetComponent<SpriteRenderer>().sprite = GrassSprite;
            TileGameObjects[x, TotalWidth - 1].GetComponent<SpriteRenderer>().sprite = GrassSprite;
        }

        SpriteController.transform.position = GetTileGameObject(0, 0).transform.position;
        SpriteController.SetTileSprites(TotalWidth - 2, TotalHeight - 2, this, 1, 1);
        running = true;

        last = tint;

        // SpriteController.gameObject.SetActive(false);
    }

    public void DisableTileMap()
    {
        gameObject.SetActive(false);
        // SpriteController.gameObject.SetActive(false);
    }

    public void EnableTileMap()
    {
        gameObject.SetActive(true);
        // SpriteController.gameObject.SetActive(true);
    }

    public void Tint()
    {
        if (last != tint)
        {
            foreach (var go in TileGameObjects)
            {
                go.GetComponent<SpriteRenderer>().material.color = tint;
                last = tint;
            }
        }
    }

    public static bool Collides(TileType type)
    {
        return (type <= TileType.CollisionTiles);
    }

    public bool GetTileCollision(int x, int y)
    {
        int ix = x / Chunk.CHUNK_SIZE;
        int iy = y / Chunk.CHUNK_SIZE;
        return Collides(_chunks[iy, ix].GetTile(x - ChunkSize * ix, y - ChunkSize * iy)) || _chunks[iy, ix].TileCollides(x - ChunkSize * ix, y - ChunkSize * iy);
    }

    public Sprite[] TestTiles;
    void RandomizeAllTiles()
    {
        foreach (var tile in TileGameObjects)
        {
            var spriteRenderer = tile.GetComponent<SpriteRenderer>();
            // spriteRenderer = new SpriteRenderer();
            spriteRenderer.sprite = TestTiles[Random.Range(0, TestTiles.Length)];
            spriteRenderer.color = Color.white;
            spriteRenderer.material.color = Color.white;
        }
    }

    void ResetColor()
    {
        foreach (var tile in TileGameObjects)
        {
            var spriteRenderer = tile.GetComponent<SpriteRenderer>();
            spriteRenderer.material.color = Color.white;
        }
    }

    public static Vec2 GetChunkOffset(float x, float y)
    {
        int chunkOffsetX = (int)(x / Chunk.CHUNK_SIZE);
        int chunkOffsetY = (int)(y / Chunk.CHUNK_SIZE);

        return new Vec2(chunkOffsetX, chunkOffsetY);
    }

    public static int GetChunkOffset(float coord)
    {
        return (int)(coord / Chunk.CHUNK_SIZE);
    }

    // oletetaan että viewrange on aina pienempi kuin chunk
    public void UpdateTilemap<T>(T player, int viewRange) where T : MonoBehaviour, ITestPlayer  // tmp  
    {
        if (tilemapPrototypeLayout)
            return;

        int chunkOffsetX = GetChunkOffset(player.transform.position.x);
        int chunkOffsetY = GetChunkOffset(player.transform.position.y);

        if (chunkOffsetX != player.ChunkOffsets.X || chunkOffsetY != player.ChunkOffsets.Y)
        {
            _chunkGenerationInProcess = true;
            StartCoroutine(SetChunkTimer(5));
            //Debug.LogFormat("player chunk changed to({0}:{1})", chunkOffsetX, chunkOffsetY);

            int chunkDtX = chunkOffsetX - player.ChunkOffsets.X;
            int chunkDtY = chunkOffsetY - player.ChunkOffsets.Y;

            player.ChunkOffsets.X = chunkOffsetX;
            player.ChunkOffsets.Y = chunkOffsetY;

            if (chunkDtX < 0) // vasen
            {
                // StartCoroutine(ThreeFrameUpdateLeft(chunkOffsetX, chunkOffsetY));
                swapColumn(2, 1);
                swapColumn(1, 0);

                for (int i = -1; i < 2; i++)    // -1
                {
                    _chunks[i + 1, 0].DisableChunkCollision();

                    GenerateChunk(0, i + 1, chunkOffsetX - 1, chunkOffsetY + i);
                    _chunks[i + 1, 0].MoveChunk(-3, 0); // moves gos
                }

                SpriteController.transform.position = GetTileGameObject(0, 0).transform.position; //  + new Vector3(0f, 17f);
                SpriteController.SetTileSprites(Chunk.CHUNK_SIZE - 3, Chunk.CHUNK_SIZE * 3 - 3, this, 1, 1);

            }
            else if (chunkDtX > 0)      // oikealle
            {
                // StartCoroutine(ThreeFrameUpdateRight(chunkOffsetX, chunkOffsetY));
                swapColumn(1, 0);
                swapColumn(2, 1);

                for (int i = -1; i < 2; i++)    // -1
                {
                    _chunks[i + 1, 2].DisableChunkCollision();

                    GenerateChunk(2, i + 1, chunkOffsetX + 1, chunkOffsetY + i);
                    _chunks[i + 1, 2].MoveChunk(3, 0);
                }

                SpriteController.transform.position = GetTileGameObject(0, 0).transform.position; //  + new Vector3(0f, 17f);
                SpriteController.SetTileSprites(Chunk.CHUNK_SIZE * 3 - 3, Chunk.CHUNK_SIZE * 3 - 3, this, Chunk.CHUNK_SIZE * 2 - 3, 1);
            }
            if (chunkDtY < 0) // alas
            {
                swapRow(2, 1);
                swapRow(1, 0);

                // CopyChunks();

                for (int i = -1; i < 2; i++)    // -1
                {
                    _chunks[0, i + 1].DisableChunkCollision();

                    GenerateChunk(i + 1, 0, chunkOffsetX + i, chunkOffsetY - 1);
                    _chunks[0, i + 1].MoveChunk(0, -3);

                }


                SpriteController.transform.position = GetTileGameObject(0, 0).transform.position;
                SpriteController.SetTileSprites(Chunk.CHUNK_SIZE * 3 - 3, Chunk.CHUNK_SIZE - 3, this, 1, 1);
            }
            else if (chunkDtY > 0)  // ylös
            {
                swapRow(1, 0);
                swapRow(2, 1);

                // CopyChunks();

                for (int i = -1; i < 2; i++)    // -1
                {
                    _chunks[2, i + 1].DisableChunkCollision();

                    GenerateChunk(i + 1, 2, chunkOffsetX + i, chunkOffsetY + 1);
                    _chunks[2, i + 1].MoveChunk(0, 3);
                }

                // koska ylös mennessä 0, 0 nousee
                SpriteController.transform.position = GetTileGameObject(0, 0).transform.position; //  + new Vector3(0f, 17f);
                SpriteController.SetTileSprites(Chunk.CHUNK_SIZE * 3 - 2, Chunk.CHUNK_SIZE * 3 - 2, this, 1, Chunk.CHUNK_SIZE * 2 - 2);
                // SpriteController.SetTileSprites(Chunk.CHUNK_SIZE * 3 - 3, Chunk.CHUNK_SIZE * 3 - 3, this, 1, Chunk.CHUNK_SIZE * 2 - 3);
            }
            SpriteController.transform.position = GetTileGameObject(0, 0).transform.position;


            //SpriteController.SetTileSprites(59, 59, this, 1, 1);
        }

        // _chunks[1, 1].offsetX = chunkOffsetX;   // ainoastaa center chunk on oikeassa chunkissa atm
        // _chunks[1, 1].offsetY = chunkOffsetY;
    }

    IEnumerator SetChunkTimer(int frames)
    {
        for (int i = 0; i < frames; i++)
            yield return null;

        _chunkGenerationInProcess = false;
    }



    IEnumerator ThreeFrameUpdateRight(int chunkOffsetX, int chunkOffsetY)
    {
        swapColumn(1, 0);
        swapColumn(2, 1);

        for (int i = -1; i < 2; i++)    // -1
        {
            _chunks[i + 1, 2].DisableChunkCollision();

            GenerateChunk(2, i + 1, chunkOffsetX + 1, chunkOffsetY + i);
            _chunks[i + 1, 2].MoveChunk(3, 0);
            yield return null;
        }

        SpriteController.transform.position = GetTileGameObject(0, 0).transform.position; //  + new Vector3(0f, 17f);
        SpriteController.SetTileSprites(Chunk.CHUNK_SIZE * 3 - 3, Chunk.CHUNK_SIZE * 3 - 3, this, Chunk.CHUNK_SIZE * 2 - 3, 1);
    }


    IEnumerator ThreeFrameUpdateLeft(int chunkOffsetX, int chunkOffsetY)
    {
        swapColumn(2, 1);
        swapColumn(1, 0);

        for (int i = -1; i < 2; i++)    // -1
        {
            _chunks[i + 1, 0].DisableChunkCollision();

            GenerateChunk(0, i + 1, chunkOffsetX - 1, chunkOffsetY + i);
            _chunks[i + 1, 0].MoveChunk(-3, 0); // moves gos
            yield return null;
        }

        SpriteController.transform.position = GetTileGameObject(0, 0).transform.position; //  + new Vector3(0f, 17f);
        SpriteController.SetTileSprites(Chunk.CHUNK_SIZE - 3, Chunk.CHUNK_SIZE * 3 - 3, this, 1, 1);
    }

    void SwapColumnsViews(int destX, int fromX)
    {
        for (int iY = 0; iY < 3; iY++)
        {
            Chunk from = _chunks[iY, fromX];

            for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
            {
                for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
                {
                    Tiles[iY * Chunk.CHUNK_SIZE, destX * Chunk.CHUNK_SIZE + x] = from.TilemapTilesView[y, x];
                }
            }
        }
    }

    void ResetTileViews()
    {
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                _chunks[y, x].SetView(x * Chunk.CHUNK_SIZE, y * Chunk.CHUNK_SIZE);
            }
        }
    }

    void CopyChunks()
    {
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                int iY = 0;
                for (int yy = 0; yy < Chunk.CHUNK_SIZE; yy++, iY++)
                {
                    int iX = 0;
                    for (int xx = 0; xx < Chunk.CHUNK_SIZE; xx++, iX++)
                    {
                        Tiles[iY, iX] = _chunks[y, x].TilemapTilesView[yy, xx];
                    }
                }
                // _chunks[y, x].SetView(y * Chunk.CHUNK_SIZE, x * Chunk.CHUNK_SIZE);
            }
        }
    }

    void swapRow(int y, int newY)
    {
        for (int i = 0; i < 3; i++)
        {
            swapChunks(i, i, y, newY);
        }
    }

    void swapColumn(int x, int newX)
    {
        for (int i = 0; i < 3; i++)
        {
            swapChunks(x, newX, i, i);
        }
    }

    void swapChunks(int offsetX, int newOffsetX, int offsetY, int newOffsetY)
    {
        // fuckihng algot
        //int startY = offsetY * Chunk.CHUNK_SIZE;
        //int startX = offsetX * Chunk.CHUNK_SIZE;
        //int destinationStartX = newOffsetX * Chunk.CHUNK_SIZE;
        //int destinationStartY = newOffsetY * Chunk.CHUNK_SIZE;

        //for (int fromY = startY, destY = destinationStartY; fromY < startY + Chunk.CHUNK_SIZE; fromY++, destY++)
        //{
        //    for (int fromX = startX, destX = destinationStartX; fromX < startX + Chunk.CHUNK_SIZE; fromX++, destX++)
        //    {
        //        Tiles[destY, destX] = Tiles[fromY, fromX];
        //    }
        //}
        // reset chunk views=?=?===?=??=???????

        Chunk tmp = _chunks[offsetY, offsetX];
        _chunks[offsetY, offsetX] = _chunks[newOffsetY, newOffsetX];
        _chunks[newOffsetY, newOffsetX] = tmp;

        //Chunk.SwapViews(_chunks[offsetY, offsetX], _chunks[newOffsetY, newOffsetX]);
    }

    Dictionary<Vec2, bool> SavedChunks = new Dictionary<Vec2, bool>(25);
    void GenerateChunk(int offsetX, int offsetY, int perlinOffsetX, int perlinOffsetY)
    {
        // tallenna entinen
        MobsControl.instance.SpawnBoids(perlinOffsetX * ChunkSize + ChunkSize / 2, perlinOffsetY * ChunkSize + ChunkSize / 2, ChunkSize / 3, Random.Range(2, 6));//pate spawn

        var chunk = _chunks[offsetY, offsetX]; // missä kohdalla _chunkeissa

        SavedChunks[new Vec2(chunk.offsetX, chunk.offsetY)] = true;
        chunk.Save();

        chunk.OnChunkChangedCleanup();
        bool exist = false;
        if (SavedChunks.TryGetValue(new Vec2(perlinOffsetX, perlinOffsetY), out exist))
        {
            chunk.offsetX = perlinOffsetX;
            chunk.offsetY = perlinOffsetY;
            chunk.Load();
        }
        else
        {
            _perlinGenerator.GenerateChunk(_chunks[offsetY, offsetX], perlinOffsetX, perlinOffsetY);
        }
        chunk.offsetX = perlinOffsetX;
        chunk.offsetY = perlinOffsetY;

    }


    // TMP TODO: DELETE!!!
    void GenerateChunk(int offsetX, int offsetY)
    {
        _perlinGenerator.GenerateChunk(_chunks[offsetY, offsetX], offsetX, offsetY);
    }

    void Update()
    {
        Tint();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _chunks[1, 1].Save();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _chunks[1, 1].Load();
        }
    }

    public enum Dir { Rigth, Left }
    public static readonly Vec2[] Data =
    {
        new Vec2(1, 1), new Vec2(1, 0),
    };

    private bool _chunkGenerationInProcess = false;

    void SwapLeft()
    {
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
            {
                Tiles[y, x] = Tiles[y, x + Chunk.CHUNK_SIZE];
                Tiles[y, x + Chunk.CHUNK_SIZE] = Tiles[y, x + 40];
            }
        }
    }

    void SwapRight()
    {
        for (int y = 3; y > 1; y--)
        {
            for (int x = 59; x > 39; x--)
            {
                Tiles[y, x] = Tiles[y, x - Chunk.CHUNK_SIZE];
                Tiles[y, x - Chunk.CHUNK_SIZE] = Tiles[y, x - 40];
            }
        }
    }

    // NOTE(Eetu):
    // jos funktioiden matematiikka on liian raskasta niin on vielä mahdollista optimoida se muutamalla kikalla
    // raskas toiminnallisuuss liittyen tiilien looppaamiseen kannattaa sijoittaa chunkkeihin suoraan
    // tarkoitettu lähinnä tiilien vaihtoon / yksittäisiin tiili operaatioihin

    public TileType GetTile(float x, float y)                   // TODO: FIX THESE
    {
        int chunkX = 1;
        int chunkY = 1;
        GetChunkOffsetXY(ref chunkX, ref chunkY, x, y);

        int offsetX = (int)x % Chunk.CHUNK_SIZE;
        int offsetY = (int)y % Chunk.CHUNK_SIZE;
        return _chunks[chunkY, chunkX].GetTile(offsetX, offsetY);
    }

    public TileType GetTile(Vector2 vec)
    {
        int chunkX = 1;
        int chunkY = 1;
        GetChunkOffsetXY(ref chunkX, ref chunkY, vec.x, vec.y);

        int offsetX = (int)vec.x % Chunk.CHUNK_SIZE;
        int offsetY = (int)vec.y % Chunk.CHUNK_SIZE;
        return _chunks[chunkY, chunkX].GetTile(offsetX, offsetY);
    }

    public GameObject GetGo(Vector2 vec)
    {
        int chunkX = 1;
        int chunkY = 1;
        GetChunkOffsetXY(ref chunkX, ref chunkY, vec.x, vec.y);

        int offsetX = (int)vec.x % Chunk.CHUNK_SIZE;
        int offsetY = (int)vec.y % Chunk.CHUNK_SIZE;
        return _chunks[chunkY, chunkX].GetGameObject(offsetX, offsetY);
    }

    public GameObject GetTileOnTileGameObject(Vector2 vec)
    {
        int chunkX = 1;
        int chunkY = 1;
        GetChunkOffsetXY(ref chunkX, ref chunkY, vec.x, vec.y);

        int offsetX = (int)vec.x % Chunk.CHUNK_SIZE;
        int offsetY = (int)vec.y % Chunk.CHUNK_SIZE;
        return _chunks[chunkY, chunkX].GetTileOnTileGameObject(offsetX, offsetY);
    }


    public void SetTile(float x, float y, TileType type)
    {
        int chunkX = 1;
        int chunkY = 1;
        GetChunkOffsetXY(ref chunkX, ref chunkY, x, y);

        int offsetX = (int)x % Chunk.CHUNK_SIZE;
        int offsetY = (int)y % Chunk.CHUNK_SIZE;
        _chunks[chunkY, chunkX].SetTile(offsetX, offsetY, type);
    }

    public GameObject GetTileGameObject(int x, int y)
    {
        int ix = x / Chunk.CHUNK_SIZE;
        int iy = y / Chunk.CHUNK_SIZE;
        return _chunks[iy, ix].GetGameObject(x - ChunkSize * ix, y - ChunkSize * iy);
    }

    public TileType GetTileFast(Vector2 position)
    {
        return GetTile((int)position.x, (int)position.y);
    }

    //public GameObject GetTileGameObject(int x, int y)
    //{
    //    throw new System.NotImplementedException();
    //}

    public TileType GetTile(int x, int y)
    {
        int ix = x / Chunk.CHUNK_SIZE;
        int iy = y / Chunk.CHUNK_SIZE;
        return _chunks[iy, ix].GetTile(x - ChunkSize * ix, y - ChunkSize * iy);
    }


    public GameObject GetTileGameObject(float x, float y)
    {
        int chunkX = 1;
        int chunkY = 1;
        GetChunkOffsetXY(ref chunkX, ref chunkY, x, y);

        int offsetX = (int)x % Chunk.CHUNK_SIZE;
        int offsetY = (int)y % Chunk.CHUNK_SIZE;
        return _chunks[chunkY, chunkX].GetGameObject(offsetX, offsetY);
    }

    private void GetChunkOffsetXY(ref int x, ref int y, float worldX, float worldY)
    {
        if (worldX > _chunks[1, 1].offsetX * Chunk.CHUNK_SIZE + Chunk.CHUNK_SIZE)
        {
            ++x;
        }
        else if (worldX < _chunks[1, 1].offsetX * Chunk.CHUNK_SIZE)
        {
            --x;
        }

        if (worldY > _chunks[1, 1].offsetY * Chunk.CHUNK_SIZE + Chunk.CHUNK_SIZE)
        {
            ++y;
        }
        else if (worldY < _chunks[1, 1].offsetY * Chunk.CHUNK_SIZE)
        {
            --y;
        }
    }

    public bool CanUpdatePathFind()
    {
        return !_chunkGenerationInProcess;
    }
}
