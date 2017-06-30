using UnityEngine;

[System.Serializable]
public class Vec2
{
    public int X;
    public int Y;
    public Vec2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Vec2 a, Vec2 b)
    {
        return (a.X == b.X && a.Y == b.Y);
    }

    public static bool operator !=(Vec2 a, Vec2 b)
    {
        return !(a == b);
    }

    //public override bool Equals(object obj)
    //{
    //    if (obj == null)
    //    {
    //        return false;
    //    }

    //    Vec2 p = obj as Vec2;
    //    if ((System.Object)p == null)
    //    {
    //        return false;
    //    }

    //    return (X == p.X) && (Y == p.Y);
    //}

    //public bool Equals(Vec2 p)
    //{
    //    if ((object)p == null)
    //    {
    //        return false;
    //    }

    //    return (X == p.X) && (Y == p.Y);
    //}

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + X.GetHashCode();
        hash = hash * 23 + Y.GetHashCode();
        return hash;
    }
}

public interface ITestPlayer
{
   Vec2 ChunkOffsets { get; set; }
}

public class ChunkMover : MonoBehaviour, ITestPlayer
{
    public float DebugViewRange = 16f; // range per suunta 8f
    private TileMap tilemap;
    public Vec2 ChunkOffsets { get; set; }
    private int _viewRange = 8;

      

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(DebugViewRange, DebugViewRange, DebugViewRange));
    }

    void Start()
    {
        tilemap = FindObjectOfType<TileMap>();
        ChunkOffsets = TileMap.GetChunkOffset (transform.position.x, transform.position.y);
    }

    void Update ()
    {
        tilemap.UpdateTilemap(this, _viewRange);
    }
}
