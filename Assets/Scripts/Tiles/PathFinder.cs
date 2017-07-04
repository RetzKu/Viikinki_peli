using System.Collections.Generic;
using UnityEngine;



public class PathFinder /*: MonoBehaviour*/
{
    public enum Dir
    {
        NoDir,
        NoWayOut,
        Up,
        Right,
        Down,
        Left,

    }

    private static readonly int Width = 60;
    private static readonly int Height = 60;
    List<List<BreadthFirstSearch.tiles>> realMap;
    //private TileType[,] testMap = new TileType[Height, Width];
    public Dir[,] dirs = new Dir[Height, Width];
    public bool run = false;

    public int GoalX;
    public int GoalY;
    public Texture2D GoalTexture2D;

    //private void OnDrawGizmos()
    //{
    //    if (run)
    //    {
    //        Gizmos.color = Color.black;

    //        for (int y = 0; y < Height; y++)
    //        {
    //            for (int x = 0; x < Width; x++)
    //            {
    //                var pos = new Vector3(x + 1, y + 1);    // tiilen origo
    //                Gizmos.DrawLine(pos, (GetDir(dirs[y, x]) + pos));
    //                Gizmos.DrawCube(GetDir(dirs[y, x]) + pos, new Vector3(0.1f, 0.1f));
    //            }
    //        }

    //        for (int yy = 0; yy < Height; yy++)
    //        {
    //            for (int xx = 0; xx < Width; xx++)
    //            {
    //                var pos = new Vector3(xx + 1f, yy + 1f);    // tiilen origo
    //                Gizmos.DrawWireCube(pos, new Vector3(1f, 1f));
    //            }
    //        }

    //        Gizmos.DrawGUITexture(new Rect(GoalX + 1f, GoalY + 2f, 1f, -1f), GoalTexture2D);
    //    }
    //}

    Vector3 GetDir(Dir dir)
    {
        const float len = 0.35f;
        switch (dir)
        {
            case Dir.NoDir:
                break;
            case Dir.Up:
                return Vector2.up * len;
            case Dir.Right:
                return Vector2.right * len;
            case Dir.Down:
                return Vector2.down * len;
            case Dir.Left:
                return Vector2.left * len;
        }
        return Vector2.zero;
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        dirs[0, 0] = Dir.Up;
    //        dirs[0, 1] = Dir.Up;
    //    }

    //    if (Input.GetKeyDown(KeyCode.G))
    //    {
    //       Search();
    //    }
    //}


    static readonly Vec2[] Neighbours = new Vec2[]
    {
        new Vec2(1, 0), new Vec2(0, -1),
        new Vec2(-1, 0), new Vec2(0, 1)
    };

    List<Vec2> GetNeighbours(Vec2 location)
    {
        List<Vec2> value = new List<Vec2>(4);
        foreach (Vec2 offset in Neighbours)
        {
            Vec2 next = new Vec2(location.X + offset.X, location.Y + offset.Y);
            if (InBounds(next) && realMap[next.Y][next.X].tileState != BreadthFirstSearch.states.wall)     // passable jne......
            {
                value.Add(next);
            }
        }

        if ((location.X + location.Y) % 2 == 0)
        {
            value.Reverse();
        }

        return value;
    }

    bool InBounds(Vec2 location)
    {
        return InBounds(location.X, location.Y);
    }

    bool InBounds(int x, int y)
    {
        return (x >= 0 && y >= 0 && x < Width && y < Height);
    }


    public void Search(List<List<BreadthFirstSearch.tiles>> moveTiles, int goalX, int goalY)
    {
        this.GoalX = goalX;
        this.GoalY = goalY;
        realMap = moveTiles;
        Queue<Vec2> frontier = new Queue<Vec2>();

        Vec2 start = new Vec2(GoalX, GoalY);
        frontier.Enqueue(start);

        Dictionary<Vec2, Vec2> cameFrom = new Dictionary<Vec2, Vec2>();
        cameFrom[start] = start;

        dirs = new Dir[Height, Width];

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            var n = GetNeighbours(current);

            for (int i = 0; i < n.Count; i++)
            {
                var next = n[i];
                if (!cameFrom.ContainsKey(next)) // LOL
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        GeneratePaths(cameFrom);
    }

    private void GeneratePaths(Dictionary<Vec2, Vec2> cameFrom)
    {
        for (int y = 1; y < Height - 1; y++)
        {
            for (int x = 1; x < Width - 1; x++)
            {
                if (realMap[y][x].tileState != BreadthFirstSearch.states.wall && dirs[y, x] != Dir.NoWayOut)
                {
                    Vec2 vec = new Vec2(x, y);
                    Vec2 value;
                    bool safe = cameFrom.TryGetValue(vec, out value);
                    if (safe)
                        dirs[y, x] = GetDirection(vec, value);
                }
            }
        }
    }

    Dir GetDirection(Vec2 xy, Vec2 cameFrom)
    {
        if (xy.X - 1 == cameFrom.X)
        {
            return Dir.Left;
        }
        if (xy.X + 1 == cameFrom.X)
        {
            return Dir.Right;
        }

        if (xy.Y - 1 == cameFrom.Y)
        {
            return Dir.Down;
        }
        if (xy.Y + 1 == cameFrom.Y)
        {
            return Dir.Up;
        }
        return Dir.NoDir;
    }
}