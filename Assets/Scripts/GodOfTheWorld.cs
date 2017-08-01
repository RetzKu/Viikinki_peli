using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodOfTheWorld : MonoBehaviour {

    // Use this for initialization
    public GameObject tilemap;
    public GameObject player;
    int worldMobs = 0;
	//void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}
    void CreateEverything()
    {
        //MobsControl.instance.SpawnBoids(player.transform.position.x, player.transform.position.y, )
    }
    void DestroyWorld()
    {
        worldMobs = MobsControl.instance.DeleteAllCurrentMobs();



        // tilemap.tuhoa();
        // luola.ala();
        // olen.donezo();

    }
    void DestroyCave()
    {
        worldMobs = MobsControl.instance.DeleteAllCurrentMobs();


  

        // tilemap.tuhoa();
        // luola.ala();
        // olen.donezo();

    }
}
