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



    public void InitChunkSprites(int width, int height, TileMap tilemap, int startX, int startY)
    {
        for (int y = startY; y < height; y++)
        {
            for (int x = startX; x < width; x++)
            {
                TileType type = tilemap.Tiles[y, x];
#if false
                        string assetName = type.ToString() + "_";
#else
                string assetName = "GrassLand_";
#endif

                // bit mask // boolean Mask XD?
                if (tilemap.Tiles[y + 1, x] == type)
                {
                    assetName += "N";
                }
                if (tilemap.Tiles[y, x + 1] == type)
                {
                    assetName += "E";
                }
                if (tilemap.Tiles[y - 1, x] == type)
                {
                    assetName += "S";
                }
                if (tilemap.Tiles[y, x - 1] == type)
                {
                    assetName += "W";
                }

                

                Sprite sprite;
                if (_textures.TryGetValue(assetName, out sprite))
                {
                    tilemap.TileGameObjects[y, x].GetComponent<SpriteRenderer>().sprite = sprite;
                }
                else
                {
                    Debug.LogWarning("Texture: " + assetName + " Missing!");
                    // TODO: default / debug texture / pink texture
                }
            }
        }
    }

    
}
