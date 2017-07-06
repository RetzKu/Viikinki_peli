using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobsControl : MonoBehaviour
{

    private class spawn
    {
        public float x, y, rad;
        public int amount;

    }

    public int Mob_Amount;
    public GameObject Wolf;
    public GameObject Archer;
    List<GameObject> Boids;

    public GameObject player;

    List<spawn> spawner = new List<spawn>();

    void Start()
    {

        Boids = new List<GameObject>(Mob_Amount); // mah fix

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("y"))
        {
            var pl = player.GetComponent<Rigidbody2D>().position;
            SpawnBoids(pl.x, pl.y, 15f, Mob_Amount);          
        }
        if (Input.GetKeyDown("m"))
        {
            DeleteAllCurrentMobs();
        }



        if (spawner.Count > 0)// spawn every uptade if it is requested
        {
            if (spawner[0].amount > 0)
            {
                float x;
                float y;
                PathFinder.Dir k;
                do
                {
                    x = Random.Range(spawner[0].x - spawner[0].rad, spawner[0].x + spawner[0].rad);
                    y = Random.Range(spawner[0].y - spawner[0].rad, spawner[0].y + spawner[0].rad);
                    k = player.GetComponent<UpdatePathFind>().path.getTileDir(new Vector2(x, y));
                }
                while (k == PathFinder.Dir.NoDir);


                GameObject go;

                if(Boids.Count % 2 == 0)
                {
                    go = Instantiate(Wolf, new Vector2(x, y), Quaternion.identity);
                    go.GetComponent<generalAi>().InitStart(x, y,EnemyType.Wolf);
                }
                else
                {
                    go = Instantiate(Archer, new Vector2(x, y), Quaternion.identity);
                    go.GetComponent<generalAi>().InitStart(x, y, EnemyType.Archer);
                }
                //wolfBoids.Add(go);
                Boids.Add(go);

                spawner[0].amount--;
            }
            else
            {
                spawner.Remove(spawner[0]);
            }
        }

        int ind = 0;

        while (ind < Boids.Count)
        {
            if (Boids[ind].GetComponent<generalAi>().killMyself())
            {
                Destroy(Boids[ind]);
                Boids.Remove(Boids[ind]);
                
               
            }
            else
            {
                Boids[ind].GetComponent<generalAi>().UpdatePosition(Boids);
                ind++;
            }
        }

    }
    public void SpawnBoids(float x, float y, float radius, int amount)
    {
        spawn k = new spawn();
        k.x = x;
        k.y = y;
        k.rad = radius;
        k.amount = amount;
        spawner.Add(k);
    }

    public void DeleteAllCurrentMobs() // korjaa
    {
        foreach (GameObject kakka in Boids)
        {
            Destroy(kakka);
        }
        Boids.Clear(); // EETU TRIGGER might
        spawner.Clear(); // EETU TRIGGER might
    }
}
