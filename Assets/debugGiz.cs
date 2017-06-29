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
        if (inited)
        {
            Gizmos.color = Color.blue;
            for(int y = 0;y < copy.moveTiles.Count; y++)
            {
                for (int x = 0; x < copy.moveTiles.Count; x++)
                {
                    switch (copy.moveTiles[y][x].tileState)
                    {
                        case BreadthFirstSearch.states.down:
                            Gizmos.DrawLine(new Vector2(x, y + plus), new Vector2(x, y - plus));
                            Gizmos.DrawLine(new Vector2(x - (plus/2), y - plus), new Vector2(x + (plus / 2), y - plus));
                            break;
                        case BreadthFirstSearch.states.up:
                            Gizmos.DrawLine(new Vector2(x, y + plus), new Vector2(x, y - plus));
                            Gizmos.DrawLine(new Vector2(x - (plus / 2), y + plus), new Vector2(x + (plus / 2), y + plus));
                            break;
                        case BreadthFirstSearch.states.left:
                            Gizmos.DrawLine(new Vector2(x + plus, y ), new Vector2(x - plus, y ));
                            Gizmos.DrawLine(new Vector2(x - plus, y - (plus / 2)), new Vector2(x - plus,y +(plus / 2)));

                            break;
                        case BreadthFirstSearch.states.right:
                            Gizmos.DrawLine(new Vector2(x+ plus, y ), new Vector2(x - plus, y));
                            Gizmos.DrawLine(new Vector2(x + plus, y - (plus / 2)), new Vector2(x + plus, y + (plus / 2)));
                            break;
                        case BreadthFirstSearch.states.goal:
                            //Gizmos.DrawLine(new Vector2(x + plus, y), new Vector2(x - plus, y));
                            Gizmos.DrawSphere(new Vector2(x, y), 0.5f);
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
