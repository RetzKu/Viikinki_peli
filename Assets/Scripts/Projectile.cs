using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {

    public GameObject Player;
    protected Vector2 velocity = new Vector2(0f,0f);
    float range;

    protected Rigidbody2D body;

    public virtual void init()
    {
        body = GetComponent<Rigidbody2D>();
    }
    protected Vector2 lerPate(Vector2 start, Vector2 end, float smooth)
    {
        float d = lerPate(0, (start-end).magnitude, smooth);
        Vector2 temp = end - start;
        temp.Normalize();
        temp *= d;
        start += temp;

        //Instantiate(this, Quaternion.LookRotation())
            
        return start;
    }
    protected float lerPate(float start,float end,float smooth)
    {
        return start +((end - start)*smooth);
    }

}
