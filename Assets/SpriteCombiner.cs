using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SpriteCombiner : MonoBehaviour
{
    public Sprite[] TileSprites;
    public string CombineDir = "Tiles/Rock";

    void Start()
    {
        TileSprites = Resources.LoadAll<Sprite>(CombineDir);
        MakeCorners();
    }

    void MakeCorners()
    {
        Sprite center = TileSprites[0];
        Sprite corner = TileSprites[9];

        SaveToFile("Kuvia/uusiKuva.png", Combine(center, corner, 0, 0));

        Sprite up = TileSprites[10];

        SaveToFile("Kuvia/aki.png", Combine(center, corner, 0, 40));
    }

    Texture2D Combine(Sprite first, Sprite second, int startX, int startY)
    {
        int w = (int)first.textureRect.width;
        int h = (int)first.textureRect.height;
        Texture2D value = new Texture2D(w, h, TextureFormat.ARGB32, false);


        Rect centerTextureRect = first.textureRect;
        Color[] pixels = first.texture.GetPixels((int)centerTextureRect.x, (int)centerTextureRect.y, (int)centerTextureRect.width, (int)centerTextureRect.height);

        Rect secondRect = second.textureRect;
        Color[] placeOnTopPixels = second.texture.GetPixels((int)secondRect.x, (int)secondRect.y, (int)secondRect.width, (int)secondRect.height);

        // Texture2D value = new Texture2D(w, h);
        w = (int)secondRect.width;
        h = (int)secondRect.height;
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                pixels[startX + i * startY + w + j] = placeOnTopPixels[i * w + j];
                // value.SetPixel(j, i, pixels[i * w + j]); // alimmainen
            }
        }

        value.SetPixels(pixels);
        value.Apply();
        return value;
    }

    void SaveToFile(string filePath, Texture2D texture)
    {
        byte[] png = texture.EncodeToPNG();
        ByteArrayToFile(filePath, png);
    }

    public static void ByteArrayToFile(string fileName, byte[] byteArray)
    {
        using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        {
            fs.Write(byteArray, 0, byteArray.Length);
        }
    }
}
