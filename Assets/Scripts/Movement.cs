using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Movement : MonoBehaviour
{
    public float slowdown;
    private Rigidbody2D rb;
    private Vector2 movement;
    public float thrust;
    public float max_spd;
    private float divider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void Update()
    {
    }

    void FixedUpdate()
    {
        tmp();
        rb.velocity += new Vector2(tmp().x, tmp().y);

    }

    Vector2 Input_checker()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        //movement = movement / divider;
        //destination += movement;
        if(movement.x == 0 && movement.y == 0)
        {
            rb.drag = slowdown;
        }
        else
        {
            rb.drag = 2;
        }
        return movement;
    }
    Vector2 tmp()
    {
        Vector2 added_spd = Input_checker();

        if (added_spd.x == 0) { rb.velocity = new Vector2(rb.velocity.x / 1.1f, rb.velocity.y); }
        if (added_spd.y == 0) { rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 1.1f); }

        if (rb.velocity.x > max_spd) { added_spd.x -= ((rb.velocity.x / max_spd) - 1)* max_spd; }
        if (rb.velocity.y > max_spd) { added_spd.y -= ((rb.velocity.y / max_spd) - 1) * max_spd; }
        if (rb.velocity.x < -max_spd) { added_spd.x += ((rb.velocity.x / -max_spd) -1) *max_spd; }
        if (rb.velocity.y < -max_spd) { added_spd.y += ((rb.velocity.y / -max_spd) - 1) * max_spd; }

        return added_spd;
    }

}
