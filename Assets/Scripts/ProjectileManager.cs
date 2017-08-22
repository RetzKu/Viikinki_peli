using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour {

    public GameObject arrow;
    public GameObject PlayerArrow;
    List<GameObject> pro_tiles = new List<GameObject>();
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
        int ind = 0;
        while (ind < pro_tiles.Count)
        {
            if (pro_tiles[ind].GetComponent<Projectile>().KillMyself)
            {
                Destroy(pro_tiles[ind]);
                pro_tiles.Remove(pro_tiles[ind]);
            }
            else
            {
                pro_tiles[ind].GetComponent<Projectile>().UpdateMovement();
                ind++;
            }
        }
    }
    public void spawnProjectile(Vector2 from,Vector2 where)
    {
        var go = Instantiate(arrow,new Vector2(0,0), Quaternion.identity);
        go.GetComponent<Projectile>().init(from,where);
        pro_tiles.Add(go);
    }
    public void SpawnPlayerProjectile(Vector2 from, Vector2 where)
    {
        var go = Instantiate(PlayerArrow, new Vector2(0, 0), Quaternion.identity);
        go.GetComponent<Projectile>().init(from, where);
        pro_tiles.Add(go);
    }
}
