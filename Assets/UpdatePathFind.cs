using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePathFind : MonoBehaviour
{

    private Rigidbody2D body;


    public BreadthFirstSearch path;
    //debugGiz Giz;

    bool uptade = true;

    int[] LasPos = new int[2];

    bool pressed = false;
    Vector2 midPoint = new Vector2(10f, 10f);
    public ITileMap tilemap;// vaihra


    // Use this for initialization
    void Start()
    {
        path = new BreadthFirstSearch();
        path.map = tilemap;
        body = GetComponent<Rigidbody2D>();
        //Giz = GetComponent<debugGiz>();
        //Giz.init(path);

        var go = GameObject.FindGameObjectWithTag("Tilemap");
        if (go)
        {
            tilemap = go.GetComponent<TileMap>();
        }
        else
        {
            Debug.LogError("Tilemap missing");
        }
    }

    void Update()
    {
        if (uptade)
        {
            if (tilemap.CanUpdatePathFind())
            {
                path.uptadeTiles(midPoint, tilemap); //get tile map // 
                uptade = false;
            }
            //Giz.init(path);
        }

        int[] m = path.calculateIndex(body.position);


        if (LasPos[0] != m[0] || LasPos[1] != m[1])
        {
            path.uptadeTiles(m[0], m[1], tilemap);
            LasPos = m;
        }


    }

    //void OnDrawGizmos() // käytä pathfind debuggaukseens
    //{
    //    Giz.OnDrawGizmosPate();
    //}
}
