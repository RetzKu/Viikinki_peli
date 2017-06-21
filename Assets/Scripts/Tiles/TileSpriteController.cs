using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteController : MonoBehaviour
{
    // wheels badman käyttää stringejä 
    private Dictionary<string, Sprite> _textures;

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

    public void InitChunkSprites(int width, int height, TileMap tilemap, int startX, int startY)
    {
        List<Vec2> problemsCases = new List<Vec2>(32);

        for (int y = startY; y < height; y++)
        {
            for (int x = startX; x < width; x++)
            {
                TileType type = tilemap.Tiles[y, x];

                string assetName = "GrassLand_";

                if (type == TileType.Mountain)
                {
                    assetName = type.ToString() + "_";
                }
                //else
                //{
                //    type = TileType.GrassLand;
                //}

                // bit mask // boolean Mask XD?
                
                int count = 0;
                if (tilemap.Tiles[y + 1, x] == type)
                {
                    assetName += "N";
                    count++;
                }
                if (tilemap.Tiles[y, x + 1] == type)
                {
                    assetName += "E";
                    count++;
                }
                if (tilemap.Tiles[y - 1, x] == type)
                {
                    assetName += "S";
                    count++;
                }
                if (tilemap.Tiles[y, x - 1] == type)
                {
                    assetName += "W";
                    count++;
                }

                if (count == 1 || count == 0)
                {
                    // ongelma tapaus
                    // tilemap.Tiles[y, x] = TileType.Invalid;
                    problemsCases.Add(new Vec2(x, y));
                }

                if (count == 4)
                {
                    assetName += "_" + Random.Range(0, 9).ToString();
                    Sprite sprite;
                    if (_textures.TryGetValue(assetName, out sprite))
                    {
                        tilemap.TileGameObjects[y, x].GetComponent<SpriteRenderer>().sprite = sprite;
                    }
                }
                else
                {
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
        }

        print(problemsCases.Count);
        foreach ( var pos in problemsCases)
        {
            Debug.LogFormat("coords: {0}, {1}", pos.X, pos.Y);
            tilemap.Tiles[pos.Y, pos.X] = GetMostCommonNeighbour(tilemap.Tiles, pos.Y, pos.X);
        }
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
            if (count[i] == count[i+1])
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
