using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Perlin))]
public class PerlinNoiseEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Perlin  myPerlin  = (Perlin) target;
        TileMap myTileMap = (TileMap) myPerlin.GetComponent<TileMap>();

        if (GUILayout.Button("Generate"))
        {
            // myPerlin.Init2();
            myPerlin.InitalizeRenderTarget();
        }
        else if (GUILayout.Button("Generate TileMap") && myTileMap != null)
        {
            myPerlin.GenerateTileMap(myTileMap);
        }
    }
}
