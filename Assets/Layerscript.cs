using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layerscript : MonoBehaviour {

    
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate()
    {
		var Objects = Physics2D.CircleCastAll(transform.position, 5, Vector2.zero, 0f, LayerMask.GetMask("ObjectLayer"));
        if(Objects.Length != 0)
        {
            foreach(RaycastHit2D t in Objects)
            {
                if (t.transform.name != "Trunk")
                {

                    if (t.transform.position.y > transform.position.y)
                    {
                        SpriteRenderer Sprites = t.transform.gameObject.GetComponent<SpriteRenderer>();
                        Sprites.sortingOrder = 0;
                    }
                    else
                    {
                        SpriteRenderer Sprites = t.transform.gameObject.GetComponent<SpriteRenderer>();
                        Sprites.sortingOrder = 50;
                    } 
                }

            }
        }
    }
}
