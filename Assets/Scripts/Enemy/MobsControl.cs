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
    public GameObject Bear;
    public GameObject BigMan;
    public GameObject BigWolf;

    List<GameObject> Boids;

    public GameObject player;

    public bool spawnWolfs;
    public bool spawnMelee;
    public bool spawnArchers;
    public bool spawnBears;
    public bool naturalSpawn;
    List<spawn> spawner = new List<spawn>();

    [HideInInspector]
    public GameObject _door { set { door = value; } }
    GameObject door;
    public bool cave = false;

    [Header("KÄYTÄ JOS USKALLAT")]
    [Range(0, 50)]
    public int noSpawn;
    [Range(0, 50)]
    public int MeleeR;
    [Range(0, 50)]
    public int ArcherR;
    [Range(0, 50)]
    public int WolfR;
    [Range(0, 50)]
    public int BearR;


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
        if (!naturalSpawn)
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
                int tries = 0;
                do
                {
                    x = Random.Range(spawner[0].x - spawner[0].rad, spawner[0].x + spawner[0].rad);
                    y = Random.Range(spawner[0].y - spawner[0].rad, spawner[0].y + spawner[0].rad);

                    k = player.GetComponent<UpdatePathFind>().path.getTileDir(new Vector2(x, y));
                    tries++;
                }
                while (k == PathFinder.Dir.NoDir && tries < 5);
                if (tries <= 5 && x > 0 && y > 0 && k != PathFinder.Dir.error)
                {
                    if (!naturalSpawn)
                    {
                        if (spawnMelee)
                        {
                            GameObject m;
                            m = Instantiate(MeleeDude, new Vector2(x, y), Quaternion.identity);
                            m.GetComponent<generalAi>()._InitStart(x, y, EnemyType.Archer, player);
                            Boids.Add(m);
                        }
                        if (spawnWolfs)
                        {
                            GameObject m;
                            m = Instantiate(Wolf, new Vector2(x, y), Quaternion.identity);
                            m.GetComponent<generalAi>()._InitStart(x, y, EnemyType.Wolf, player);
                            Boids.Add(m);
                        }

                    }
                    else if (tries < 5)
                    {
                        randomSpawn(x, y);
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////////
                    //wolfBoids.Add(go);

                }
                if (tries > 5 && cave)
                {
                    door.GetComponent<door>().mobs -= 1;
                }
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
                if (Boids[ind].GetComponent<generalAi>().MyType == EnemyType.bear && Boids[ind].GetComponent<generalAi>().kys)
                {

                    Boids[ind].GetComponent<generalAi>().enabled = false;
                    Boids[ind].transform.parent = GameObject.Find("luola_tuho").transform;
                }
                else
                {
                    Destroy(Boids[ind]);
                }
                Boids.Remove(Boids[ind]);
                if (cave)
                {
                    door.GetComponent<door>().mobs -= 1;
                }
            }
            else
            {
                if (slow)
                {
                    Boids[ind].GetComponent<generalAi>().SlowRune(5f, 0.5f);
                    print("slow");
                }
                //Boids[ind].GetComponent<generalAi>().UpdatePosition();
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

    public int DeleteAllCurrentMobs() // korjaa
    {
        int l = Boids.Count;
        foreach (GameObject kakka in Boids)
        {
            Destroy(kakka);
        }
        Boids.Clear();
        spawner.Clear();
        return l;
    }
    public void spawnBigMan(float x, float y)
    {
        GameObject m;
        m = Instantiate(BigMan, new Vector2(x, y), Quaternion.identity);
        m.GetComponent<generalAi>()._InitStart(x, y, EnemyType.Archer, player);
        Boids.Add(m);
    }
    public void spawnBigWolf(float x, float y)
    {
        GameObject m;
        m = Instantiate(BigWolf, new Vector2(x, y), Quaternion.identity);
        m.GetComponent<generalAi>()._InitStart(x, y, EnemyType.Wolf, player);
        Boids.Add(m);
    }

    void randomSpawn(float x, float y)
    {
        //for(int i = 0; i < maxamount; i++)
        //{
        GameObject go;
        int r = UnityEngine.Random.Range(0, 50);
        if (r > noSpawn)
        {
            r -= noSpawn;
            int tulos = round(r);

            if (tulos == BearR)
            {
                go = Instantiate(Bear, new Vector2(x, y), Quaternion.identity);
                go.GetComponent<generalAi>()._InitStart(x, y, EnemyType.bear, player);
            }
            else if (tulos == ArcherR)
            {
                go = Instantiate(Archer, new Vector2(x, y), Quaternion.identity);
                go.GetComponent<generalAi>()._InitStart(x, y, EnemyType.Archer, player);
            }
            else if (tulos == MeleeR)
            {
                go = Instantiate(MeleeDude, new Vector2(x, y), Quaternion.identity);
                go.GetComponent<generalAi>()._InitStart(x, y, EnemyType.Archer, player);
            }
            else if (tulos == WolfR)
            {
                go = Instantiate(Wolf, new Vector2(x, y), Quaternion.identity);
                go.GetComponent<generalAi>()._InitStart(x, y, EnemyType.Wolf, player);
            }
            else
            {
                Debug.Log("INVALID SPAWN VALUE");
                return;
            }
            Boids.Add(go);

        }
        // }
    }


    int round(int r)
    {
        return (Mathf.Abs(ArcherR - r) < Mathf.Abs(MeleeR - r)) ? (Mathf.Abs(WolfR - r) < Mathf.Abs(ArcherR - r)) ? (Mathf.Abs(WolfR - r) < Mathf.Abs(BearR - r)) ? WolfR : BearR : (Mathf.Abs(ArcherR - r) < Mathf.Abs(BearR - r)) ? ArcherR : BearR : (Mathf.Abs(WolfR - r) < (MeleeR - r)) ? (Mathf.Abs(WolfR - r) < Mathf.Abs(BearR - r)) ? WolfR : BearR : (Mathf.Abs(MeleeR - r) < Mathf.Abs(BearR - r)) ? MeleeR : BearR;
    }

}
