using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePathFind : MonoBehaviour {
    public BreadthFirstSearch path;

    bool uptade = true;



    bool pressed = false;
    Vector2 midPoint = new Vector2(30f, 30f);
    public TileMap terveisin;
    // joku lastile

    // Use this for initialization
    void Start () {
        path = new BreadthFirstSearch();
        path.map = terveisin;
        path.uptadeTiles(midPoint, terveisin);
        //bool colid = TileMap.Collides(terveisin.GetTileFast(y, j));

    }

    // Update is called once per frame
    void Update() {
        // if in new tile -> uptade
        if (Input.GetKeyDown("y"))
            if (!pressed)
            {
                midPoint.x++;

                path.uptadeTiles(midPoint, terveisin); //get tile map
                pressed = true;
            }
        else if (Input.GetKeyUp("y"))
            pressed = false;  
    }

   
}
