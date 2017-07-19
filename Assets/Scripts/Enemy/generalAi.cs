using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Wolf,
    Archer
}
public enum Dir
{
    Left,
    Down,
    Up,
    Right,
    Basic
}
public enum collision
{
    Main,
    Right,
    Left,
    none
}
public enum action
{
    Attack,
    LeapStart,
    LeapEnd,
    Moving,
    Idle
}
[Flags]
public enum behavior
{
    separate = 1,
    alingment = 2,
    cohesion = 4,
    wander = 8,
    giveWanderingTargetSolo = 16,
    changeSoloDIr = 32,
    seek = 64,
    arrive = 128,
    moveToAttRange = 256,
    Inleap = 512,
    startLeap = 1024,
    Collide = 2048,
    wanderGroup = separate | alingment | cohesion,
    startWanderingSolo = wander | giveWanderingTargetSolo,
    changeSoloWanderDir = wander | changeSoloDIr,
    seekAndArrive = seek | arrive,
    findPath = seek | separate,
    //getInPosition = seek | arrive
}
public abstract class generalAi : MonoBehaviour
{
    protected float collideDist = 1f;
    protected collision CollState = collision.none;

    [HideInInspector]
    public float spawnX { get; set; }
    [HideInInspector]
    public float spawnY { get; set; }
    [HideInInspector]
    public bool inAttack = false;
    [HideInInspector]
    public bool agro = false;
    protected bool knocked = false;
    public enemyDir myDir { get { return rotation._myDir; } }
    public float attackDist;
    public float MaxSpeed = 0.04f;
    float knockDist = 3f;
    public float knockTime = 0.1f;
    float knockCounter = 0f;
    Vector2 knock = new Vector2(0, 0);
    protected bool GiveStartTarget = true;
    protected bool kys = false;
    protected int counter = 0;
    public int IdleRefreshRate = 100;
    public float IdleRadius = 60.0f;
    public float IdleBallDistance = 100.0f;
    public float MaxSteeringForce = 0.001f;    // higher = better steering
    public float ArriveRadius = 0.3f;     // slowdown beginning

    protected Rigidbody2D body;

    public float desiredseparation = 0.7f; // def
    public float alingmentDistance = 1.0f; // def
    
    public float sepF = 0.1f;                // def
    public float aliF = 0.2f;                // def
    public float cohF = 0.1f;                // def

    protected int flags = 0;

    [HideInInspector]
    public Vector2 velocity = new Vector2(); //An object’s PVector velocity will remain constant if it is in a state of equilibrium.
    protected Vector2 target = new Vector2();

    public EnemyType MyType { get { return myType; } }
    protected EnemyType myType;

    protected EnemyMovement Physics = new EnemyMovement();
    protected GameObject player;
    protected EnemyRotater rotation = new EnemyRotater();

    public abstract void UpdatePosition();
    public abstract void InitStart(float x,float y, EnemyType type,GameObject player);

    protected float slowTime;
    protected float slowTimer;
    protected float slowPercent;
    //protected float holderSpeed;
    protected bool slow = false;
    public void followPlayer(ref Vector2 dist, Vector2 playerPos, float attackDist,ref Vector2 target,ref int flags,EnemyMovement Physics,float sepF)
    {
        //print(attackDist);
        dist.Normalize();
        dist *= attackDist;
        target = playerPos + dist;
        flags = (int)behavior.seekAndArrive | (int)behavior.separate;
        Physics._sepF = sepF * 2;
    }

