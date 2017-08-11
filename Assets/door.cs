using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

enum BossTypes
{
    unInit,
    BigMan,
    BigWolf
}

public class door : MonoBehaviour
{
    bool created = false;
    string seed;
    int height = 50; // mah random
    int widht = 50; // mah random
    int fillpercent = 50;
    public GameObject GodOfTheWorld;
    List<Room> finalRooms = new List<Room>();
    public float mobs = 0;
    private TileMap _tilemap;
    BossTypes boss = BossTypes.unInit;
    void Start()
    {
        _tilemap = GameObject.FindWithTag("Tilemap").GetComponent<TileMap>();
    }
    bool spawnedMobs = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        ChunkMover mover = other.gameObject.transform.parent.GetComponent<ChunkMover>();
        if (mover.UnderGround)
        {
            // maan päälle
            _tilemap.EnableTileMap();
            MobsControl.instance.DeleteAllCurrentMobs();
            mover.UnderGround = false;
            MapGenerator.Instance.DestroyCave();
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<UpdatePathFind>().tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<TileMap>();
            player.GetComponent<UpdatePathFind>().path.map = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<TileMap>();
            int[] m = player.GetComponent<UpdatePathFind>().path.calculateIndex(player.transform.position);
            player.GetComponent<UpdatePathFind>().path.uptadeTiles(m[0], m[1], GameObject.FindGameObjectWithTag("Tilemap").GetComponent<TileMap>());
            ParticleSpawner.instance.destroybloods();
            MobsControl.instance.cave = false;

        }
        else
        {
            _tilemap.DisableTileMap();
            MobsControl.instance.DeleteAllCurrentMobs();
            Activate();
            mover.UnderGround = true;

            MapGenerator dungeon = MapGenerator.Instance;
            var go = GameObject.FindWithTag("SpriteController");
            TileSpriteController tileSpriteController = go.GetComponent<TileSpriteController>();
            tileSpriteController.ResetAllTiles();
            tileSpriteController.SetTileSprites(dungeon.Width - 1, dungeon.Height - 1, dungeon, 1, 1);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<UpdatePathFind>().tilemap = dungeon;

            int[] m = player.GetComponent<UpdatePathFind>().path.calculateIndex(player.transform.position);


            player.GetComponent<UpdatePathFind>().path.uptadeTiles(m[0], m[1], dungeon);

            MobsControl.instance._door = this.gameObject;
            MobsControl.instance.cave = true;

            //player.GetComponent<UpdatePathFind>().path.uptadeTiles(player.transform.position,dungeon);
            if (!spawnedMobs)
            {
                spawnCaveMobs();
                spawnedMobs = true;
            }
            else
            {
                spawnCreadedMobs();
            }
            ParticleSpawner.instance.destroybloods();

        }
    }

    void spawnCreadedMobs()
    {

        int rooms = finalRooms.Count - 1;
        if (rooms == 0)
        {

            return;
        }
        for (int i = 0; i < rooms; i++)
        {
            if (i == 0)
            {
                float rndx = UnityEngine.Random.Range(0f, (float)finalRooms[i].roomsize);
                float rndy = UnityEngine.Random.Range(0f, (float)finalRooms[i].roomsize);
                Vector2 k = new Vector2(finalRooms[i].tiles[(int)rndx].tileX, finalRooms[i].tiles[(int)rndy].tileY);
                /// te = k;
                Vector2 zero = (Vector2)MapGenerator.Instance.GetTileGameObject(0, 0).transform.position;

                switch (boss)
                {
                    case BossTypes.BigMan:
                        MobsControl.instance.spawnBigMan(k.x + zero.x, k.y + zero.y);
                        break;
                    case BossTypes.BigWolf:
                        MobsControl.instance.spawnBigWolf(k.x + zero.x, k.y + zero.y);
                        break;
                    default:
                        print("error loading boss");
                        break;
                }
                // generate boss mayn
                //spawn boss mayn
            }
            if (mobs > 0)
            {
                //spawned = true;
                for (int j = 0; j < mobs; j++)
                {
                    float rndx = UnityEngine.Random.Range(0f, (float)finalRooms[i].roomsize);
                    float rndy = UnityEngine.Random.Range(0f, (float)finalRooms[i].roomsize);

                    Vector2 k = new Vector2(finalRooms[i].tiles[(int)rndx].tileX, finalRooms[i].tiles[(int)rndy].tileY);
                    //te = k;
                    Vector2 zero = (Vector2)MapGenerator.Instance.GetTileGameObject(0, 0).transform.position;
                    MobsControl.instance.SpawnBoids(k.x + zero.x, k.y + zero.y, 0, 1);
                }
            }
        }
    }
    //void Start()
    //{
    //    //Spawner = GameObject.FindGameObjectWithTag("Spawner");
    //    //GameObject tileObject = new GameObject("(" + y + "," + x + ")");
    //    //tileObject.transform.parent = parent.transform;
    //    //tileObject.transform.position = new Vector3(x + chunkOffsetX, y + chunkOffsetY, 0);
    //    //tileObject.layer = ;

    //    //SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
    //    //spriteRenderer.sortingLayerName = "TileMap";
    //    //spriteRenderer.sprite = SuperSprite;
    //}

    public void DeActivate()
    {
        // mene pois luolasta
    }
    // Update is called once per frame
    //void OnDrawGizmos()
    //{
    //    if (created)
    //    {
    //        for(int y = 0;y < height; y++)
    //        {
    //            for (int x = 0; x < widht; x++)
    //            {
    //                Gizmos.color = Color.black;
    //                if ( MapGenerator.Instance.map[x,y] == TileType.CaveFloor)
    //                {
    //                    //Gizmos.DrawCube(new Vector2(x + 100, y + 100),new Vector3(1,1,1));
    //                }
    //                else if (MapGenerator.Instance.map[x, y] == TileType.CaveWall)
    //                {
    //                    Gizmos.DrawSphere(new Vector2(x + 100, y + 100), 1);
    //                }
    //                else
    //                {
    //                    Gizmos.color = Color.yellow;
    //                    Gizmos.DrawSphere(new Vector2(x + 100, y + 100), 10);
    //                }
    //            }
    //        }

    //    }
    //}
    List<Vector2> spawnpos = new List<Vector2>();
    bool spawneddd = false;

    public void spawnCaveMobs()
    {
        spawneddd = true;
        int rooms = finalRooms.Count - 1;
        if (rooms == 0)
        {

            return;
        }
        for (int i = 0; i < rooms; i++)
        {
            if (i == 0)
            {
                // generate boss mayn
                //spawn boss mayn
                float rndx;
                float rndy;
                do
                {
                    rndx = UnityEngine.Random.Range(0f, (float)finalRooms[i].roomsize - 1);
                    rndy = UnityEngine.Random.Range(0f, (float)finalRooms[i].roomsize - 1);
                } while (MapGenerator.Instance.GetTileCollision(finalRooms[i].tiles[(int)rndx].tileX, finalRooms[i].tiles[(int)rndy].tileY));
                Vector2 k = new Vector2(finalRooms[i].tiles[(int)rndx].tileX, finalRooms[i].tiles[(int)rndy].tileY);
               /// te = k;
                Vector2 zero = (Vector2)MapGenerator.Instance.GetTileGameObject(0, 0).transform.position;
                int rndBoss = UnityEngine.Random.Range(0, 1);
                spawnpos.Add(new Vector2(k.x + zero.x, k.y + zero.y));

                switch (rndBoss)
                {
                    case 0:
                        MobsControl.instance.spawnBigWolf(k.x + zero.x, k.y + zero.y);
                        boss = BossTypes.BigWolf;
                        print("boss created wolf");
                        break;
                    case 1:
                        MobsControl.instance.spawnBigMan(k.x + zero.x, k.y + zero.y);
                        boss = BossTypes.BigMan;
                        print("boss created man");
                        break;
                }

            }
            float _mobAmount = finalRooms[i].roomsize / 10;
            int mobAmount = UnityEngine.Random.Range(5,8);
            print(mobAmount);
            mobs += mobAmount;
            if (mobAmount > 1) 
            {
                //spawned = true;
                for(int j = 0;j < mobAmount; j++)
                {
                    float rndx;
                    float rndy;
                    do
                    {
                        rndx = UnityEngine.Random.Range(0f, (float)finalRooms[i].roomsize - 1);
                        rndy = UnityEngine.Random.Range(0f, (float)finalRooms[i].roomsize - 1);
                    } while (MapGenerator.Instance.GetTileCollision(finalRooms[i].tiles[(int)rndx].tileX, finalRooms[i].tiles[(int)rndy].tileY));

                    Vector2 k = new Vector2(finalRooms[i].tiles[(int)rndx].tileX, finalRooms[i].tiles[(int)rndy].tileY);
                    //te = k;
                    Vector2 zero = (Vector2)MapGenerator.Instance.GetTileGameObject(0, 0).transform.position;
                    spawnpos.Add(new Vector2(k.x + zero.x, k.y + zero.y));
                    MobsControl.instance.SpawnBoids(k.x + zero.x, k.y + zero.y, 0, 1);
                }
            }
        }

    }
    //bool spawned = false;
    //Vector2 te = new Vector2(0,0);
    void OnDrawGizmos()
    {

        //Vector2 zero = (Vector2)MapGenerator.Instance.GetTileGameObject(0, 0).transform.position;

        //foreach (Room r in finalRooms)
        //    {
        //        foreach(MapGenerator.Coord t in r.tiles)
        //        {
        //            Gizmos.color = Color.yellow;
        //            Gizmos.DrawSphere(new Vector2(t.tileX + zero.x,t.tileY + zero.y), 1);
        //        }
        //    }
            foreach(Vector2 vec in spawnpos)
            {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(new Vector3(vec.x,vec.y,-1),1);
            }
    }
    void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //nput.mousePosition;
            MobsControl.instance.SpawnBoids(pos.x, pos.y, 0, 1);
            print("spawning");
        }
    }
    void generateBoss()
    {

    }
    Vector2 offset = new Vector2(0, 0);
    public void Activate()
    {
        //destroy world

        //create cave
        if (!created)
        {
            createNewCave();
        }
        finalRooms = MapGenerator.Instance.GenerateMap(widht, height, seed, fillpercent);
        if (!created)                                                                                   // create door
        {
            int temp = finalRooms[finalRooms.Count - 1].edgeTiles.Count;
            int rand = UnityEngine.Random.Range(0, temp);

            MapGenerator.Instance.map[finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileX, finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileY - 1] = TileType.CaveDoor;
            offset = new Vector2((float)finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileX,(float) finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileY - 1);
            created = true;
            print("door created");


        }
        Vector2 mapPosition = (Vector2)transform.position - offset;

        MapGenerator.Instance.showRooms(mapPosition);

        //smooth
        //spawn

        // for(;;asdas123123123)
        //    playbackInput();
        //    updtea();

        //draw():;




        ////drawCave
        //GameObject parent = new GameObject("CAVES!");
        ////parent.transform.parent = tilemap;

        //// DELETE OLD ONES!!!! HOX

        //for (int x = 0; x < widht; x++)
        //{
        //    for (int y = 0; y < height; y++)
        //    {
        //        GameObject tileObject = new GameObject("(" + y + "," + x + ")");
        //        tileObject.transform.parent = parent.transform;
        //        tileObject.transform.position = new Vector3(x , y, 0);

        //        SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
        //        if(MapGenerator.Instance.map[x,y] == TileType.CaveFloor)
        //        {
        //            spriteRenderer.sprite = GrassSprite;
        //        }
        //        else if(MapGenerator.Instance.map[x, y] == TileType.CaveWall)
        //        {
        //            spriteRenderer.sprite = SuperSprite;
        //        }
        //        else
        //        {
        //            //print("drawing star point");
        //            spriteRenderer.sprite = StartSprite;
        //        }
        //    }
        //}

        //for (int i = 0;i < finalRooms.Count; i++)
        //{
        //    if(finalRooms[i].roomsize > 10)
        //    {
        //        MobsControl.instance.SpawnBoids((float)finalRooms[i].tiles[0].tileX, (float)finalRooms[i].tiles[0].tileY, 5,5);//EETU TRIGGER
        //    }
        //    else if (finalRooms[i].roomsize < 20)
        //    {
        //        MobsControl.instance.SpawnBoids((float)finalRooms[i].tiles[0].tileX, (float)finalRooms[i].tiles[0].tileY, 5, 10);//EETU TRIGGER
        //    }
        //    if (finalRooms[i].roomsize < 30)
        //    {
        //        MobsControl.instance.SpawnBoids((float)finalRooms[i].tiles[0].tileX, (float)finalRooms[i].tiles[0].tileY, 5, 17);//EETU TRIGGER
        //    }
        //}


    }
    void createNewCave()
    {
        seed = DateTime.Now.Ticks.ToString();
    }
}
