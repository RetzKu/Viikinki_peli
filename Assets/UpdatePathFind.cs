using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePathFind : MonoBehaviour {

    private Rigidbody2D body;


    public BreadthFirstSearch path;
    debugGiz Giz = new debugGiz();

    bool uptade = true;

    int[] LasPos = new int[2];

    bool pressed = false;
    Vector2 midPoint = new Vector2(10f, 10f);
    public TileMap terveisin;


    // joku lastile

    // Use this for initialization
    void Start ()
    {
        path = new BreadthFirstSearch();
        path.map = terveisin;
        body = GetComponent<Rigidbody2D>();
        //path.uptadeTiles(midPoint, terveisin);
        //bool colid = TileMap.Collides(terveisin.GetTileFast(y, j));
    }

    // Update is called once per frame
    void Update() {
        // if in new tile -> uptade

        if (uptade)
        {
            path.uptadeTiles(midPoint, terveisin); //get tile map
            uptade = false;
            Giz.init(path);
        }

        int[] m = path.calculateIndex(body.position);
        

        if(LasPos[0] != m[0] || LasPos[1] != m[1])
        {
            //Vector3 j = path.map.GetGameObjectFast(0, 0).transform.position;


            path.uptadeTiles(m[0],m[1], terveisin);
            print("path updated");
            LasPos = m;
        }
        //if (Input.GetKeyDown("y"))          
        //    {
        //        midPoint.x++;

        //        path.uptadeTiles(midPoint, terveisin); //get tile map
        //        pressed = true;
        //    }
        //if (Input.GetKeyDown("u"))
        //{
        //    midPoint.y++;

        //    path.uptadeTiles(midPoint, terveisin); //get tile map
        //    pressed = true;
        //}

    }

    void OnDrawGizmos()
    {
        Giz.OnDrawGizmosPate();


    }
}
