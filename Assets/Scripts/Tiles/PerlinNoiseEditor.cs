using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Perlin))]
public class PerlinNoiseEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Perlin myPerlin = (Perlin) target;
        if (GUILayout.Button("Generate!"))
        {
            myPerlin.Init2(256, 256);
        }
    }
}
