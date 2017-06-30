using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawnattavia resuja
//  eli eri kivi lajikkeita ja eri puu lajikkeita?


[CreateAssetMenu(menuName = "Tiles/ObjectSpawnSettings")]
public class TilemapObjectSpawnSettings : ScriptableObject
{
    public TileType Type;
    public SpawnSettingsData[] SpawnableObjects;
}













[System.Serializable]
public class SpawnSettingsData  
{
    public GameObject ObjectPrefab;
    public float SpawnRate;
}
