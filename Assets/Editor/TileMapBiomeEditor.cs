using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BiomeSettings))]
public class TileMapBiomeEditor : Editor
{
    BiomeSettings Settings;
    private float enumWidht = 100f; 

    public void OnEnable()
    {
        Settings = (BiomeSettings)target;

        foreach (var elevationData in Settings.Elevations)
        {
            if (elevationData.EditorName == "")
            {
                elevationData.EditorName = "Hello World!";
            }
        }

    }

    public override void OnInspectorGUI()
    {
        //  EditorGUILayout.LabelField("Elevation: (min: " + Settings.ElevationMin.ToString() + ") Moisture: max: " + Settings.ElevationMax.ToString() + ")");
        //  EditorGUILayout.MinMaxSlider(ref Settings.ElevationMin, ref Settings.ElevationMax, 0f, 100f);
        //  EditorGUILayout.LabelField("Max Val:");
        //  EditorGUILayout.MinMaxSlider(ref Settings.MoistureMin, ref Settings.MoistureMax, 0f, 100f);


        for (int j = 0; j < Settings.Elevations.Length; j++)
        {
            var elevations = Settings.Elevations[j];
            elevations.EditorName = EditorGUILayout.TextArea(Settings.Elevations[j].EditorName);
            EditorGUI.indentLevel += 2;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("End Elevation: ");
            elevations.StartElevation = EditorGUILayout.Slider(elevations.StartElevation, 0f, 1f);
            EditorGUILayout.EndHorizontal();


            for (int i = 0; i < elevations.Tiles.Length; i++)
            {
                var tileData = elevations.Tiles[i];

                GUILayout.Label("Tile type: ");
                tileData.Type = (TileType)EditorGUILayout.EnumPopup(tileData.Type, GUILayout.Width(enumWidht));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Moisture End: ");
                tileData.StartMoisture = EditorGUILayout.Slider(tileData.StartMoisture, 0f, 1f);
                EditorGUILayout.EndHorizontal();
            }

            var buttonWidth = GUILayout.Width(100);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add new tile", buttonWidth))
            {
                int l = elevations.Tiles.Length;
                BiomeSettings.TileData[] copy = new BiomeSettings.TileData[l + 1];
                System.Array.Copy(elevations.Tiles, copy, l);
                copy[l] = new BiomeSettings.TileData();
                elevations.Tiles = copy;
            }

            if (GUILayout.Button("Delete last", buttonWidth))
            {
                int l = elevations.Tiles.Length;
                BiomeSettings.TileData[] copy = new BiomeSettings.TileData[l - 1];
                System.Array.Copy(elevations.Tiles, copy, l - 1);
                elevations.Tiles = copy;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(40);
            // EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUI.indentLevel -= 2;
        }

        EditorGUILayout.LabelField("heello");
        if (DrawDefaultInspector())
        {

        }

        Validate();

        EditorUtility.SetDirty(Settings);
    }

    public void Validate()
    {
        var elevations = Settings.Elevations;

        if (elevations.Length == 0)
            return;

        if (elevations[0].StartElevation > 100f)
            elevations[0].StartElevation = 100f;

        for (int i = 0; i < elevations.Length - 1; i++)
        {
            if (elevations[i].StartElevation > elevations[i + 1].StartElevation)
            {
                elevations[i + 1].StartElevation = elevations[i].StartElevation;
            }

            if (elevations[i + 1].StartElevation > 100f)
            {
                elevations[i + 1].StartElevation = 100f;
            }
        }
    }
}
