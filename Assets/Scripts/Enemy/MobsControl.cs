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
    List<GameObject> Boids;
    List<spawn> spawner = new List<spawn>();
    //[HideInInspector]


    // Use this for initialization
    void Start()
    {
        print("Starting spawning");
        //Mobs = new List<GameObject>(Mob_Amount);
        //spawner = new List<Vector4>(1);
        Boids = new List<GameObject>(Mob_Amount); // mah fix
        for (int i = 0; i < Mob_Amount; i++)
        {

            float x = Random.Range(0f, 50f);
            float y = Random.Range(0f, 50f);
            var go = Instantiate(EnemyPrefab, new Vector2(x, y), Quaternion.identity);
            go.GetComponent<EnemyAI>().InitStart(x, y);
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
                go.GetComponent<EnemyAI>().InitStart(x, y);
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
            Boids[i].GetComponent<EnemyAI>().UpdatePosition(Boids);
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

    public void DeleteAllCurrentMobs()
    {
        foreach (GameObject kakka in Boids)
        {
            Destroy(kakka);
        }
        Boids = new List<GameObject>(); // EETU TRIGGER
        spawner = new List<spawn>(); // EETU TRIGGER
    }
}
