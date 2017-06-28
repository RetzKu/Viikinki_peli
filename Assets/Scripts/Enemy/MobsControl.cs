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

    //List<GameObject> Mobs;
    public int Mob_Amount;
    public GameObject EnemyPrefab;
    public GameObject enemyChild;
    List<GameObject> Boids;
    //List<GameObject> wolfBoids;
    //List<GameObject> archerBoids;

    List<spawn> spawner = new List<spawn>();
    //[HideInInspector]


    // Use this for initialization
    void Start()
    {
        print("Starting spawning");
        //Mobs = new List<GameObject>(Mob_Amount);
        //spawner = new List<Vector4>(1);
        Boids = new List<GameObject>(Mob_Amount); // mah fix
        //wolfBoids = new List< GameObject > (Mob_Amount);
        //archerBoids = new List< GameObject > (Mob_Amount);
        for (int i = 0; i < Mob_Amount; i++)
        {
            float x = 0;
            float y = 0;
            do
            {
                x = Random.Range(0f, 50f);
                y = Random.Range(0f, 50f);
            }
            while (Physics2D.OverlapCircleAll(new Vector2(x, y), 0.5f).Length != 0);  // EETU TRIGGER
            var go = Instantiate(EnemyPrefab, new Vector2(x, y), Quaternion.identity);
            //var ga = Instantiate(enemyChild, new Vector2(x, y), Quaternion.identity);

            //ga.transform.parent = go.transform;

            if (i <= Mob_Amount/2)
            {
                go.GetComponent<EnemyAI>().InitStart(x, y,EnemyType.Archer);
                go.layer = 8; // enemy
                //archerBoids.Add(go);
            }
            else
            {
                go.GetComponent<EnemyAI>().InitStart(x, y, EnemyType.Wolf);
                go.layer = 8; // enemy
                //wolfBoids.Add(go);
            }
            Boids.Add(go);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (spawner.Count > 0)// spawn every uptade if it is requested
        {
            if (spawner[0].amount > 0)
            {
                float x = Random.Range(spawner[0].x - spawner[0].rad, spawner[0].x + spawner[0].rad);
                float y = Random.Range(spawner[0].x - spawner[0].rad, spawner[0].x + spawner[0].rad);
                var go = Instantiate(EnemyPrefab, new Vector2(x, y), Quaternion.identity);
                go.GetComponent<EnemyAI>().InitStart(x, y,EnemyType.Wolf);
                //wolfBoids.Add(go);
                Boids.Add(go);

                spawner[0].amount--;
            }
            else
            {
                spawner.Remove(spawner[0]);
            }
        }
        for (int i = 0; i < Boids.Count; i++)
        {
            //if (Boids[i].GetComponent<EnemyAI>().myType == EnemyType.Wolf)
            //{
            Boids[i].GetComponent<EnemyAI>().UpdatePosition(Boids);
            //}
            //else if(Boids[i].GetComponent<EnemyAI>().myType == EnemyType.Archer)
            //{
            //    Boids[i].GetComponent<EnemyAI>().UpdatePosition(archerBoids);
            //}
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
