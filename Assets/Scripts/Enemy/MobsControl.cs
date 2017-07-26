using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobsControl : MonoBehaviour
{
    public static MobsControl instance;


    private class spawn
    {
        public float x, y, rad;
        public int amount;

    }

    // JOONAN BOOL MUUTTUJA, PATE NÄPIT IRTI, THX // fuq u mayn
    [Header("Enemies deal dmg")]
    public bool enemiesDealDamage = true;

    bool slow = false; // DEBUG
    public int Mob_Amount;
    public GameObject Wolf;
    public GameObject Archer;
    public GameObject MeleeDude;
    List<GameObject> Boids;

    public GameObject player;

    public bool spawnWolfs;
    public bool spawnMelee;
    public bool spawnArchers;
    public bool spawnBears;
    public bool naturalSpawn;
    List<spawn> spawner = new List<spawn>();
    void Start()
    {

        Boids = new List<GameObject>(Mob_Amount); // mah fix
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!naturalSpawn)
        {
            if (Input.GetKeyDown("y"))
            {
                var pl = player.GetComponent<Rigidbody2D>().position;
                SpawnBoids(pl.x, pl.y, 4f, Mob_Amount);          
            }
            if (Input.GetKeyDown("m"))
            {
                DeleteAllCurrentMobs();
            }
            if (Input.GetKeyDown("k"))
            {
                slow = true;
            }
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
                if (!naturalSpawn)
                {
                    if (spawnMelee)
                    {
                        GameObject m;
                        m = Instantiate(MeleeDude, new Vector2(x, y), Quaternion.identity);
                        m.GetComponent<generalAi>().InitStart(x, y,EnemyType.Archer, player);
                        Boids.Add(m);
                    }
                    if (spawnWolfs)
                    {
                        GameObject m;
                        m = Instantiate(Wolf, new Vector2(x, y), Quaternion.identity);
                        m.GetComponent<generalAi>().InitStart(x, y, EnemyType.Wolf, player);
                        Boids.Add(m);
                    }

                }
                else
                {
                    if (Boids.Count % 2 == 0)
                    {
                        GameObject m;
                        m = Instantiate(MeleeDude, new Vector2(x, y), Quaternion.identity);
                        m.GetComponent<generalAi>().InitStart(x, y, EnemyType.Archer, player);
                        Boids.Add(m);
                    }
                    else
                    {
                        GameObject m;
                        m = Instantiate(Wolf, new Vector2(x, y), Quaternion.identity);
                        m.GetComponent<generalAi>().InitStart(x, y, EnemyType.Wolf, player);
                        Boids.Add(m);
                    }

                }

                //wolfBoids.Add(go);

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
                if (slow)
                {
                    Boids[ind].GetComponent<generalAi>().SlowRune(5f,0.5f);
                    print("slow");
                }
                Boids[ind].GetComponent<generalAi>().UpdatePosition();
                ind++;
            }
        }
        if (slow)
        {
            slow = false;
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
        Boids.Clear();
        spawner.Clear(); 
    }

}
