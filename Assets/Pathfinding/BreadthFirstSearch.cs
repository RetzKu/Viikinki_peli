using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch
{

    int goalX, goalY;
    List<List<tiles>>  moveTiles = new List<List<tiles>>();

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

    public states getTileDir(int x, int y)
    {
        return moveTiles[x][y].tileState; // palauttaa movement laskujen mukaan x y position
    }
    public void uptadeTiles(List<List<tiles>> tiles_list, int playerX, int playerY)
    {
        moveTiles.Clear();
        for (int y = 0; y < tiles_list.Count;y++)
        {
            List<tiles> temp = new List<tiles>();
            for (int x = 0; x < tiles_list[y].Count; y++)
            {
                // get walls 
                temp.Add(new tiles() { x = x, y = y, tileState = states.unVisited/*GET WALL INFO*/ });
            }
            moveTiles.Add(temp);
        } 
        goalX = playerX;

        goalY = playerY;
        tiles_list[playerY][playerX].tileState = states.goal;
        List<tiles> frontier = new List<tiles>();
        frontier.Add(tiles_list[playerY][playerX]);
        //int tileValue = 1;

        while (frontier.Count != 0)
        {
            foreach (tiles i in frontier) // check neighbours
            {
                if (tiles_list[i.x - 1][i.y].tileState == states.unVisited)
                {
                    frontier.Add(tiles_list[i.x - 1][i.y]);
                    tiles_list[i.x - 1][i.y].tileState = states.right;
                    //tiles_list[i.x - 1][i.y].value = tileValue++;
                }
                if (tiles_list[i.x + 1][i.y].tileState == states.unVisited)
                {
                    frontier.Add(tiles_list[i.x + 1][i.y]);
                    tiles_list[i.x + 1][i.y].tileState = states.left;
                    //tiles_list[i.x + 1][i.y].value = tileValue++;
                }
                if (tiles_list[i.x][i.y - 1].tileState == states.unVisited)
                {
                    frontier.Add(tiles_list[i.x][i.y - 1]);
                    tiles_list[i.x][i.y - 1].tileState = states.up;
                    //tiles_list[i.x ][i.y- 1].value = tileValue++;
                }
                if (tiles_list[i.x][i.y + 1].tileState == states.unVisited)
                {
                    frontier.Add(tiles_list[i.x][i.y + 1]);
                    tiles_list[i.x][i.y + 1].tileState = states.down;
                    //tiles_list[i.x][i.y + 1].value = tileValue++;
                }
                frontier.Remove(i); // check does this work
            }
        }

    }
}
