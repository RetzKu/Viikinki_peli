using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteController : MonoBehaviour
{
    // wheels badman käyttää stringejä 
    private Dictionary<string, Sprite> _textures;
    GameObject[] temporarySecondLayer = new GameObject[450];    // LOL


    public SpriteControllerSettings Settings;

    int[] TileCount = new int[(int)TileType.Max];

    void Awake()
    {
        // lataa kaikki johonkin vaikka dicciiin aluks
        // nopeampiakin ratkaisuja olisi
        _textures = new Dictionary<string, Sprite>(16);
        Sprite[] sprites = Resources.LoadAll<Sprite>("Tiles/");
        foreach (Sprite sprite in sprites)
        {
            //  _textures[sprite.name] = sprite;
            //  print(sprite.name);
        }

        for (int i = 0; i < temporarySecondLayer.Length; i++)
        {
            temporarySecondLayer[i] = new GameObject();
            // go.SetActive(false);
            var renderer = temporarySecondLayer[i].AddComponent<SpriteRenderer>();
            temporarySecondLayer[i].transform.parent = gameObject.transform;
            renderer.sortingLayerName = "TileMapBorder";
        }

        foreach (var tile in Settings.SpriteData)
        {
            if (tile.Sprites.Length == 0)
                continue;

            string name = tile.Type.ToString();
            TileCount[(int) tile.Type] = tile.Sprites.Length;

            int count = 0;
            for (int i = 0; i < tile.Sprites.Length - 4; i++)
            {
                _textures[name + stringsEnds[i]] = tile.Sprites[i];
                print(_textures[name + stringsEnds[i]]);
                count++;
            }
            TileCount[(int) tile.Type] = count;

            for (int i = tile.Sprites.Length - 4, stringEndIndex = 9; i < tile.Sprites.Length; i++, stringEndIndex++)
            {
                _textures[name + stringsEnds[stringEndIndex]] = tile.Sprites[i];
                print(_textures[name + stringsEnds[stringEndIndex]]);
            }
        }
    }

    private readonly string[] stringsEnds = new string[13]
    {
        "_0", "_1", "_2", "_3", "_4", "_5", "_6", "_7", "_8", "_E0", "_N0", "_S0", "_W0"
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

    //public enum TileType
    //{
    //    Invalid,
    //    Water,
    //    DeepWater,
    //    Mountain,

    //    CollisionTiles,
    //    Beach,
    //    GrassLand, // norm caps
    //    Forest,
    //    Jungle,
    //    Savannah,
    //    Desert,
    //    Snow
    //}

    //private static readonly int[,] tileVisualRules = {
    //    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    //    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    //    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    //};

    bool IsUpperTile(TileType upper, TileType down)
    {
        if (upper == down) return true;
        return upper > down;  // TODO: Kunnon säännöt
    }


    string GetAssetNameBitmask(int x, int y, TileMap Tilemap, out int value, out bool found)
    {
        found = false;
        TileType type = Tilemap.GetTileFast(x, y);

        string assetName = "";

        if (type == TileType.Mountain || type == TileType.GrassLand || type == TileType.Water)
        {
            assetName = type.ToString() + "_";
            found = true;
        }

        if (type == TileType.Forest)
        {
            assetName = "Forest_";
            found = true;
        }

        // 4-bit directions
        // North, west, east, south
        value = 0;
        if (IsUpperTile(type, Tilemap.GetTileFast(x, y + 1)))  // Tähän reunan insert
        {
            value += 1;
        }
        if (IsUpperTile(type, Tilemap.GetTileFast(x + 1, y)))
        {
            value += 4;
        }
        if (IsUpperTile(type, Tilemap.GetTileFast(x, y - 1)))
        {
            value += 8;
        }
        if (IsUpperTile(type, Tilemap.GetTileFast(x - 1, y)))
        {
            value += 2;
        }
        // asset randilla reuna ton mukaan
        // assetName += value.ToString();  // TODO: lisää reuna
        // Reunojen alle sitä viereistä ???
        return assetName;
    }

    string GetAssetName(int x, int y, TileType[,] tiles, out int count)
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

    public int GetAssetCount(TileType type)
    {
        return TileCount[(int)type];

        //  if (type == TileType.Water)
        //{
        //    return 1;
        //}
        //return 9;
    }

    public void SetTileSprites(int width, int height, TileMap tilemap, int startX, int startY)
    {
        int tempIndex = 0;
        float offsetX = transform.position.x;
        float offsetY = transform.position.y;

        for (int y = startY; y < height; y++)
        {
            for (int x = startX; x < width; x++)
            {
                int value = 0;
                bool found;

                string assetName = GetAssetNameBitmask(x, y, tilemap, out value, out found);
                TileType type = tilemap.GetTileFast(x, y);  // TODO: optimization opportunity

                Sprite sprite;  // perus 

                if (!found)
                    assetName = "GrassLand_";
                assetName += Random.Range(0, GetAssetCount(type));    // max count smt smt 


                if (_textures.TryGetValue(assetName, out sprite))
                {
                    tilemap.GetGameObjectFast(x, y).GetComponent<SpriteRenderer>().sprite = sprite;
                }
                else
                {
                    Debug.LogWarning("Texture: " + assetName + " Missing!");
                }

                // lisää: reunat
                // 4-bit directions
                // North, west, east, south
                if (value == 15)
                {
                    continue;
                }

                if (tempIndex < temporarySecondLayer.Length)
                {
                    string border = "GrassLand";

                    if (type == TileType.GrassLand || type == TileType.Water || type == TileType.Mountain || type == TileType.Forest)
                    {
                        border = type.ToString();
                    }

                    //if (type == TileType.Forest)
                    //{
                    //    border = "tileset_ground_";
                    //}

                    print(border);
                    if ((value & (1 << 1 - 1)) == 0)
                    {
                        temporarySecondLayer[tempIndex].GetComponent<SpriteRenderer>().sprite = _textures[border + "_N0"];
                        temporarySecondLayer[tempIndex].SetActive(true);
                        temporarySecondLayer[tempIndex].transform.position = new Vector3(x + offsetX, y + offsetY + 1);
                        tempIndex++;
                    }
                    if ((value & (1 << 2 - 1)) == 0)
                    {
                        temporarySecondLayer[tempIndex].GetComponent<SpriteRenderer>().sprite = _textures[border + "_W0"];
                        temporarySecondLayer[tempIndex].transform.position = new Vector3(x + offsetX - 1, y + offsetY);
                        temporarySecondLayer[tempIndex].SetActive(true);
                        tempIndex++;
                    }
                    if ((value & (1 << 3 - 1)) == 0)
                    {
                        temporarySecondLayer[tempIndex].GetComponent<SpriteRenderer>().sprite = _textures[border + "_E0"];
                        temporarySecondLayer[tempIndex].transform.position = new Vector3(x + offsetX + 1, y + offsetY);
                        temporarySecondLayer[tempIndex].SetActive(true);
                        tempIndex++;
                    }
                    if ((value & (1 << 4 - 1)) == 0)
                    {
                        temporarySecondLayer[tempIndex].GetComponent<SpriteRenderer>().sprite = _textures[border + "_S0"];
                        temporarySecondLayer[tempIndex].transform.position = new Vector3(x + offsetX, y + offsetY - 1);
                        temporarySecondLayer[tempIndex].SetActive(true);
                        tempIndex++;
                    }
                }
            }
        }
        for (int i = tempIndex; i < temporarySecondLayer.Length; i++)
        {
            temporarySecondLayer[i].active = false;
        }
    }

    public void InitChunkSprites(int width, int height, TileMap tilemap, int startX, int startY)
    {
        List<Vec2> problemsCases = new List<Vec2>(32);
        List<Vec2> BorderTiles = new List<Vec2>(512);

        for (int y = startY; y < height; y++)
        {
            for (int x = startX; x < width; x++)
            {
                int count = 0;
                string assetName = GetAssetName(x, y, tilemap.Tiles, out count);

                if (count == 1 || count == 0)
                {
                    problemsCases.Add(new Vec2(x, y));
                }
                if (count < 4)
                {
                    BorderTiles.Add(new Vec2(x, y));
                }

                Sprite sprite;
                if (_textures.TryGetValue(assetName, out sprite))
                {
                    tilemap.TileGameObjects[y, x].GetComponent<SpriteRenderer>().sprite = sprite;
                }
                else
                {
                    Debug.LogWarning("Texture: " + assetName + " Missing!");
                }
            }
        }

        print(problemsCases.Count);
        foreach (var pos in problemsCases)
        {
            Debug.LogFormat("coords: {0}, {1}", pos.X, pos.Y);
            tilemap.Tiles[pos.Y, pos.X] = GetMostCommonNeighbour(tilemap.Tiles, pos.X, pos.Y);

            //int count = 0;
            //string assetName = GetAssetName(pos.X, pos.Y, tilemap.Tiles, out count);

            //if (count == 1 || count == 0)
            //{
            //    problemsCases.Add(new Vec2(pos.X, pos.Y));
            //}

            //Sprite sprite;
            //if (_textures.TryGetValue(assetName, out sprite))
            //{
            //    tilemap.TileGameObjects[pos.Y, pos.Y].GetComponent<SpriteRenderer>().sprite = sprite;
            //}
            //else
            //{
            //    Debug.LogWarning("Texture: " + assetName + " Missing!");
            //}

            // TODO: FUNKTIO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //for(int i = 0; i < neighbours.Length; i++)
            //{
            //    int x = neighbours[i].X + pos.X;
            //    int y = neighbours[i].Y + pos.Y;

            //    int count = 0;
            //    // string assetName = GetAssetName(x, y, tilemap.Tiles, out count);

            //    //if (count == 1 || count == 0)
            //    //{
            //    //    problemsCases.Add(new Vec2(x, y));
            //    //}

            //    Sprite sprite;
            //    if (_textures.TryGetValue(assetName, out sprite))
            //    {
            //        tilemap.TileGameObjects[y, x].GetComponent<SpriteRenderer>().sprite = sprite;
            //    }
            //    else
            //    {
            //        Debug.LogWarning("Texture: " + assetName + " Missing!");
            //    }
            //}
        }
        // probleemojen
        // naapureiden uudestaan initointi
        //DoubleLayerBorders(BorderTiles, tilemap.Tiles, tilemap.TileGameObjects, tilemap);
    }

    // TMP NEW CREATE!
    public bool TileWeight(TileType type, TileType type2)
    {
        return true;
    }

    //public void DoubleLayerBorders(List<Vec2> borderTiles, TileType[,] tiles, GameObject[,] gameobjects, TileMap map) // border tile list 
    //{
    //    Debug.Log("meilla on: " + borderTiles.Count);
    //    for (int i = 0; i < borderTiles.Count; i++)
    //    {
    //        if (true) // TODO: REAL WEIGHTING
    //        {
    //            var neighbour = GetFirstDifferentNeighbour(tiles, borderTiles[i].X, borderTiles[i].Y);
    //            //if (i % 10 == 0)
    //            //    Debug.Log(tile.ToString());

    //            if (i < 100)
    //            {
    //                var renderer = temporarySecondLayer[i].GetComponent<SpriteRenderer>(); //  gameobjects[borderTiles[i].Y, borderTiles[i].X].GetComponent<SpriteRenderer>();

    //                if (neighbour == TileType.Forest)
    //                {
    //                    renderer.sprite = ForesFiller;
    //                }
    //                else if (neighbour == TileType.GrassLand)
    //                {
    //                    renderer.sprite = grassFiller;
    //                }

    //                renderer.transform.position = new Vector3(borderTiles[i].X, borderTiles[i].Y);
    //                renderer.gameObject.SetActive(true);

    //                SetNeighbours(borderTiles[i].X, borderTiles[i].Y, map, tiles[borderTiles[i].Y, borderTiles[i].X]);
    //            }
    //        }
    //    }
    //}

    //public void SetNeighbours(int x, int y, TileMap map, TileType backgroundTile)
    //{
    //    foreach (var pos in neighbours)
    //    {
    //        if (TileType.GrassLand == map.Tiles[y, x])
    //        {
    //            map.TileGameObjects[y + pos.Y, x + pos.X].GetComponent<SpriteRenderer>().sprite = grassFiller;
    //            Debug.LogWarning("called");
    //        }
    //    }
    //}

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

    // tehdään iso oletus generaatiosta
    // proper funktio tarvitaan? joissain rajatapauksissa
    // vertaile edes kaikkia XD

    TileType GetMostCommonNeighbour(TileType[,] tiles, int x, int y)
    {
        TileType value = TileType.Invalid;

        TileType[] count = new TileType[4]    // NESW
        {
            TileType.Invalid, TileType.Invalid, TileType.Invalid, TileType.Invalid
        };

        for (int i = 0; i < 4; i++)
        {
            TileType neighbour = tiles[neighbours[i].Y + y, neighbours[i].X + x];
            count[i] = neighbour;
        }

        for (int i = 0; i < 3; i++)
        {
            if (count[i] == count[i + 1])
            {
                value = count[i];
                break;
            }
        }

        if (value == TileType.Invalid)
        {
            Debug.LogWarning("missing " + value);
        }

        return value;
    }
}
