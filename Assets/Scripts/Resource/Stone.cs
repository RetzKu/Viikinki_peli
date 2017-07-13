using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Resource
{
    //[System.Serializable]
    //public static Sprite[] Sprites;
    public override void Init(bool destroyed)
    {
        
    }

    public override void OnDead()
    {
        // TODO: ERIKOISTA
        ObjectPool.instance.PoolObject(this.gameObject);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hit(25);
        }
    }
}
