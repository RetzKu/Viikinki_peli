using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {

    protected float moveDistance = 8f;
    protected float lerpTime = 0.5f;
    protected float currentLerpTime = 0;
    protected Rigidbody2D body;
    protected Vector2 from = new Vector2();
    public Vector2 _from {get{ return from; } }
    protected Vector2 where = new Vector2();
    public bool KillMyself { get { return killMyself; }}
    protected bool killMyself = false;


    public virtual void init(Vector2 from, Vector2 where)
    {
        body = GetComponent<Rigidbody2D>();
        this.from = from;
        transform.position = from;
        Vector2 temp =  where -from;
        temp.Normalize();
        temp *= moveDistance;
        this.where = from + temp;
        //where.Normalize();
        //this.where = body.position + (where * moveDistance);
        transform.up = (Vector3)where - transform.position;
    }
    protected Vector2 lerPate(Vector2 start, Vector2 end, float smooth)
    {
        float d = lerPate(0, (start-end).magnitude, smooth);
        Vector2 temp = end - start;
        temp.Normalize();
        temp *= d;
        start += temp;            
        return start;
    }
    protected float lerPate(float start,float end,float smooth)
    {
        return start +((end - start)*smooth);
    }
    public abstract void UpdateMovement();
    public void kys()
    {
        killMyself = true;
    }
}
