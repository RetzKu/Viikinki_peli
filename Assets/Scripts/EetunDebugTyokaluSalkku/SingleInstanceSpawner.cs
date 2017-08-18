using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleInstanceSpawner : MonoBehaviour
{
    public List<SpawnInstance> Spawns = new List<SpawnInstance>(10);

    void Start()
    {
    }

    public void TryToSpawn(Chunk chunk, int x, int y)
    {
        // chunk.AddObject();
    }
}
