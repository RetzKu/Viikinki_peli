using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

public class TestWriter : MonoBehaviour
{
    public static string GetChunkPath(string chunkName)
    {
        return Application.persistentDataPath + "/" + chunkName + ".sav";
    }

    public static bool ChunkExists(string name)
    {
        return File.Exists(GetChunkPath(name));
    }

    public static void Save(TileType[,] chunk, Dictionary<Vec2, GameObject> tileObjects, string name)
    {
        string path = GetChunkPath(name);
#if false

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, chunk);

        // bf.Serialize(file, go);

        file.Close();
#else
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            byte[] byteArray = ToBytes(chunk);
            int size = byteArray.Length;
            writer.Write(size);
            writer.Write(byteArray);

            writer.Write(tileObjects.Count);

            // int c = 0;
            foreach (var keyvaluePair in tileObjects)
            {
                if (!keyvaluePair.Value.activeSelf) // HOTFIX XD
                {
                    writer.Write(-1);
                    writer.Write(-1);
                    writer.Write((int)ResourceType.Max);
                }
                else
                {
                    writer.Write(keyvaluePair.Key.X);
                    writer.Write(keyvaluePair.Key.Y);
                    writer.Write((int)keyvaluePair.Value.GetComponent<Resource>().type);
                }
                // c++;
                // Debug.Log((int)keyvaluePair.Value.GetComponent<Resource>().type);
            }
            // Debug.Log("save " + name + " " + c);
        }
#endif

        // Debug.Log("Saved chunk: " + chunk);
        // Debug.Log(Application.persistentDataPath);
    }


    public static TileType[,] Load(string chunkName, out Dictionary<Vec2, ResourceType> types)
    {
        if (File.Exists(Application.persistentDataPath + "/" + chunkName + ".sav"))
        {
#if false
            FileStream file = File.Open(Application.persistentDataPath + "/" + chunkName + ".sav", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + chunkName + ".sav", FileMode.Open);
            TileType[,] loaded = (TileType[,])bf.Deserialize(file);

            // PrintTile(loaded);

            file.Close();
            // Debug.Log("Loaded Chunk: " + chunkName);
            return loaded;
#else
            // using (FileStream fs = File.OpenRead(file))
            int c = 0;
            FileStream file = File.Open(Application.persistentDataPath + "/" + chunkName + ".sav", FileMode.Open);
            using (BinaryReader reader = new BinaryReader(file))
            {
                int size = reader.ReadInt32();
                byte[] buffer = reader.ReadBytes(size);
                TileType[,] tiles = FromBytes(buffer);


                int count = reader.ReadInt32();
                types = new Dictionary<Vec2, ResourceType>(count);

                for (int i = 0; i < count; i++)
                {
                        
                    Vec2 pos = new Vec2(reader.ReadInt32(), reader.ReadInt32());
                    ResourceType type = (ResourceType)reader.ReadInt32();

                    if (type == ResourceType.Max) // HOTFIXS
                    {
                        continue;
                    }
                    types.Add(pos, type);

                    // writer.Write(-1);
                    // writer.Write(-1);
                    // writer.Write((int)ResourceType.Max);
                    c++;
                }
                // Debug.Log("load " + chunkName + " " + c);
                return tiles;
            }
#endif
        }
        else
        {
            Debug.LogError("Couldn't load Chunk: " + chunkName);
            types = null;
            return new TileType[17, 17];
        }
    }

    static byte[] ToBytes(object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);
        byte[] bytes = ms.ToArray();
        return bytes;
    }

    static TileType[,] FromBytes(byte[] buffer)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        TileType[,] mat = (TileType[,])bf.Deserialize(ms);
        return mat;
    }

public static void PrintTile(TileType[,] map)
    {
        string data = null;
        foreach (var row in map)
        {
            data += row.ToString();
        }
        Debug.Log(data);
    }
}
