using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tiles/SpriteControllerSettings")]
public class SpriteControllerSettings : ScriptableObject
{
    [System.Serializable]
    public class SpriteControllerSettingsData
    {
        public TileType Type;
        public Sprite[] Sprites;
    }

    public SpriteControllerSettingsData[] SpriteData;
}

