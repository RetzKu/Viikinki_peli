using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCave : MonoBehaviour {

    List<Cave> caves = new List<Cave>();
    MapGenerator generator = new MapGenerator();

    class Cave
    {
        public string seed;
        public int fillPercent;

        int widht;
        int height;
    }

    public void GenerateCave()
    {

    }



    //List<MapGenerator> caves;


	// Use this for initialization
	//void Start () {
 //       //caves = new List<MapGenerator>();
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}
}
