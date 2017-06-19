using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteController : MonoBehaviour
{
    // wheels badman käyttää stringejä 
    private Dictionary<string, Sprite> _textures;

    void Start()
    {
        // lataa kaikki johonkin vaikka dicciiin aluks
        // nopeampiakin ratkaisuja olisi
        _textures = new Dictionary<string, Sprite>(16);
        Sprite[] sprites = Resources.LoadAll<Sprite>("Tiles/");
        foreach (Sprite sprite in sprites)
        {
            _textures[sprite.name] = sprite;
            print(sprite.name);
        }
    }

    // nurkat rajatapauksia / chunkkien välistä keskustelua 
    // spawnatessa tasoita aivan kaikki 
    // liikkuessa vain 
    // ne tietyt saumat ?



    public void InitChunkSprites(Chunk chunk)
    {
        // chunk.TileGameObjects

        int width = Chunk.CHUNK_SIZE;
        int height = Chunk.CHUNK_SIZE;

        // käy jokainen läpi ja valitse spritelle nimi!

        // looppa rajat erikseen ??
        // ei out of bounds checkkiä silloin



        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {

                TileType type = chunk.Tiles[y, x];
                //if (type != TileType.GrassLand)
                //{
                //    return; // vain GrassLand on implementattu atm!!!
                //}

#if false
                string assetName = type.ToString() + "_";
#else
                string assetName = "GrassLand_";
#endif

                // bit mask // boolean Mask XD?
                if (chunk.Tiles[y + 1, x] == type)
                {
                    assetName += "N";
                }
                if (chunk.Tiles[y, x + 1] == type)
                {
                    assetName += "E";
                }
                if (chunk.Tiles[y - 1, x] == type)
                {
                    assetName += "S";
                }
                if (chunk.Tiles[y, x - 1] == type)
                {
                    assetName += "W";
                }

                Sprite sprite;
                if (_textures.TryGetValue(assetName, out sprite))
                {
                    chunk.TileGameObjects[y, x].GetComponent<SpriteRenderer>().sprite = sprite;
                }
                else
                {
                    Debug.LogWarning("Texture: " + assetName + " Missing!");
                    // TODO: default / debug texture / pink texture
                }
            }
        }
    }

    public void SmoothCorners(Chunk[,] chunks, bool left) // EIT OITNM IOTJ EJ AJSDKL AJSD LKAJSD lk
    {
        // O | | X
        // O | | X
        // O | | X
        int chunkMaxIndex = Chunk.CHUNK_SIZE - 1;
        for (int chunkY = 0; chunkY < 3; chunkY++)
        {
            Chunk currentChunk = chunks[chunkY, 0];
            Chunk nextChunk = chunks[chunkY, 1];

            int i = 0;
            int maxIndex = chunkMaxIndex;
            if (chunkY == 0)
            {
                i = 1;
            }
            else if (chunkY == 2)
            {
                maxIndex--;
            }

            for (int y = i; y < maxIndex; y++) // ylintä ja alinta vaakasuoraa ei smooth ? 
            {
                TileType type = currentChunk.Tiles[y, chunkMaxIndex];
                TileType neighboutrChunkTileType = currentChunk.Tiles[y, 0];

                // kasaa molemmille oikeat textuurit

                string assetName = "GrassLand_";
                if (currentChunk.Tiles[y + 1, chunkMaxIndex] == type)
                {
                    assetName += "N";
                }
                if (type == neighboutrChunkTileType)
                {
                    assetName += "E";
                }
                if (currentChunk.Tiles[y - 1, chunkMaxIndex] == type)
                {
                    assetName += "S";
                }
                if (currentChunk.Tiles[y, chunkMaxIndex - 1] == type)
                {
                    assetName += "W";
                }

                Sprite sprite;
                if (_textures.TryGetValue(assetName, out sprite))
                {
                    currentChunk.TileGameObjects[y, chunkMaxIndex].GetComponent<SpriteRenderer>().sprite = sprite;
                }
                else
                {
                    Debug.LogWarning("Texture: " + assetName + " Missing!");
                    // TODO: default / debug texture / pink texture
                }

                assetName = "GrassLand_";
                if (currentChunk.Tiles[y + 1, 0] == neighboutrChunkTileType)
                {
                    assetName += "N";
                }
                if (neighboutrChunkTileType == currentChunk.Tiles[y, 0])
                {
                    assetName += "E";
                }
                if (currentChunk.Tiles[y - 1, 0] == neighboutrChunkTileType)
                {
                    assetName += "S";
                }
                if (neighboutrChunkTileType == type)
                {
                    assetName += "W";
                }

                if (_textures.TryGetValue(assetName, out sprite))
                {
                    nextChunk.TileGameObjects[y, 0].GetComponent<SpriteRenderer>().sprite = sprite;
                }
                else
                {
                    Debug.LogWarning("Texture: " + assetName + " Missing!");
                    // TODO: default / debug texture / pink texture
                }
            }
        }
        // O O O O
        // - - - -
        // X X X X

        // sivuttain kaikki sivuittain olevat ?

        // ylös alas kaikka vaakasuorassa olevat

    }

    public void OnSpawn(Chunk[,] chunk) // alll
    {

    }


}
