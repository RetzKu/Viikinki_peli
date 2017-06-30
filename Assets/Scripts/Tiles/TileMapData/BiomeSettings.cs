using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tiles/BiomeSpawnRates")]
public class BiomeSettings : ScriptableObject
{
    [System.Serializable]
    public class TileData
    {
        [Range(0f, 1f)]
        public float StartMoisture;
        public TileType Type;
        public string AssetName;
        public TilemapObjectSpawnSettings SpawnSettings;
    }

    [System.Serializable]
    public class ElevationData
    {
        [Range(0f, 1f)]
        public float StartElevation;
        public TileData[] Tiles;
        public string EditorName;
    }
    public ElevationData[] Elevations;
}

//  0 -> ;;28 -> ;;60 -> ;;71
//  if ( < elevation
//  
// 
// 
 //       if (e < 0.16f) return TileType.DeepWater;         
 //       if (e < 0.22f) return TileType.Water;
 //       if (e < 0.26f) return TileType.Beach;
 //       if (e < 0.50f)
 //       {
 //           if (m< 0.20f) return TileType.Desert;
 //           if (m > 0.70f) return TileType.Jungle;
 //           return TileType.GrassLand;
 //       }
