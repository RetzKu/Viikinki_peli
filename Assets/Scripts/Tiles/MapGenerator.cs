using System.Collections;
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

    public Room(List<MapGenerator.Coord> roomTiles, TileType[,] map)
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
                        if (map[x, y] == TileType.CaveWall)
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


public interface ITileMap
{
    int Height { get; set; }
    int Width  { get; set; }
    GameObject GetTileGameObject(int x, int y);
    TileType GetTile(int x, int y);
    bool GetTileCollision(int x, int y);
    bool CanUpdatePathFind();
}


public class MapGenerator : MonoBehaviour, ITileMap
{
    public Sprite GrassSprite;
    public Sprite SuperSprite;
    public Sprite StartSprite;

    private Vector3 StartPosition;
    private GameObject _cavesParent;

    GameObject[,] TileObjects;
    private static MapGenerator instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _cavesParent = new GameObject("Caves");
    }

    public static MapGenerator Instance
    {
        get
        {
            return instance;
        }
    }

    public int Width { get; set; }
    public int Height { get; set; }

    string seed;
    int smoothTimes = 4;
    int walls = 4;
    //public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    public TileType[,] map; // 0 empty tile, 1 wall tile

    public bool CanUpdatePathFind()
    {
        return true;
    }

    bool inited = false ;//debug
    public List<Room> GenerateMap(int widht, int height, string seed, int fillPercent)
    {
        inited = true;
        this.Width = widht;
        this.Height = height;
        this.seed = seed;
        this.randomFillPercent = fillPercent;

        map = new TileType[widht, height];
        RandomFillMap();

        for (int i = 0; i < smoothTimes; i++)
        {
            SmoothMap();
        }

        List<Room> surviving = ProcessMap();

        return surviving;
    }
    public TileType GetTile(int x,int y)
    {
        return map[x, y];
    }

    public GameObject GetTileGameObject(int x, int y) //TODO: !!
    {
        return TileObjects[x,y];
    }
    void OnDrawGizmos()
    {
        if (inited)
        {
            Gizmos.DrawSphere(GetTileGameObject(0, 0).transform.position, 1);
            Gizmos.DrawSphere(GetTileGameObject(Width - 1,Height -1).transform.position, 1);
            Gizmos.DrawSphere(GetTileGameObject(Width - 1, Height - 1).transform.position, 1);
        }
    }
    public bool GetTileCollision(int x,int y)
    {
        return map[x, y] == TileType.CaveWall;
    }
    public void DestroyCave()
    {
        for(int y = 0;y < TileObjects.GetLength(0); y++)
        {
            for (int x = 0; x < TileObjects.GetLength(1); x++)
            {
                Destroy(TileObjects[y, x]);
            }
        }
        TileObjects = new GameObject[Width,Height];

    }
    public void showRooms(Vector2 offset)
    {
        TileObjects = new GameObject[Width, Height];
        
        // GameObject parent = new GameObject("CAVES!");

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                GameObject tileObject = new GameObject("(" + y + "," + x + ")");
                tileObject.transform.parent = _cavesParent.transform;
                tileObject.transform.position = new Vector3(x + offset.x, y + offset.y, 0);

                SpriteRenderer spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingLayerName = "TileMap";
                if (MapGenerator.Instance.map[x, y] == TileType.CaveFloor)
                {
                    spriteRenderer.sprite = GrassSprite;
                }
                else if (MapGenerator.Instance.map[x, y] == TileType.CaveWall)
                {
                    spriteRenderer.sprite = SuperSprite;
                    tileObject.AddComponent<BoxCollider2D>();
                    tileObject.layer = 19;
                }
                else
                {
                    //print("drawing star point");
                    spriteRenderer.sprite = StartSprite;
                }
                TileObjects[x, y] = tileObject;
            }
        }

        //fire.transform.parent = GameObject.Find("luola_tuho").transform;
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
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                {
                    map[x, y] = TileType.CaveWall;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? TileType.CaveWall : TileType.CaveFloor;
                }
            }
        }
    }

    List<Room> ProcessMap()
    {
        List<List<Coord>> wallregions = GetRegions(TileType.CaveWall);

        int wallThresholdSize = 50; // any wall region which is less than 50 tiles will be removed

        foreach (List<Coord> wallregion in wallregions)
        {
            if (wallregion.Count < wallThresholdSize)
            {
                foreach (Coord tile in wallregion)
                {
                    map[tile.tileX, tile.tileY] = TileType.CaveFloor;
                }
            }
        }

        List<List<Coord>> roomregions = GetRegions(TileType.CaveFloor);

        int roomThresholdSize = 50; // any wall region which is less than 50 tiles will be removed

        List<Room> survivingRooms = new List<Room>();

        foreach (List<Coord> roomregion in roomregions)
        {
            if (roomregion.Count < roomThresholdSize)
            {
                foreach (Coord tile in roomregion)
                {
                    map[tile.tileX, tile.tileY] = TileType.CaveWall;
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
                        map[realX, realY] = TileType.CaveFloor;
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
        return new Vector3(-Width / 2 + .5f + tile.tileX, 2, -Height / 2 + .5f + tile.tileY);
    }
    List<List<Coord>> GetRegions(TileType tiletype)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        TileType[,] mapFlags = new TileType[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                mapFlags[x,y] = TileType.CaveFloor;
            }
            
        }
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (mapFlags[x, y] == TileType.CaveFloor && map[x, y] == tiletype)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = TileType.CaveWall;
                    }
                }
            }
        }

        return regions;
    }

    void SmoothMap()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                int neighbourWallTiles = getSurroundingWallCount(x, y);

                if (neighbourWallTiles > walls)
                {
                    map[x, y] = TileType.CaveWall;
                }
                else if (neighbourWallTiles < walls)
                {
                    map[x, y] = TileType.CaveFloor;
                }
            }
        }
    }

    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        TileType[,] mapFlags = new TileType[Width, Height];
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                mapFlags[x, y] = TileType.CaveFloor;
            }

        }
        TileType tileType = map[startX, startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));

        mapFlags[startX, startY] = TileType.CaveWall;
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
                        if (mapFlags[x, y] == TileType.CaveFloor && map[x, y] == tileType)
                        {
                            mapFlags[x, y] = TileType.CaveWall;
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
        return x >= 0 && x < Width && y >= 0 && y < Height;
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
                        if(map[neighbourX, neighbourY] == TileType.CaveWall) { wallCount++; }
                        //wallCount += map[neighbourX, neighbourY];
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
    //        for (int x = 0; x < Width; x++)
    //        {
    //            for (int y = 0; y < Height; y++)
    //            {
    //                Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
    //                Vector3 pos = new Vector3(-Width / 2 + x + .5f, 0, -Height / 2 + y + .5f);
    //                Gizmos.DrawCube(pos, Vector3.one);
    //            }
    //        }
    //    }
    //}
}

