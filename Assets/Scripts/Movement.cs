using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;



public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
        private Vector2 movement;
            public float slowdown = 10;
            public float thrust = 15;
            public float max_spd = 3;

    public bool Keyboard = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 SpeedLimiter = SpeedLimitChecker();
        rb.velocity += new Vector2(SpeedLimiter.x, SpeedLimiter.y);
    }

    Vector2 Input_checker()
    {
        Vector2 movement = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical")).normalized;

        if (movement.x == 0 && movement.y == 0) {rb.drag = slowdown;}
        else {rb.drag = 2;}

        return movement;
    }
    Vector2 SpeedLimitChecker()
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
