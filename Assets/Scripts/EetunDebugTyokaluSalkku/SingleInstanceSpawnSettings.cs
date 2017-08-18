using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tiles/SingleInstanceSpawnSettings")]
public class SingleInstanceSpawnSettings : ScriptableObject
{
    public List<SpawnInstance> Spawns;
    public int SpawnRate;
}
