using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodOfTheWorld : MonoBehaviour {

    // Use this for initialization
    public GameObject tilemap;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void CreateEverything()
    {

    }
    void DestroyEverything()
    {
        MobsControl.instance.DeleteAllCurrentMobs();


        for(int y = 0; y < TileMap.TotalHeight; y++)
        {
            for (int x = 0; x < TileMap.TotalHeight; x++)
            {
                Destroy(tilemap.GetComponent<TileMap>().GetGameObjectFast(x,y));
            }
        }

        // tilemap.tuhoa();
        // luola.ala();
        // olen.donezo();

    }
}
