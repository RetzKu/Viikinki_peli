using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public enum PlayerDir
{
    right = 3,
    left = 0,
    up = 2,
    down = 1,
    def = 5
}

public class Movement : MonoBehaviour
{
    PlayerDir pd = PlayerDir.def;
    PlayerDir fx = PlayerDir.def;
    private Rigidbody2D rb;
        private Vector2 movement;
            public float slowdown = 10;
            public float thrust = 15;
            public float max_spd = 3;
            public float min_spd_pate = 0.2f;
            public float max_spd_pate = 3;
            bool inAttack = false;
    public bool Keyboard = true;
    public CustomJoystick Joystick;
    public bool lerpUp = true;
    float currentlerpate = 0f;
    float lerpateTime = 0.5f;
    float attackTime = 0.6f;
    float attackTimer = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (inAttack)
        {
            pateClock();
        }
        lerpate();
        //print(max_spd);
        Vector2 SpeedLimiter = SpeedLimitChecker();
        rb.velocity += new Vector2(SpeedLimiter.x, SpeedLimiter.y);
    }

    Vector2 Input_checker()
    {
        //Vector2 movement = new Vector2(CrossPlatformInputManager.GetAxisRaw("Horizontal"), CrossPlatformInputManager.GetAxisRaw("Vertical")).normalized; // ???
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized; // WASD liikkuminen koneell
        //Vector2 movement = Joystick.GetInputVector(); // Kun buildataan phonelle

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
        getRotation(added_spd);
        return added_spd;

    }
    void lerpate()
    {
        if (lerpUp)
        {
            currentlerpate += Time.deltaTime;
            if (currentlerpate > lerpateTime)
            {
                currentlerpate = lerpateTime;
            }
        }
        else
        {
            {
                currentlerpate -= Time.deltaTime;
                if (currentlerpate < 0)
                {
                    currentlerpate = 0;
                    max_spd = min_spd_pate;
                }
            }
        }
        float t = currentlerpate / lerpateTime;

        max_spd = min_spd_pate + ((max_spd_pate - min_spd_pate) * t);

    }
    public void UpPateDir(PlayerDir dir)
    {
        fx = dir;
        if (dir == pd)
        {
            return;
        }
        else
        {
            inAttack = true;
            GetComponent<AnimatorScript>().Sprites.EnableSprites((int)dir);
            GetComponent<AnimatorScript>()._Lock = true;
        }
    }
    void pateClock()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackTime)
        {
            lerpUp = true;
            inAttack = false;
            attackTimer = 0f;
            GetComponent<AnimatorScript>()._Lock = false;
        }
        else
        {
            lerpUp = false;
        }
    }
    void getRotation(Vector2 velocity)
    {
        PlayerDir temp = PlayerDir.def;
        if (Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.y))
        {
            if (velocity.x < 0)
            {
                temp = PlayerDir.left;
            }
            else
            {
                temp = PlayerDir.right;
            }
        }
        else if (Mathf.Abs(velocity.y) >= Mathf.Abs(velocity.x))
        {
            if (velocity.y < 0)
            {
                temp = PlayerDir.down;
            }
            else
            {
                temp = PlayerDir.up;
            }

        }
        pd = temp;
    }
}
