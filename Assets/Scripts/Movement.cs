using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Movement : MonoBehaviour
{

    public float thrust;
    public Rigidbody2D rb;
    private Vector2 movement;
    // Use this for initialization

    public int Max_spd;
    public float Slowdown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void Update()
    {
        movement = check_axis_speed();
        rb.AddForce(movement * thrust * Time.deltaTime);
        drag_check();
    }

    void FixedUpdate()
    {

    }

    Vector2 check_axis_speed()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (rb.velocity.x > Max_spd)
        {
            movement.x = 0;
        }
        if (rb.velocity.x < -Max_spd)
        {
            movement.x = 0;
        }
        if (rb.velocity.y > Max_spd)
        {
            movement.y = 0;
        }
        if (rb.velocity.y < -Max_spd)
        {
            movement.y = 0;
        }
        return movement;
    }

    void drag_check()
    {
        if(rb.velocity.x > Slowdown && rb.velocity.x < -Slowdown || rb.velocity.y > Slowdown && rb.velocity.y < -Slowdown)
        {
            rb.drag = 1;
        }
        else
        {
            if(rb.drag < 30)
            {
                rb.drag = rb.drag * 2;
            }
            else
            {
                rb.drag = 1;
            }
        }
    }
}
