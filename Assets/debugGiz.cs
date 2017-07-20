using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugGiz : MonoBehaviour {
    BreadthFirstSearch copy;
    bool inited = false;
    float plus = 0.3f;
    public void init(BreadthFirstSearch k)
    {
        copy = k;
        inited = true;
    }

    public void OnDrawGizmosPate()
    {
        
        Vector2 mapstart = copy.map.GetGameObjectFast(0, 0).transform.position;
        if (inited)
        {
            Gizmos.color = Color.blue;
            for(int y = 0;y < copy.moveTiles.Count; y++)
            {
                for (int x = 0; x < copy.moveTiles.Count; x++)
                {
                    float tx = x + mapstart.x;
                    float ty = y + mapstart.y;
                    switch (copy.panther.dirs[y,x])
                    {
                        case PathFinder.Dir.Down:
                            Gizmos.DrawLine(new Vector2(tx, ty + plus), new Vector2(tx, ty - plus));
                            Gizmos.DrawLine(new Vector2(tx - (plus/2), ty - plus), new Vector2(tx + (plus / 2), ty - plus));
                            break;
                        case PathFinder.Dir.Up:
                            Gizmos.DrawLine(new Vector2(tx, ty + plus), new Vector2(tx, ty - plus));
                            Gizmos.DrawLine(new Vector2(tx - (plus / 2), ty + plus), new Vector2(tx + (plus / 2), ty + plus));
                            break;
                        case PathFinder.Dir.Left:
                            Gizmos.DrawLine(new Vector2(tx + plus, ty ), new Vector2(tx - plus,ty ));
                            Gizmos.DrawLine(new Vector2(tx - plus, ty - (plus / 2)), new Vector2(tx - plus,ty +(plus / 2)));

                            break;
                        case PathFinder.Dir.Right:
                            Gizmos.DrawLine(new Vector2(tx+ plus, ty ), new Vector2(tx - plus, ty));
                            Gizmos.DrawLine(new Vector2(tx + plus, ty - (plus / 2)), new Vector2(tx + plus, ty + (plus / 2)));
                            break;
                        case PathFinder.Dir.NoDir:
                            //Gizmos.DrawLine(new Vector2(x + plus, y), new Vector2(x - plus, y));
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