    public void findPath(ref int flags,ref Vector2 velocity,ref Vector2 target ,GameObject player,Rigidbody2D body)
    {
        PathFinder.Dir k = player.GetComponent<UpdatePathFind>().path.getTileDir(body.position);

        if (k == PathFinder.Dir.NoDir)
        {
            flags = 0;
            velocity *= 0;
            print("im STUCK");

        }
        else if (k == PathFinder.Dir.Right)
        {
            flags = (int)behavior.findPath;
            target = new Vector2(body.position.x + 1, body.position.y);
        }
        else if (k == PathFinder.Dir.Left)
        {
            flags = (int)behavior.findPath;
            target = new Vector2(body.position.x - 1, body.position.y);
        }
        else if (k == PathFinder.Dir.Up)
        {
            flags = (int)behavior.findPath;
            target = new Vector2(body.position.x, body.position.y + 1);
        }
        else if (k == PathFinder.Dir.Down)
        {
            flags = (int)behavior.findPath;
            target = new Vector2(body.position.x, body.position.y - 1);
        }
        else
        {
            flags = 0;
            velocity *= 0;
            print("im STUCK");
        }
    }

    public void wander(Collider2D[] HeardArray,ref int flags,ref bool GiveStartTarget,ref int counter,int IdleRefreshRate)
    {
        if (HeardArray.Length > 1)
        {
            flags = (int)behavior.wanderGroup;
            GiveStartTarget = true;
        }
        else
        {
            if (GiveStartTarget)
            {
                flags = (int)behavior.startWanderingSolo;
                counter = 0;
                GiveStartTarget = false;
            }
            if (counter > IdleRefreshRate)
            {
                flags = (int)behavior.changeSoloWanderDir;
                counter = 0;
            }
            else
            {
                flags = (int)behavior.wander;
                counter++;
            }
        }
    }
    public void RayCollide(ref collision CollState,ref Vector2 velocity,float collideDist, Rigidbody2D body)
    {
        CollState = collision.none;
        LayerMask mask = LayerMask.GetMask("Collide");
        Vector2 main = velocity;
        main.Normalize();
        main *= collideDist; // EETU TRIGGER
        Vector2 perpendicular = new Vector2(main.y, main.x * -1);
        perpendicular /= 2f;
        Vector2 first = (main + perpendicular);
        Vector2 second = (main + (perpendicular * -1));
        if (Physics2D.Raycast(body.position, main, collideDist, mask).collider != null)
        {
            CollState = collision.Main;
        }
        else if (Physics2D.Raycast(body.position, first, collideDist, mask).collider != null)
        {
            CollState = collision.Right;
        }
        else if (Physics2D.Raycast(body.position, second, collideDist, mask).collider != null)
        {
            CollState = collision.Left;
        }

    }
    public Vector2 getPosition() // tulee jokaselle
    {
        return body.position;
    }
    public virtual void KnockBack()
    {
        if (!knocked)
        {
            knocked = true;
            rotation.Lock = true; 
            resetValues();
            knock = body.position - player.GetComponent<DetectEnemies>().getPosition();
            knock.Normalize();
            knock *= knockDist;
            flags = (int)behavior.seek;
            Physics._maxSpeed = MaxSpeed * 5;
        }
        
    }
    protected void knocktimer()
    {
        knockCounter += Time.deltaTime;

        if(knockCounter < knockTime)
        {
            float t = knockCounter / knockTime;
            float k =  lerpate(MaxSpeed * 5, 0, t);
            //print(k);
            Physics._maxSpeed = k;
            target = body.position + knock;
        }
        else
        {
            knocked = false;
            Physics._maxSpeed = MaxSpeed;
            knockCounter = 0;
            rotation.Lock = false;
        }
    }
    float lerpate(float start, float end, float smooth)
    {
        return start + ((end - start) * smooth);
    }
    public void killMePls()
    {
        kys = true;
    }
    public virtual void SlowRune(float time,float slowPercent)
    {
        if (!slow)
        {          
            this.slowPercent = slowPercent;
            ParticleSpawner.instance.SpawSlow(this.gameObject, time);
            MaxSpeed *= slowPercent;
            Physics._maxSpeed = MaxSpeed;
            slowTime = time;
            slow = true;
        }
    }
    protected virtual void SlowRuneTimer()
    {
        slowTimer += Time.deltaTime;
        if(slowTimer> slowTime)
        {
            print("freed");
            Physics._maxSpeed = MaxSpeed /= slowPercent;
            slowTimer = 0f;
            slow = false;
        }
    }
    public abstract void resetValues();
    public abstract bool killMyself();
}
