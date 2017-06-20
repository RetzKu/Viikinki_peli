﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Room : IComparable<Room>
{
    public List<MapGenerator.Coord> tiles;
    public List<MapGenerator.Coord> edgeTiles;
    public List<Room> connectedRooms;
    public int roomsize;
    public bool isAccessibleFromMainRoom;
    public bool isMainRoom;
    public Room()
    { }

    public Room(List<MapGenerator.Coord> roomTiles, int[,] map)
    {
        tiles = roomTiles;
        roomsize = tiles.Count;
        connectedRooms = new List<Room>();
        edgeTiles = new List<MapGenerator.Coord>();

        foreach (MapGenerator.Coord tile in tiles)
        {
            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (x == tile.tileX || y == tile.tileY)
                    {
                        if (map[x, y] == 1)
                        {
                            edgeTiles.Add(tile);
                        }
                    }
                }
            }
        }
    }

    public void SetAccessibleFromMainRoom()
    {
        if (!isAccessibleFromMainRoom)
        {
            isAccessibleFromMainRoom = true;
            foreach (Room connectedRoom in connectedRooms)
            {
                connectedRoom.SetAccessibleFromMainRoom();
            }
        }
    }

    public int CompareTo(Room otherRoom)
    {
        return otherRoom.roomsize.CompareTo(roomsize);
    }
    public static void ConnectRooms(Room roomA, Room roomB)
    {
        if (roomA.isAccessibleFromMainRoom)
        {
            roomB.SetAccessibleFromMainRoom();
        }
        else if (roomB.isAccessibleFromMainRoom)
        {
            roomA.SetAccessibleFromMainRoom();
        }
        roomA.connectedRooms.Add(roomB);
        roomB.connectedRooms.Add(roomA);
    }
    public bool isConnected(Room otherRoom)
    {
        return connectedRooms.Contains(otherRoom);
    }


}




public class MapGenerator
{
    private static MapGenerator instance;

    public static MapGenerator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MapGenerator();
            }
            return instance;
        }
    }

    int widht;
    int height;
    public 
    string seed;
    int smoothTimes = 4;
    int walls = 4;
    //public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    public int[,] map; // 0 empty tile, 1 wall tile
    

    public List<Room> GenerateMap(int widht, int height, string seed, int fillPercent)
    {
        this.widht = widht;
        this.height = height;
        this.seed = seed;
        this.randomFillPercent = fillPercent;

        map = new int[widht, height];
        RandomFillMap();

        for (int i = 0; i < smoothTimes; i++)
        {
            SmoothMap();
        }

        List<Room> surviving = ProcessMap();

        return surviving;
    }

    void RandomFillMap()
    {
        //if (useRandomSeed)
        //{
        //var joku = DateTime.Now.Second;
        //seed = DateTime.Now.Ticks.ToString();
        
        //    seed = Time.time.ToString();
        //}
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        for (int x = 0; x < widht; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == widht - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    List<Room> ProcessMap()
    {
        List<List<Coord>> wallregions = GetRegions(1);

        int wallThresholdSize = 50; // any wall region which is less than 50 tiles will be removed

        foreach (List<Coord> wallregion in wallregions)
        {
            if (wallregion.Count < wallThresholdSize)
            {
                foreach (Coord tile in wallregion)
                {
                    map[tile.tileX, tile.tileY] = 0;
                }
            }
        }

        List<List<Coord>> roomregions = GetRegions(0);

        int roomThresholdSize = 50; // any wall region which is less than 50 tiles will be removed

        List<Room> survivingRooms = new List<Room>();

        foreach (List<Coord> roomregion in roomregions)
        {
            if (roomregion.Count < roomThresholdSize)
            {
                foreach (Coord tile in roomregion)
                {
                    map[tile.tileX, tile.tileY] = 1;
                }
            }
            else
            {
                survivingRooms.Add(new Room(roomregion, map));
            }
        }
        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;

        ConnectClosectRooms(survivingRooms);

        return survivingRooms;
        
    }
    void ConnectClosectRooms(List<Room> allRooms, bool forceAccesibilityFromMainRoom = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccesibilityFromMainRoom)
        {
            foreach (Room room in allRooms)
            {
                if (room.isAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        foreach (Room roomA in roomListA)
        {
            if (!forceAccesibilityFromMainRoom)
            {
                possibleConnectionFound = false;
                if (roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }

            foreach (Room roomB in roomListB)
            {
                if (roomA == roomB || roomA.isConnected(roomB))
                {
                    continue;
                }


                //if (roomA.isConnected(roomB))
                //{
                //    possibleConnectionFound = false;
                //    break;
                //}
                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                    {
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];

                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                        {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }
            if (possibleConnectionFound && !forceAccesibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }
        if (possibleConnectionFound && forceAccesibilityFromMainRoom)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosectRooms(allRooms, true);
        }


        if (!forceAccesibilityFromMainRoom)
        {
            ConnectClosectRooms(allRooms, true);
        }
    }

    void drawCircle(Coord c, int r)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x * x + y * y <= r * r)
                {
                    int realX = c.tileX + x;
                    int realY = c.tileY + y;
                    if (IsInMapRange(realX, realY))
                    {
                        map[realX, realY] = 0;
                    }
                }
            }
        }
    }

    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        Room.ConnectRooms(roomA, roomB);



        //Debug.DrawLine(CoordToWOrldPoint(tileA), CoordToWOrldPoint(tileB), Color.green, 100);

        List<Coord> line = GetLine(tileA, tileB);
        foreach (Coord c in line)
        {
            drawCircle(c, 1);
        }


    }
    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();
        int x = from.tileX;
        int y = from.tileY;

        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;

        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        bool inverted = false;
        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));
            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }
            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }

        }
        return line;
    }

    Vector3 CoordToWOrldPoint(Coord tile)
    {
        return new Vector3(-widht / 2 + .5f + tile.tileX, 2, -height / 2 + .5f + tile.tileY);
    }
    List<List<Coord>> GetRegions(int tiletype)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[widht, height];
        for (int x = 0; x < widht; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapFlags[x, y] == 0 && map[x, y] == tiletype)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }

        return regions;
    }

    void SmoothMap()
    {
        for (int x = 0; x < widht; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = getSurroundingWallCount(x, y);

                if (neighbourWallTiles > walls)
                {
                    map[x, y] = 1;
                }
                else if (neighbourWallTiles < walls)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[widht, height];
        int tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));

        mapFlags[startX, startY] = 1;
        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                    {
                        if (mapFlags[x, y] == 0 && map[x, y] == tileType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return tiles;

    }

    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < widht && y >= 0 && y < height;
    }

    int getSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }

            }
        }
        return wallCount;
    }
    public struct Coord
    {
        public int tileX;
        public int tileY;

        public Coord(int x, int y)
        {
            tileX = x;
            tileY = y;
        }
    }

    


    //private void OnDrawGizmos()
    //{
    //    if (map != null)
    //    {
    //        for (int x = 0; x < widht; x++)
    //        {
    //            for (int y = 0; y < height; y++)
    //            {
    //                Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
    //                Vector3 pos = new Vector3(-widht / 2 + x + .5f, 0, -height / 2 + y + .5f);
    //                Gizmos.DrawCube(pos, Vector3.one);
    //            }
    //        }
    //    }
    //}
}

