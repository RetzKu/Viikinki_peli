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
        //private Vector2 movement;
            public float slowdown = 10;
            public float thrust = 15;
            private float max_spd = 3;
            private float min_spd_pate = 0.2f;
            private float max_spd_pate = 3;
            bool inAttack = false;
    bool knockBack = false;
    public bool Keyboard = true;
    public CustomJoystick Joystick;
    public bool lerpUp = true;
    float currentlerpate = 0f;
    float lerpateTime = 0.5f;
    float attackTime = 0.6f;
    float attackTimer = 0f;
    float knockTime = 0.2f;
    float knockTimer = 0f;
    float steerForce = 1f;
    Vector2 knockDir = new Vector2(0f, 0f);
    Vector2 vel = new Vector2(0f, 0f);
    Vector2 acc = new Vector2(0f, 0f);
    // 2 max drag
    // 0 min drag

    void applyForce(Vector2 force)
    {
        acc += force;
    }
    void updateMovement()
    {
        acc *= Time.deltaTime;
        vel += acc;
        if(vel.magnitude > max_spd_pate)
        {
            vel.Normalize();
            vel *= max_spd_pate;
            //print("limiting speed");
        }
        //vel*=Time.deltaTime;
        rb.MovePosition(rb.position + vel);
        acc *= 0;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!knockBack)
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
        else
        {
            knockClock();
        }

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
        vel = added_spd;
        return added_spd;

    }
    public void KnockBack(Vector2 dir)//takes enemyposition
    {
        if (!knockBack)
        {
            dir -= rb.position;
            knockBack = true;
            dir.Normalize();
            dir *= 10f;
            knockDir = /*rb.position +*/ dir; // korjaa maybebebe
            GetComponent<AnimatorScript>()._Lock = true;
        }
    }
    void knockClock()
    {
        knockTimer += Time.deltaTime;
        if(knockTimer < knockTime)
        {
            //Vector2 des = knockDir - rb.position;
            //des.Normalize();
            //des *= max_spd_pate;
            //Vector2 steer = des - vel;
            //if(steer.magnitude > steerForce)
            //{
            //    steer.Normalize();
            //    steer *= steerForce;
            //}

            //applyForce(steer);
            //updateMovement();
            float t = knockTimer / knockTime;
            float spd = lerp(max_spd_pate * 0.1f, 0, t);
            knockDir.Normalize();
            knockDir *= spd;
            rb.MovePosition(rb.position + knockDir);
        }
        else
        {
            GetComponent<AnimatorScript>()._Lock = false;
            knockBack = false;
            knockTimer = 0f;
            rb.drag = slowdown; // Tarkista 
        }
    }
    float lerp(float min,float max,float t)
    {
        return  min + ((max - min) * t);
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

        max_spd = lerp(min_spd_pate, max_spd_pate,t);

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
