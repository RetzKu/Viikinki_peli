using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Perlin))]
public class PerlinNoiseEditor : Editor
{
    private bool _update = true;

    public override void OnInspectorGUI()
    {
        Perlin  myPerlin  = (Perlin) target;
        TileMap myTileMap = (TileMap) myPerlin.GetComponent<TileMap>();

        if (DrawDefaultInspector())
        {
            // arvoja muutettu
            myPerlin.InitalizeRenderTarget();
            if (_update)
            {
                myPerlin.GenerateTileMap(myTileMap);
            }
        }

        if (GUILayout.Button("Generate"))
        {
            myPerlin.InitalizeRenderTarget();
        }
        else if (GUILayout.Button("Generate ITileMap") && myTileMap != null)
        {
            myPerlin.GenerateTileMap(myTileMap);
        }
        else if (GUILayout.Button("toggle update"))
        {
            _update = !_update;
        }
        else if (GUILayout.Button("Generate Big Map!"))
        {
            //myPerlin.GenerateWorldTextureMap(myPerlin.BigMapWidth, myPerlin.BigMapWidth, 0f, -2f);
        }
    }
}
