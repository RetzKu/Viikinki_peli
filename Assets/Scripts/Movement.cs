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
    float steerForce = 0.05f;
    float steerSPD = 0.2f;
    Vector2 knockDir = new Vector2(0f, 0f);
    Vector2 vel = new Vector2(0f, 0f);
    Vector2 acc = new Vector2(0f, 0f);
    private int LastDirection;

    public float Duration;
    public bool Started = false;
    private float startTime;
    public bool Slowed = false;
    public float ModifiedMaxSpd;
    // 2 max drag
    // 0 min drag

    void applyForce(Vector2 force)
    {
        acc += force;
    }
    void updateMovement()
    {
        //acc *= Time.deltaTime;
        vel += acc;
        //vel*=Time.deltaTime;
        if (vel.magnitude > steerSPD)
        {
            vel.Normalize();
            vel *= steerSPD;
            //print("limiting speed");
        }

        rb.MovePosition(rb.position + vel);
        acc *= 0;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ModifiedMaxSpd = max_spd;
    }

    public void AttackSlow()
    {
        if(Started)
        {
            startTime = Time.time;
            Started = false;
        }
        float t = (Time.time - startTime) / Duration;
        ModifiedMaxSpd = Mathf.SmoothStep(0.1f, max_spd, t);
        if(ModifiedMaxSpd == 3)
        {
            Slowed = false;
        }
    }

    void FixedUpdate()
    {
        if (!knockBack)
        {
            Vector2 SpeedLimiter = SpeedLimitChecker();
            vel = rb.velocity += SpeedLimiter;
        }
        else
        {
            knockClock();
        }
        if(Slowed)
        {
            AttackSlow();
        }
    }

    Vector2 Input_checker()
    {
        //Vector2 movement = new Vector2(CrossPlatformInputManager.GetAxisRaw("Horizontal"), CrossPlatformInputManager.GetAxisRaw("Vertical")).normalized; // ???
#if UNITY_EDITOR
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized; // WASD liikkuminen koneell
#elif UNITY_ANDROID
        Vector2 movement = Joystick.GetInputVector(); // Kun buildataan phonelle
#endif

        if (movement.x == 0 && movement.y == 0) { rb.drag = slowdown; }
        else { rb.drag = 2; }

        return movement;
    }

    Vector2 SpeedLimitChecker()
    {
        Vector2 added_spd = Input_checker();

        if (added_spd.x == 0) { rb.velocity = new Vector2(rb.velocity.x / 1.1f, rb.velocity.y); }
        if (added_spd.y == 0) { rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 1.1f); }

        if (rb.velocity.x > ModifiedMaxSpd) { added_spd.x -= ((rb.velocity.x / ModifiedMaxSpd) - 1) * ModifiedMaxSpd; }
        if (rb.velocity.y > ModifiedMaxSpd) { added_spd.y -= ((rb.velocity.y / ModifiedMaxSpd) - 1) * ModifiedMaxSpd; }
        if (rb.velocity.x < -ModifiedMaxSpd) { added_spd.x += ((rb.velocity.x / -ModifiedMaxSpd) - 1) * ModifiedMaxSpd; }
        if (rb.velocity.y < -ModifiedMaxSpd) { added_spd.y += ((rb.velocity.y / -ModifiedMaxSpd) - 1) * ModifiedMaxSpd; }
        //vel = added_spd;
        return added_spd;

    }
    public void KnockBack(Vector2 dir)//takes enemyposition
    {
        if (!knockBack)
        {
            dir -= rb.position;
            knockBack = true;
            dir.Normalize();
            dir *= -10f;
            knockDir = /*rb.position +*/ dir; // korjaa maybebebe
        }
    }
    void knockClock()
    {
        knockTimer += Time.deltaTime;
        if (knockTimer < knockTime)
        {
            //Vector2 des = knockDir - rb.position;
            //des.Normalize();
            //des *= steerSPD;
            //Vector2 steer = des - vel;
            //if (steer.magnitude > steerForce)
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
            knockBack = false;
            knockTimer = 0f;
            rb.drag = slowdown; // Tarkista 
        }
    }
    float lerp(float min, float max, float t)
    {
        return min + ((max - min) * t);
    }

    //void lerpate()
    //{
        //if (lerpUp)
        //{
        //    currentlerpate += Time.deltaTime;
        //    if (currentlerpate > lerpateTime)
        //    {
        //        currentlerpate = lerpateTime;
        //    }
        //}
        //else
        //{
        //    {
        //        currentlerpate -= Time.deltaTime;
        //        if (currentlerpate < 0)
        //        {
        //            currentlerpate = 0;
        //            max_spd = min_spd_pate;
        //        }
        //    }
        //}

        //float t = currentlerpate / lerpateTime;
        //ModifiedMaxSpd = lerp(min_spd_pate, max_spd_pate, t);
        
    //}
    //public void pateClock()
    //{
    //    attackTimer += Time.deltaTime;
    //    if (attackTimer > attackTime)
    //    {
    //        lerpUp = true;
    //        inAttack = false;
    //        attackTimer = 0f;
    //    }
    //    else
    //    {
    //        lerpUp = false;
    //    }
    //}
}
