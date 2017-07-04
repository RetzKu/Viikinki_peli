﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch
{
    bool inited = false;

    int goalX, goalY;
    public List<List<tiles>>  moveTiles = new List<List<tiles>>();

    public TileMap map;

    public class tiles
    {
        public int x, y;
        public states tileState;
        public tiles()
        {

        }
    }
    public enum states
    {
        goal,
        unVisited,
        wall,
        up,
        down,
        right,
        left
    }

    public states getTileDir(Vector2 k)
    {
        if (inited)
        {
            int[] j = calculateIndex(k);              
            return moveTiles[j[1]][j[0]].tileState; // palauttaa movement laskujen mukaan x y position
        }
        else
        {
            return states.wall;                     // EETU TRIGGER
        }
    }



    int[] calculateIndex(Vector2 k)
    {
       Vector3 j = map.GetGameObjectFast(0, 0).transform.position;
       Vector2 i = j;
       Vector2 temp = k - i;

        int[] h = new int[2];
        h[0] = (int)temp.x;
        h[1] = (int)temp.y;


        return h;
    }
    public void uptadeTiles(Vector2 position,TileMap tileMap)
    {
        uptadeTiles((int)position.x, (int)position.y, tileMap);
    }

    public void uptadeTiles(int playerX, int playerY, TileMap tileMap)
    {
        inited = true;
        playerX--; // KORJAA
        playerY--; // KORJAA
        map = tileMap; //EETU TRIGGER, mah ei tarvi korjata
        moveTiles.Clear(); 
        for (int y = 0; y < TileMap.TotalHeight; y++)
        {
            List<tiles> temp = new List<tiles>();
            for (int x = 0; x < TileMap.TotalWidth; x++)
            {
                if (x == playerX && y == playerY)
                        temp.Add(new tiles() { x = x, y = y, tileState = states.goal});
                else
                        temp.Add(new tiles() { x = x, y = y, tileState = TileMap.Collides(tileMap.GetTileFast(x, y)) ? states.wall : states.unVisited});   /*GET WALL INFO*/
                
            }
            moveTiles.Add(temp);
        } 
        goalX = playerX;
        goalY = playerY;

        Dictionary<tiles, int> toimii = new Dictionary<tiles, int>();

        Queue<tiles> hue = new Queue<tiles>(100);
        hue.Enqueue(moveTiles[playerY][playerX]);


        while (hue.Count != 0)
        {
            tiles current = hue.Dequeue();

            if (current.x - 1 >= 0 && moveTiles[current.y][current.x - 1].tileState == states.unVisited)
            {
                tiles tile = moveTiles[current.y][current.x - 1];
                hue.Enqueue(tile);
                tile.tileState = states.right;
            }
            if (current.x + 1 <= 59 && moveTiles[current.y][current.x + 1].tileState == states.unVisited)
            {
                tiles tile = moveTiles[current.y][current.x + 1];
                hue.Enqueue(tile);
                tile.tileState = states.left;
            }
            if (current.y + 1 <= 59 && moveTiles[current.y + 1][current.x].tileState == states.unVisited)
            {
                tiles tile = moveTiles[current.y + 1][current.x];
                hue.Enqueue(tile);
                tile.tileState = states.down;
            }
            if (current.y - 1 >= 0 && moveTiles[current.y - 1][current.x].tileState == states.unVisited)
            {
                tiles tile = moveTiles[current.y-1][current.x];
                hue.Enqueue(tile);
                tile.tileState = states.up;
            }
        }


    }
}
