using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugGiz : MonoBehaviour
{
    BreadthFirstSearch copy;
    bool inited = false;
    float plus = 0.3f;
    public bool DrawTileDirs = true;

    public static Dictionary<Vec2, Vec2> PathfinderCameFroms = new Dictionary<Vec2, Vec2>(289);

    public UpdatePathFind Updater;

    private void DrawCameFromGizmos()
    {
        foreach (var keyValuePair in PathfinderCameFroms)
        {
            Vec2 v = keyValuePair.Value;
            // PathFinder.GetDir()
            // keyValuePair.Value  
        }

    }


    private void DrawDirections()
    {
        if (DrawTileDirs)
        {

            PathFinder.Dir[,] dirs = copy.panther.dirs;
            Vector2 mapstart = copy.map.GetTileGameObject(0, 0).transform.position;

            for (int i = 1; i < 50; i++)
            {
                for (int y = 1; y < 50; y++)
                {
                    // print(i + " " + y);
                    //Vector3 arrow = PathFinder.GetDir(Updater.path.getTileDir(new int[] { y, i }));
                    // PathFinder.GetDir(dirs[i, y]); 
                   // Gizmos.DrawLine(new Vector3(mapstart.x + y, mapstart.y + i, 4f), new Vector3(mapstart.x + y, mapstart.y + i, 4f) + arrow);
                }
            }
        }
    }






    public void init(BreadthFirstSearch k)
    {
        copy = k;
        inited = true;
    }

    public void OnDrawGizmosPate()
    {
        DrawDirections();
        return;
        Vector2 mapstart = copy.map.GetTileGameObject(0, 0).transform.position;
        if (inited)
        {
            Gizmos.color = Color.blue;
            for (int y = 0; y < copy.moveTiles.Count; y++)
            {
                for (int x = 0; x < copy.moveTiles.Count; x++)
                {
                    float tx = x + mapstart.x;
                    float ty = y + mapstart.y;
                    switch (copy.moveTiles[y][x].tileState)
                    {
                        //case PathFinder.Dir.Down:
                        //    Gizmos.DrawLine(new Vector2(tx, ty + plus), new Vector2(tx, ty - plus));
                        //    Gizmos.DrawLine(new Vector2(tx - (plus/2), ty - plus), new Vector2(tx + (plus / 2), ty - plus));
                        //    break;
                        //case PathFinder.Dir.Up:
                        //    Gizmos.DrawLine(new Vector2(tx, ty + plus), new Vector2(tx, ty - plus));
                        //    Gizmos.DrawLine(new Vector2(tx - (plus / 2), ty + plus), new Vector2(tx + (plus / 2), ty + plus));
                        //    break;
                        //case PathFinder.Dir.Left:
                        //    Gizmos.DrawLine(new Vector2(tx + plus, ty ), new Vector2(tx - plus,ty ));
                        //    Gizmos.DrawLine(new Vector2(tx - plus, ty - (plus / 2)), new Vector2(tx - plus,ty +(plus / 2)));

                        //    break;
                        //case PathFinder.Dir.Right:
                        //    Gizmos.DrawLine(new Vector2(tx+ plus, ty ), new Vector2(tx - plus, ty));
                        //    Gizmos.DrawLine(new Vector2(tx + plus, ty - (plus / 2)), new Vector2(tx + plus, ty + (plus / 2)));
                        //    break;
                        case BreadthFirstSearch.states.wall:
                            Gizmos.color = Color.blue;
                            //Gizmos.DrawLine(new Vector2(x + plus, y), new Vector2(x - plus, y));
                            Gizmos.DrawSphere(new Vector2(tx, ty), 0.5f);
                            break;
                        case BreadthFirstSearch.states.unVisited:
                            //Gizmos.DrawLine(new Vector2(x + plus, y), new Vector2(x - plus, y));
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawSphere(new Vector2(tx, ty), 0.5f);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
    // Use this for initialization
    //void Start () {

    //}

    // Update is called once per frame
    //void Update () {

    //}
}


// janin rekkari: vez-925, tölkki light batteryä, kolmen raidan kengät.
