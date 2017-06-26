using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteController : MonoBehaviour
{
    // wheels badman käyttää stringejä 
    private Dictionary<string, Sprite> _textures;
    GameObject[] temporarySecondLayer = new GameObject[100];

    public Sprite temproraryFillSprite;

    public Sprite grassFiller;
    public Sprite ForesFiller;


    void Awake()
    {
        // lataa kaikki johonkin vaikka dicciiin aluks
        // nopeampiakin ratkaisuja olisi
        _textures = new Dictionary<string, Sprite>(16);
        Sprite[] sprites = Resources.LoadAll<Sprite>("Tiles/");
        foreach (Sprite sprite in sprites)
        {
            _textures[sprite.name] = sprite;
            //print(sprite.name);
        }
        
        for(int i = 0; i < temporarySecondLayer.Length; i++)
        {
            temporarySecondLayer[i] = new GameObject();
            // go.SetActive(false);
            var renderer = temporarySecondLayer[i].AddComponent<SpriteRenderer>();
            renderer.sprite = temproraryFillSprite;
            temporarySecondLayer[i].transform.parent = gameObject.transform;
        }
    }

    //nurkat rajatapauksia / chunkkien välistä keskustelua
    //spawnatessa tasoita aivan kaikki
    //liikkuessa vain
    //ne tietyt saumat?

    //    public void InitChunkSprites(Chunk chunk)
    //    {
    //        // chunk.TileGameObjects

    //        int width = Chunk.CHUNK_SIZE;
    //        int height = Chunk.CHUNK_SIZE;

    //        // käy jokainen läpi ja valitse spritelle nimi!

    //        // looppa rajat erikseen ??
    //        // ei out of bounds checkkiä silloin



    //        for (int y = 0; y < height; y++)
    //        {
    //            for (int x = 1; x < width; x++)
    //            {

    //                TileType type = chunk.Tiles[y, x];
    //                //if (type != TileType.GrassLand)
    //                //{
    //                //    return; // vain GrassLand on implementattu atm!!!
    //                //}

    //#if false
    //                        string assetName = type.ToString() + "_";
    //#else
    //                string assetName = "GrassLand_";
    //#endif

    //                // bit mask // boolean Mask XD?
    //                if (chunk.Tiles[y + 1, x] == type)
    //                {
    //                    assetName += "N";
    //                }
    //                if (chunk.Tiles[y, x + 1] == type)
    //                {
    //                    assetName += "E";
    //                }
    //                if (chunk.Tiles[y - 1, x] == type)
    //                {
    //                    assetName += "S";
    //                }
    //                if (chunk.Tiles[y, x - 1] == type)
    //                {
    //                    assetName += "W";
    //                }

    //                Sprite sprite;
    //                if (_textures.TryGetValue(assetName, out sprite))
    //                {
    //                    chunk.TileGameObjects[y, x].GetComponent<SpriteRenderer>().sprite = sprite;
    //                }
    //                else
    //                {
    //                    Debug.LogWarning("Texture: " + assetName + " Missing!");
    //                    // TODO: default / debug texture / pink texture
    //                }
    //            }
    //        }
    //    }



    private static Vec2[] neighbours = {
        new Vec2(0, -1), new Vec2(0, 1),
        new Vec2(-1, 0),  new Vec2(1, 0)
    };

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
            for(int i = 0; i < neighbours.Length; i++)
            {
                int x = neighbours[i].X + pos.X;
                int y = neighbours[i].Y + pos.Y;

                int count = 0;
                string assetName = GetAssetName(x, y, tilemap.Tiles, out count);

                //if (count == 1 || count == 0)
                //{
                //    problemsCases.Add(new Vec2(x, y));
                //}

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
        // probleemojen
        // naapureiden uudestaan initointi

        DoubleLayerBorders(BorderTiles, tilemap.Tiles, tilemap.TileGameObjects, tilemap);
    }

    // TMP NEW CREATE!
    public bool TileWeight(TileType type, TileType type2)
    {
        return true;
    }

   

    public void DoubleLayerBorders(List<Vec2> borderTiles, TileType[,] tiles, GameObject[,] gameobjects, TileMap map) // border tile list 
    {


        Debug.Log("meilla on: " + borderTiles.Count);
        for (int i = 0; i < borderTiles.Count; i++)
        {
            if (true) // TODO: REAL WEIGHTING
            {
                var neighbour = GetFirstDifferentNeighbour(tiles, borderTiles[i].X, borderTiles[i].Y);
                //if (i % 10 == 0)
                //    Debug.Log(tile.ToString());

                if (i < 100)
                {
                    var renderer = temporarySecondLayer[i].GetComponent<SpriteRenderer>(); //  gameobjects[borderTiles[i].Y, borderTiles[i].X].GetComponent<SpriteRenderer>();

                    if (neighbour == TileType.Forest)
                    {
                        renderer.sprite = ForesFiller;
                    }
                    else if (neighbour == TileType.GrassLand)
                    {
                        renderer.sprite = grassFiller;
                    }

                    //gameobjects[borderTiles[i].Y, borderTiles[i].X].SetActive(false);
                    renderer.transform.position = new Vector3(borderTiles[i].X, borderTiles[i].Y);
                    renderer.gameObject.SetActive(true);

                    SetNeighbours(borderTiles[i].X, borderTiles[i].Y, map, tiles[borderTiles[i].Y, borderTiles[i].X]);
                }
            }
        }
    }

    public void SetNeighbours(int x, int y, TileMap map, TileType backgroundTile)
    {
        foreach(var pos in neighbours)
        {
            if (TileType.GrassLand == map.Tiles[y, x])
            {
                map.TileGameObjects[y + pos.Y, x + pos.X].GetComponent<SpriteRenderer>().sprite = grassFiller;
                Debug.LogWarning("called");
            }
        }
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
    private static readonly Vec2[] Diagonals = {
        new Vec2(-1, -1), new Vec2(1, -1),  // 
        new Vec2(-1, 1),  new Vec2(1, 1)
    };

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

    //foreach(var pos in problemsCases)
    //{
    //    tilemap.Tiles[pos.Y, pos.X] = // get most neighbours ???
    //}

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


// TEMPTEPMTPOEIJ OIASJ

//public void InitChunkSprites(int width, int height, TileMap tilemap, int startX, int startY)
//{
//    List<Vec2> problemsCases = new List<Vec2>(32);

//    for (int y = startY; y < height; y++)
//    {
//        for (int x = startX; x < width; x++)
//        {
//            //TileType type = tilemap.Tiles[y, x];

//            //string assetName = "GrassLand_";

//            //if (type == TileType.Mountain)
//            //{
//            //    assetName = type.ToString() + "_";
//            //}
//            //else
//            //{
//            //    type = TileType.GrassLand;
//            //}

//            // bit mask // boolean Mask XD?

//            //int count = 0;

//            //if (tilemap.Tiles[y + 1, x] == type)
//            //{
//            //    assetName += "N";
//            //    count++;
//            //}
//            //if (tilemap.Tiles[y, x + 1] == type)
//            //{
//            //    assetName += "E";
//            //    count++;
//            //}
//            //if (tilemap.Tiles[y - 1, x] == type)
//            //{
//            //    assetName += "S";
//            //    count++;
//            //}
//            //if (tilemap.Tiles[y, x - 1] == type)
//            //{
//            //    assetName += "W";
//            //    count++;
//            //}
//            int count = 0;
//            string assetName = GetAssetName(x, y, tilemap.Tiles, out count);

//            if (count == 1 || count == 0)
//            {
//                problemsCases.Add(new Vec2(x, y));
//            }

//            //if (count == 4)
//            //{
//            //    // int dir = GetDiagonals(tilemap.Tiles, x, y, type);

//            //    //if (dir != 0)
//            //    //{
//            //    //    assetName += "_d" + dir.ToString();
//            //    //}
//            //    //else
//            //    //{
//            //    //    assetName += "_" + Random.Range(0, 9).ToString();
//            //    //}

//            //    Sprite sprite;
//            //    if (_textures.TryGetValue(assetName, out sprite))
//            //    {
//            //        tilemap.TileGameObjects[y, x].GetComponent<SpriteRenderer>().sprite = sprite;
//            //    }
//            //}
//            //else

//            {
//                Sprite sprite;
//                if (_textures.TryGetValue(assetName, out sprite))
//                {
//                    tilemap.TileGameObjects[y, x].GetComponent<SpriteRenderer>().sprite = sprite;
//                }
//                else
//                {
//                    Debug.LogWarning("Texture: " + assetName + " Missing!");
//                }
//            }
//        }
//    }

//    print(problemsCases.Count);
//    foreach (var pos in problemsCases)
//    {
//        Debug.LogFormat("coords: {0}, {1}", pos.X, pos.Y);
//        tilemap.Tiles[pos.Y, pos.X] = GetMostCommonNeighbour(tilemap.Tiles, pos.Y, pos.X);
//    }