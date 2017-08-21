using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileSpriteController : MonoBehaviour
{
    private Dictionary<string, Sprite> _textures;

    public SpriteControllerSettings Settings;

    int[] TileCount = new int[(int)TileType.Max];
    bool[] Implemented = new bool[(int)TileType.Max];

    private static readonly int StringEndNumberCount = 9;

    private string[] EnumNames = new string[(int)TileType.Max];
    private string[] numbers = new string[StringEndNumberCount];

    private GameObject _borders;


    public Sprite[] Mountains;
    public Sprite[] FlatMountains;

    public enum CurrentPool
    {
        Border,
        Border1,
        Border2,
        Max
    }
    private CurrentPool currentPool = CurrentPool.Border;

    string GetCurrentPool()
    {
        if (currentPool == CurrentPool.Max)
        {
            currentPool = CurrentPool.Border;
        }
        return currentPool++.ToString();
    }

    // TODO: ONKO T}YNN};s
    private List<GameObject> borders = new List<GameObject>(0);

    private Dictionary<TileType, Sprite[]> _textures2 = new Dictionary<TileType, Sprite[]>(10);

    void Awake()
    {
        _borders = new GameObject("Borders");
        // _borders.transform.parent = this.transform;
        // lataa kaikki johonkin vaikka dicciiin aluks
        // nopeampiakin ratkaisuja olisi
        _textures = new Dictionary<string, Sprite>(16);
        //Sprite[] sprites = Resources.LoadAll<Sprite>("Tiles/");

        //for (int i = 0; i < temporarySecondLayer.Length; i++)
        //{
        //    temporarySecondLayer[i] = new GameObject();
        //    // go.SetActive(false);
        //    var renderer = temporarySecondLayer[i].AddComponent<SpriteRenderer>();
        //    temporarySecondLayer[i].transform.parent = gameObject.transform;
        //    renderer.sortingLayerName = "TileMapBorder";
        //}

        foreach (var tile in Settings.SpriteData)
        {
            if (tile.Sprites.Length == 0)
                continue;

            Implemented[(int)tile.Type] = true;

            string name = tile.Type.ToString();
            TileCount[(int)tile.Type] = tile.Sprites.Length;

            int count = 0;

            Sprite[] spritesArray = new Sprite[13];
            for (int i = 0; i < tile.Sprites.Length - 4; i++)
            {
                _textures[name + stringsEnds[i]] = tile.Sprites[i];
                spritesArray[i] = tile.Sprites[i];
                count++;
            }
            TileCount[(int)tile.Type] = count;

            for (int i = tile.Sprites.Length - 4, stringEndIndex = 9; i < tile.Sprites.Length; i++, stringEndIndex++)
            {
                _textures[name + stringsEnds[stringEndIndex]] = tile.Sprites[i];
                spritesArray[i] = tile.Sprites[i];
            }

            _textures2[tile.Type] = spritesArray;
        }

        for (int i = 0; i < (int)TileType.Max; i++)
        {
            EnumNames[i] = ((TileType)i).ToString() + "_";
        }

        for (int i = 0; i < StringEndNumberCount; i++)
        {
            numbers[i] = i.ToString();
        }
    }

    string GetEnumName(TileType type)
    {
        return EnumNames[(int)type];
    }

    string GetNumberName(int n)
    {
        return numbers[n];
    }

    // dictionary arrayseen, jossa assetit oikeilla paikoilla ?

    private readonly string[] stringsEnds = new string[13]
    {
        "_0", "_1", "_2", "_3", "_4", "_5", "_6", "_7", "_8", "_S0", "_W0", "_N0", "_E0"
    };

    string GetAssetNameByte(int x, int y, TileType[,] tiles, out int count)
    {
        count = 0;
        TileType type = tiles[y, x];
        string assetName = "GrassLand_";

        if (type == TileType.Mountain)
        {
            assetName = type.ToString() + "_";
        }
        if (tiles[y + 1, x] == type)
        {
            assetName += "N";
            count++;
        }
        if (tiles[y, x + 1] == type)
        {
            assetName += "E";
            count++;
        }
        if (tiles[y - 1, x] == type)
        {
            assetName += "S";
            count++;
        }
        if (tiles[y, x - 1] == type)
        {
            assetName += "W";
            count++;
        }

        if (count == 4)
        {
            int dir = GetDiagonals(tiles, x, y, type);

            if (dir != 0)
            {
                assetName += "_d" + dir.ToString();
            }
            else
            {
                assetName += "_" + Random.Range(0, 9).ToString();
            }
        }
        return assetName;
    }

    private static Vec2[] neighbours = {
        new Vec2(0, -1), new Vec2(0, 1),
        new Vec2(-1, 0),  new Vec2(1, 0)
    };

    private static readonly Vec2[] Diagonals = {
        new Vec2(-1, -1), new Vec2(1, -1),  // 
        new Vec2(-1, 1),  new Vec2(1, 1)
    };

    bool IsImplemented(TileType type)
    {
        return Implemented[(int)type];
    }

    bool IsUpperTile(TileType upper, TileType down)
    {
        if (upper == down) return true;
        return (int)upper > (int)down;  // TODO: Kunnon säännöt
    }

    string GetAssetNameBitmask(int x, int y, TileMap Tilemap, TileType type, out int value, out bool found)
    {
        found = false;
        string assetName = "";
        if (IsImplemented(type))
        {
            assetName = GetEnumName(type);
            found = true;
        }

        // 4-bit directions
        // North, west, east, south
        value = 0;
        if (IsUpperTile(type, Tilemap.GetTile(x, y + 1)))  // Tähän reunan insert
        {
            value += 1;
        }
        if (IsUpperTile(type, Tilemap.GetTile(x + 1, y)))
        {
            value += 4;
        }
        if (IsUpperTile(type, Tilemap.GetTile(x, y - 1)))
        {
            value += 8;
        }
        if (IsUpperTile(type, Tilemap.GetTile(x - 1, y)))
        {
            value += 2;
        }
        return assetName;
    }

    int GetAssetNameBitmaskNoStr(int x, int y, ITileMap Tilemap, TileType type)
    {
        // found = IsImplemented(type) ? true : false;
        // 4-bit directions
        // North, west, east, south
        int value = 0;
        if (IsUpperTile(type, Tilemap.GetTile(x, y + 1)))  // Tähän reunan insert
        {
            value += 1;
        }
        if (IsUpperTile(type, Tilemap.GetTile(x + 1, y)))
        {
            value += 4;
        }
        if (IsUpperTile(type, Tilemap.GetTile(x, y - 1)))
        {
            value += 8;
        }
        if (IsUpperTile(type, Tilemap.GetTile(x - 1, y)))
        {
            value += 2;
        }
        return value;
    }

    string GetAssetName(int x, int y, TileType[,] tiles, out int count)
    {
        count = 0;
        TileType type = tiles[y, x];
        string assetName = "GrassLand_";

        if (type == TileType.Mountain)
        {
            assetName = GetEnumName(type);
        }

        if (tiles[y + 1, x] == type)
        {
            assetName += "N";
            count++;
        }
        if (tiles[y, x + 1] == type)
        {
            assetName += "E";
            count++;
        }
        if (tiles[y - 1, x] == type)
        {
            assetName += "S";
            count++;
        }
        if (tiles[y, x - 1] == type)
        {
            assetName += "W";
            count++;
        }

        if (count == 4)
        {
            int dir = GetDiagonals(tiles, x, y, type);

            if (dir != 0)
            {
                assetName += "_d" + dir.ToString();
            }
            else
            {
                assetName += "_" + Random.Range(0, 9).ToString();
            }
        }
        return assetName;
    }

    public int GetAssetCount(TileType type)
    {
        return TileCount[(int)type];
    }


    public void ResetAllTiles()
    {
        for (int i = 0; i < borders.Count; i++)
        {
            var border = borders[i];
            {
                border.transform.parent = null;
                ObjectPool.instance.PoolObject(border);
            }
        }
        borders.Clear();
    }

    public enum TileDir
    {
        N = 14,
        S = 7,
        E = 13,
        W = 11,
    }

    public void SetTileSprites(int width, int height, ITileMap tilemap, int startX, int startY)
    {
        // int tempIndex = 0;
        float offsetX = tilemap.GetTileGameObject(0, 0).transform.position.x;
        float offsetY = tilemap.GetTileGameObject(0, 0).transform.position.y;

        // sstring currentPool = GetCurrentPool();
        string currentPool = "Border";

        for (int y = startY; y < height; y++)
        {
            for (int x = startX; x < width; x++)
            {
                TileType type = tilemap.GetTile(x, y);

    #if false    
                   if (type == TileType.Water || type == TileType.DeepWater)
                    {
                        tilemap.GetTileGameObject(x, y).GetComponent<Animator>().enabled = true;
                        return;
                    }
                    else
                    {
                        tilemap.GetTileGameObject(x, y).GetComponent<Animator>().enabled = false;
                    }
    #endif

                if (type == TileType.Mountain) // outdoor mountain ===??)==??=9
                {
                    int tile = GetAssetNameBitmaskNoStr(x, y, tilemap, type);
                    if (tile == 15)
                    {
                        tilemap.GetTileGameObject(x, y).GetComponent<SpriteRenderer>().sprite = FlatMountains[Random.Range(0, FlatMountains.Length - 1)];
                    }
                    else
                    {
                        tilemap.GetTileGameObject(x, y).GetComponent<SpriteRenderer>().sprite = Mountains[tile];
                    }

                    if (Perlin.CanPlaceDoor && (tile == (int)TileDir.N || tile == (int)TileDir.S || tile == (int)TileDir.E || tile == (int)TileDir.W))
                    {
                        var go = Instantiate(Perlin.DoorHackGameObject);
                        go.transform.position = tilemap.GetTileGameObject(x, y).transform.position;
                        go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, ZlayerManager.GetZFromY(go.transform.position));
                        // tilemap.AddLater_hack(x, y, go);
                        print("door placed");
                        Perlin.CanPlaceDoor = false;

                        // translate hack
                        switch (tile)
                        {
                            case (int)TileDir.N:
                                go.transform.Translate(Vector3.up);
                                break;
                            case (int)TileDir.S:
                                go.transform.Translate(Vector3.down);
                                break;
                            case (int)TileDir.E:
                                go.transform.Translate(Vector3.left);
                                break;
                            case (int)TileDir.W:
                                go.transform.Translate(Vector3.right);
                                break;
                            default:
                                break;
                        }
                    }
                    continue;
                }

                // normals:
                int value = GetAssetNameBitmaskNoStr(x, y, tilemap, type);

                if (IsImplemented(type))
                    tilemap.GetTileGameObject(x, y).GetComponent<SpriteRenderer>().sprite = _textures2[type][(Random.Range(0, GetAssetCount(type)))];

                if (value == 15)    // ei voi tulla enää reunoja
                {
                    continue;
                }

                // lisää: reunat
                // 4-bit directions
                // North, west, east, south
#if false
                if (tempIndex < temporarySecondLayer.Length - 1)
                {
                    ObjectPool.instance.GetObjectForType("Border", true);

                    string border = "GrassLand_";

                    if (IsImplemented(type))
                    {
                        border = GetEnumName(type);
                    }

                    if ((value & (1 << 1 - 1)) == 0)            // TODO: TEXTURES2 TÄNNE
                    {
                        temporarySecondLayer[tempIndex].GetComponent<SpriteRenderer>().sprite = _textures[border + "N0"];
                        temporarySecondLayer[tempIndex].SetActive(true);
                        temporarySecondLayer[tempIndex].transform.position = new Vector3(x + offsetX, y + offsetY + 1);
                        tempIndex++;
                    }
                    if ((value & (1 << 2 - 1)) == 0)
                    {
                        temporarySecondLayer[tempIndex].GetComponent<SpriteRenderer>().sprite = _textures[border + "W0"];
                        temporarySecondLayer[tempIndex].transform.position = new Vector3(x + offsetX - 1, y + offsetY);
                        temporarySecondLayer[tempIndex].SetActive(true);
                        tempIndex++;
                    }
                    if ((value & (1 << 3 - 1)) == 0)
                    {
                        temporarySecondLayer[tempIndex].GetComponent<SpriteRenderer>().sprite = _textures[border + "E0"];
                        temporarySecondLayer[tempIndex].transform.position = new Vector3(x + offsetX + 1, y + offsetY);
                        temporarySecondLayer[tempIndex].SetActive(true);
                        tempIndex++;
                    }
                    if ((value & (1 << 4 - 1)) == 0)
                    {
                        temporarySecondLayer[tempIndex].GetComponent<SpriteRenderer>().sprite = _textures[border + "S0"];
                        temporarySecondLayer[tempIndex].transform.position = new Vector3(x + offsetX, y + offsetY - 1);
                        temporarySecondLayer[tempIndex].SetActive(true);
                        tempIndex++;
                    }
#else

                string border = "GrassLand_";

                if (IsImplemented(type))
                {
                    border = GetEnumName(type);
                }

                if ((value & (1 << 1 - 1)) == 0)            // TODO: TEXTURES2 TÄNNE
                {
                    var go = ObjectPool.instance.GetObjectForType(currentPool, false);
                    go.GetComponent<SpriteRenderer>().sprite = _textures[border + "N0"];
                    go.transform.position = new Vector3(x + offsetX, y + offsetY + 1);
                    go.transform.parent = _borders.transform;
                    borders.Add(go);
                }

                if ((value & (1 << 2 - 1)) == 0)
                {
                    var go = ObjectPool.instance.GetObjectForType(currentPool, false);
                    go.GetComponent<SpriteRenderer>().sprite = _textures[border + "W0"];
                    go.transform.position = new Vector3(x + offsetX - 1, y + offsetY);
                    go.transform.parent = _borders.transform;
                    borders.Add(go);
                }

                if ((value & (1 << 3 - 1)) == 0)
                {
                    var go = ObjectPool.instance.GetObjectForType(currentPool, false);
                    go.GetComponent<SpriteRenderer>().sprite = _textures[border + "E0"];
                    go.transform.position = new Vector3(x + offsetX + 1, y + offsetY);
                    go.transform.parent = _borders.transform;
                    borders.Add(go);
                }

                if ((value & (1 << 4 - 1)) == 0)
                {
                    var go = ObjectPool.instance.GetObjectForType(currentPool, false);
                    go.GetComponent<SpriteRenderer>().sprite = _textures[border + "S0"];
                    go.transform.position = new Vector3(x + offsetX, y + offsetY - 1);
                    go.transform.parent = _borders.transform;
                    borders.Add(go);
                }
#endif
            }
        }

        // tarkista rajat
        for (int i = 0; i < borders.Count; i++)
        {
            var border = borders[i];
            if (offsetX > border.transform.position.x ||
                offsetX + TileMap.TotalWidth < border.transform.position.x ||
                offsetY > border.transform.position.y ||
                offsetY + TileMap.TotalHeight < border.transform.position.y)
            {
                // border.transform.parent = null;
                border.transform.parent = null;
                ObjectPool.instance.PoolObject(border);
                borders.RemoveAt(i);
                i--;
            }
        }
        // fuck fuck
    }

    // TMP NEW CREATE!
    [Obsolete]
    public bool TileWeight(TileType type, TileType type2)
    {
        return true;
    }


    // oletetaan että biomeja on vain kahta eriä vierekkäin
    public TileType GetFirstDifferentNeighbour(TileType[,] tiles, int x, int y)
    {
        TileType selfType = tiles[y, x];
        for (int i = 0; i < 4; i++)
        {
            TileType type = tiles[neighbours[i].Y + y, neighbours[i].X + x];
            if (type != selfType)
            {
                return type;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            TileType type = tiles[y + Diagonals[i].X, x + Diagonals[i].Y];
            if (type != selfType)
            {
                return type;
            }
        }
        return TileType.Invalid;
    }

    // todo: replace with enum
    // ala vasen   //   ylä vasaen    // ala oikea  // ala vasen
    int GetDiagonals(TileType[,] types, int x, int y, TileType selfType)
    {
        for (int i = 0; i < 4; i++)
        {
            var type = types[y + Diagonals[i].X, x + Diagonals[i].Y];
            if (type != selfType)
            {
                return i + 1;
            }
        }
        return 0;
    }

    // reshapre ternary test XDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD:
    void bar(bool b, bool c, bool d, bool e, bool f)
    {
        var pate = b ? (f ? -1 : 2) : (c ? (e ? 5 : 3) : (d ? 4 : 100));
    }
}
