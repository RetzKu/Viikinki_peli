﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class door : MonoBehaviour
{

    bool created = false;
    string seed;
    int height = 50; // mah random
    int widht = 50; // mah random
    int fillpercent = 45;

    List<Room> finalRooms = new List<Room>();

    private TileMap _tilemap;

    void Start()
    {
        _tilemap = GameObject.FindWithTag("Tilemap").GetComponent<TileMap>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ChunkMover mover = other.gameObject.transform.parent.GetComponent<ChunkMover>();
        if (mover.UnderGround)
        {
            // maan päälle
            _tilemap.EnableTileMap();

            mover.UnderGround = false;
        }
        else
        {
            _tilemap.DisableTileMap();
            Activate();
            mover.UnderGround = true;

            MapGenerator dungeon = MapGenerator.Instance;
            var go = GameObject.FindWithTag("SpriteController");
            TileSpriteController tileSpriteController = go.GetComponent<TileSpriteController>();
            tileSpriteController.ResetAllTiles();
            tileSpriteController.SetTileSprites(dungeon.Width - 1, dungeon.Height - 1, dungeon, 1, 1);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<UpdatePathFind>().path.map = dungeon;
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
    public void spawnCaveMobs()
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
                // generate boss mayn
                //spawn boss mayn
            }
            float mobAmount = finalRooms[i].roomsize / 10;
            if (mobAmount > 1)
            {
                Vector2 k = new Vector2(finalRooms[i].tiles[6].tileX, finalRooms[i].tiles[6].tileY);
                MobsControl.instance.SpawnBoids(k.x, k.y, 2, (int)mobAmount);
            }
        }
    }
    void generateBoss()
    {

    }

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
            created = true;
            print("door created");


        }

        MapGenerator.Instance.showRooms();

        //smooth
        //spawn







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
