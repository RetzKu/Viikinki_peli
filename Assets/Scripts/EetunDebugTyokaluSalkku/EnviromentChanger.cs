using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentChanger : MonoBehaviour {

	// Use this for initialization
    private TileMap tileMap;
	void Start ()
	{
	    tileMap = FindObjectOfType<TileMap>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetMouseButtonDown(0))
	    {
	        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tileMap.SetTile(pos.x, pos.y, TileType.Forest);
	    }
	}
}
