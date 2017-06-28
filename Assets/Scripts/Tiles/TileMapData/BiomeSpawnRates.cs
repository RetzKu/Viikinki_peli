using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(menuName = "Tiles/BiomeSpawnRates")]
public class BiomeSettings : ScriptableObject
{
    [System.Serializable]
    public class TileData
    {
        public float startMoisture;
        public TileType type;
        public Sprite[] sprites;
        public string assetName;
    }
    [System.Serializable]
    public class ElevationData
    {
        public float startElevation;
        public TileData[] tiles;
    }

    public ElevationData[] elevations;
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