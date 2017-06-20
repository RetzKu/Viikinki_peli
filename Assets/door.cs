using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class door : MonoBehaviour {

    bool created = false;
    string seed;
    int height = 70; // mah random
    int widht = 100; // mah random
    int fillpercent = 55;

    List<Room> finalRooms = new List<Room>();

    public Sprite GrassSprite;
    public Sprite SuperSprite;
    public Sprite StartSprite;


    void Start()
    {
        //Activate();
        // init sprites
    }

    // Update is called once per frame
    public void Activate () {
        //destroy world

        //create cave
        if (!created)
        {
            createNewCave();
        }
        finalRooms =  MapGenerator.Instance.GenerateMap(widht, height, seed, fillpercent);
        if (!created)
        {
            int temp = finalRooms[finalRooms.Count - 1].edgeTiles.Count;
            int rand = UnityEngine.Random.Range(0, temp);

            if (MapGenerator.Instance.map[finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileX, finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileY-1] == 1)
            {
                MapGenerator.Instance.map[finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileX, finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileY - 1] = 2;
            }
            else if (MapGenerator.Instance.map[finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileX, finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileY +1] == 1)
            {
                MapGenerator.Instance.map[finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileX, finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileY + 1] = 2;
            }
            else if (MapGenerator.Instance.map[finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileX-1, finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileY] == 1)
            {
                MapGenerator.Instance.map[finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileX - 1, finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileY] = 2;
            }
            else
            {
                MapGenerator.Instance.map[finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileX + 1, finalRooms[finalRooms.Count - 1].edgeTiles[rand].tileY] = 2;
            }

        }
        
        //drawCave
        GameObject parent = new GameObject("CAVES!");
        //parent.transform.parent = tilemap;

        // DELETE OLD ONES!!!! HOX

        for (int x = 0; x < widht; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tileObject = new GameObject("(" + y + "," + x + ")");
                tileObject.transform.parent = parent.transform;
                tileObject.transform.position = new Vector3(x , y, 0);

                SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                if(MapGenerator.Instance.map[x,y] == 0)
                {
                    spriteRenderer.sprite = GrassSprite;
                }
                else if(MapGenerator.Instance.map[x, y] == 1)
                {
                    spriteRenderer.sprite = SuperSprite;
                }
                else
                {
                    //print("drawing star point");
                    spriteRenderer.sprite = StartSprite;
                }



            }
        }
        //var go = Instantiate(EnemyPrefab, new Vector2(x, y), Quaternion.identity);
        //go.GetComponent<EnemyMovement>().InitStart(x, y);

    }
    void createNewCave()
    {
        seed = DateTime.Now.Ticks.ToString();
    }
}
