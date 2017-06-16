using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobsControl : MonoBehaviour {

    

    // Use this for initialization
    //List<GameObject> Mobs;
    public int Mob_Amount;
    public GameObject EnemyPrefab;
    List<GameObject> Boids;



    void Start () {
        print("Starting spawning");
        //Mobs = new List<GameObject>(Mob_Amount);
        Boids = new List<GameObject>(Mob_Amount); // mah fix
        for (int i = 0; i < Mob_Amount; i++)
        {
         
            int x = Random.Range(5,25);
            int y = Random.Range(5,25);
            var go = Instantiate(EnemyPrefab, new Vector2(x,y), Quaternion.identity);
            go.GetComponent<EnemyMovement>().InitStart(x, y);
            Boids.Add(go);
        }


    }
	
	// Update is called once per frame
	void Update()
    {
        //print(Boids.Count);
        for(int i = 0; i < Boids.Count; i++)
        {
            //print(i);
            //Mobs[i].GetComponent<EnemyMovement>().separate(Mobs);
            //Boids[i].GetComponent<EnemyMovement>().flock(Boids);
           Boids[i].GetComponent<EnemyMovement>().applyBehavior(Boids);
           Boids[i].GetComponent<EnemyMovement>().MovementUpdate();

        }
    }

}
