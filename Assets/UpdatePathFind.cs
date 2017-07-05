﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePathFind : MonoBehaviour {

    private Rigidbody2D body;


    public BreadthFirstSearch path;
    //debugGiz Giz;

    bool uptade = true;

    int[] LasPos = new int[2];

    bool pressed = false;
    Vector2 midPoint = new Vector2(10f, 10f);
    public TileMap terveisin;

    // Use this for initialization
    void Start ()
    {
        path = new BreadthFirstSearch();
        path.map = terveisin;
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (uptade)
        {
            path.uptadeTiles(midPoint, terveisin); //get tile map
            uptade = false;
            //Giz.init(path);
        }

        int[] m = path.calculateIndex(body.position);
        

        if(LasPos[0] != m[0] || LasPos[1] != m[1])
        {
            path.uptadeTiles(m[0],m[1], terveisin);
            LasPos = m;
        }


    }

    //void OnDrawGizmos()
    //{
    //    Giz.OnDrawGizmosPate();


    //}
}
