using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

public class TestWriter : MonoBehaviour
{

    private const int FileSize = 10 * 1024 * 1024;
    void Start()
    {
        var path = Path.Combine(Application.persistentDataPath, "bigfile.dat");
        try
        {
            File.WriteAllBytes(path, new byte[FileSize]);
            TestFileStream(path);
            TestBinaryReaders(path);
        }
        finally
        {
            File.Delete(path);
        }

        Chunk t = new Chunk();
    }

    void Update()
    {
        //TileType[,] a = new TileType[Chunk.CHUNK_SIZE, Chunk.CHUNK_SIZE];
        //a[1,1] = TileType.Beach;

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    Save(a, "terve");
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    Load("terve");
        //}
    }

    public static void Save(TileType[,] chunk, string name)
    {

        string path = Application.persistentDataPath + "/" + name + ".sav";

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, chunk);
        file.Close();

        // Debug.Log("Saved chunk: " + chunk);
        // Debug.Log(Application.persistentDataPath);
    }

    public static TileType[,] Load(string chunkName)
    {
        if (File.Exists(Application.persistentDataPath + "/" + chunkName + ".sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + chunkName + ".sav", FileMode.Open);
            TileType[,] loaded = (TileType[,])bf.Deserialize(file);

            PrintTile(loaded);

            file.Close();
            // Debug.Log("Loaded Chunk: " + chunkName);

            return loaded;
        }
        else
        {
            Debug.LogError("Couldn't load Chunk: " + chunkName);
            return new TileType[17, 17];
        }
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



    private void TestFileStream(string path)
    {
        using (var stream = new FileStream(path, FileMode.Open))
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var readBytes = new byte[FileSize];
            var log = "Read Size,Time\n";
            foreach (var readSize in new[] { 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096 })
            {
                stream.Position = 0;
                stopwatch.Reset();
                stopwatch.Start();
                var offset = 0;
                do
                {
                    offset += stream.Read(readBytes, offset, Math.Min(readSize, FileSize - offset));
                }
                while (offset < FileSize);
                var time = stopwatch.ElapsedMilliseconds;
                log += readSize + "," + time + "\n";
            }
            Debug.Log(log);
        }
    }

    private void TestBinaryReaders(string path)
    {
        using (var stream = new FileStream(path, FileMode.Open))
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var log = "Reader,Time\n";
            var numValues = FileSize / sizeof(ushort);
            var readValues = new ushort[numValues];
            var reader = new BinaryReader(stream);
            stopwatch.Reset();
            stopwatch.Start();
            for (var i = 0; i < numValues; ++i)
            {
                readValues[i] = reader.ReadUInt16();
            }
            var time = stopwatch.ElapsedMilliseconds;
            log += "BinaryReader," + time + "\n";

            stream.Position = 0;
            var bufferedReader = new BufferedBinaryReader(stream, 4096);
            stopwatch.Reset();
            stopwatch.Start();
            while (bufferedReader.FillBuffer())
            {
                var readValsIndex = 0;
                for (
                    var numReads = bufferedReader.NumBytesAvailable / sizeof(ushort);
                    numReads > 0;
                    --numReads
                )
                {
                    readValues[readValsIndex++] = bufferedReader.ReadUInt16();
                }
            }
            time = stopwatch.ElapsedMilliseconds;
            log += "BufferedBinaryReader," + time + "\n";
            Debug.Log(log);
        }
    }
}
