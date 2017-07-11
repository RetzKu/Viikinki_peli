using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Resource
{
    //[System.Serializable]
    //public static Sprite[] Sprites;

    public override void OnDead()
    {
        // TODO: ERIKOISTA
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hit(25);
        }
    }
}
