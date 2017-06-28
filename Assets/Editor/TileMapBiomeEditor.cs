using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BiomeSettings))]
public class TileMapBiomeEditor : Editor
{
    BiomeSettings Settings;

    public void OnEnable()
    {
        Settings = (BiomeSettings)target;
    }

    public override void OnInspectorGUI()
    {


        //  EditorGUILayout.LabelField("Elevation: (min: " + Settings.ElevationMin.ToString() + ") Moisture: max: " + Settings.ElevationMax.ToString() + ")");
        //  EditorGUILayout.MinMaxSlider(ref Settings.ElevationMin, ref Settings.ElevationMax, 0f, 100f);
        //  EditorGUILayout.LabelField("Max Val:");
        //  EditorGUILayout.MinMaxSlider(ref Settings.MoistureMin, ref Settings.MoistureMax, 0f, 100f);


        EditorGUILayout.LabelField("heello");
        if (DrawDefaultInspector())
        {

        }

        Validate();

        EditorUtility.SetDirty(Settings);
    }

    public void Validate()
    {
        var elevations = Settings.elevations;

        if (elevations[0].startElevation > 100f)
            elevations[0].startElevation = 100f;

        for (int i = 0; i < elevations.Length; i++)
        {
            if (elevations[i].startElevation > elevations[i + 1].startElevation)
            {
                elevations[i + 1].startElevation = elevations[i].startElevation;
            }

            if (elevations[i + 1].startElevation > 100f)
            {
                elevations[i + 1].startElevation = 100f;
            }
        }
    }
}
