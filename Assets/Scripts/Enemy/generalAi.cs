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
    CollideEnv = 4096,
    wanderGroup = separate | alingment | cohesion,
    startWanderingSolo = wander | giveWanderingTargetSolo,
    changeSoloWanderDir = wander | changeSoloDIr,
    seekAndArrive = seek | arrive,
    findPath = seek | separate,
    //getInPosition = seek | arrive
}
public abstract class generalAi : MonoBehaviour
{
    protected bool obc = false;

    protected float collideDist = 2f;
    protected collision CollState = collision.none;
    public float swingDist = 1f;

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
    public float knockTime = 0.2f;
    float knockCounter = 0f;
    float knockPercent = 1f;

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
    protected bool inCave = false;

    public void followPlayer(ref Vector2 dist, Vector2 playerPos, float attackDist,ref Vector2 target,ref int flags,EnemyMovement Physics,float sepF)
    {
        //print(attackDist);
        dist.Normalize();
        dist *= attackDist;
        target = playerPos + dist;
        flags = (int)behavior.seekAndArrive | (int)behavior.separate /*| (int)behavior.CollideEnv*/;
        //Physics._sepF = sepF * 2;
    }
    float envTime = 0.1f;
    float envTimer = 0f;
    protected void getEnvironment(ref Collider2D[] environment)
    {
        envTimer += Time.deltaTime;
        if(envTimer > envTime)
        {
            LayerMask mask = LayerMask.GetMask("ObjectLayer");
            environment = Physics2D.OverlapCircleAll(body.position, 1f, mask);// muokkaa radiusta
            envTimer = 0;
        }

    }
    float t = 0f;
    float tr = 0.1f;
    protected void getFriends(ref Collider2D[] friends, ref Collider2D[] coll, float alir,float desr,LayerMask mask)
    {
        t += Time.deltaTime;
        if(t > tr)
        {
            friends = Physics2D.OverlapCircleAll(body.position, alir, mask);
            coll = Physics2D.OverlapCircleAll(body.position, desr, mask);
            t = 0f;
        }
    }
    public void findPath(ref int flags,ref Vector2 velocity,ref Vector2 target ,GameObject player,Rigidbody2D body)
    {
        PathFinder.Dir k = player.GetComponent<UpdatePathFind>().path.getTileDir(body.position);
        rotation.rotToPl = false;
        rotation.playerPos = player.transform.position;
        if (k == PathFinder.Dir.NoDir)
        {
            if (player.GetComponent<UpdatePathFind>().path.getTileDir(new Vector2 (body.position.x-1, body.position.y)) != PathFinder.Dir.NoDir)
            {
                flags = (int)behavior.findPath;
                target = player.GetComponent<UpdatePathFind>().path.getTileTrans(new Vector2(body.position.x - 1, body.position.y));
            }
            else if (player.GetComponent<UpdatePathFind>().path.getTileDir(new Vector2(body.position.x+1, body.position.y)) != PathFinder.Dir.NoDir)
            {
                flags = (int)behavior.findPath;
                target = player.GetComponent<UpdatePathFind>().path.getTileTrans(new Vector2(body.position.x + 1, body.position.y));
            }
            else if (player.GetComponent<UpdatePathFind>().path.getTileDir(new Vector2(body.position.x, body.position.y-1)) != PathFinder.Dir.NoDir)
            {
                flags = (int)behavior.findPath;
                target = player.GetComponent<UpdatePathFind>().path.getTileTrans(new Vector2(body.position.x , body.position.y-1));
            }
            else if (player.GetComponent<UpdatePathFind>().path.getTileDir(new Vector2(body.position.x, body.position.y+1)) != PathFinder.Dir.NoDir)
            {
                flags = (int)behavior.findPath;
                target = player.GetComponent<UpdatePathFind>().path.getTileTrans(new Vector2(body.position.x, body.position.y + 1));
            }
            else
            {
                flags = 0;
                velocity *= 0;
            }
        }
        else if (k == PathFinder.Dir.Right)
        {
            flags = (int)behavior.findPath;
            target = player.GetComponent<UpdatePathFind>().path.getTileTrans(new Vector2(body.position.x +1, body.position.y));
        }
        else if (k == PathFinder.Dir.Left)
        {
            flags = (int)behavior.findPath;
            target = player.GetComponent<UpdatePathFind>().path.getTileTrans(new Vector2(body.position.x -1, body.position.y));
        }
        else if (k == PathFinder.Dir.Up)
        {
            flags = (int)behavior.findPath;
            target = player.GetComponent<UpdatePathFind>().path.getTileTrans(new Vector2(body.position.x, body.position.y + 1));
        }
        else if (k == PathFinder.Dir.Down)
        {
            flags = (int)behavior.findPath;
            target = player.GetComponent<UpdatePathFind>().path.getTileTrans(new Vector2(body.position.x, body.position.y - 1));
        }
        else
        {
            flags = 0;
            velocity *= 0;
        }
    }

    public void reversedFindPath(ref int flags, ref Vector2 velocity, ref Vector2 target, GameObject player, Rigidbody2D body) // älä käytä, riks pox
    {
        int[] ind = player.GetComponent<UpdatePathFind>().path.calculateIndex(body.position);
        PathFinder.Dir k = player.GetComponent<UpdatePathFind>().path.getTileDir(body.position);
        rotation.rotToPl = true;
        rotation.playerPos = player.transform.position;
        if (k == PathFinder.Dir.NoDir)
        {
            flags = 0;
            velocity *= 0;
        }
        else if (k == PathFinder.Dir.Right)
        {
            flags = (int)behavior.findPath;
            ind[0]--;
            PathFinder.Dir temp = player.GetComponent<UpdatePathFind>().path.getTileDir(ind);
            if (temp != PathFinder.Dir.NoDir || temp != PathFinder.Dir.NoWayOut)
            {
                target = player.GetComponent<UpdatePathFind>().path.getTileTrans(new Vector2(body.position.x - 1, body.position.y));               
            }
            else
            {
                flags = 0;
                velocity *= 0;
            }
        }
        else if (k == PathFinder.Dir.Left)
        {
            flags = (int)behavior.findPath;
            ind[0]++;
            PathFinder.Dir temp = player.GetComponent<UpdatePathFind>().path.getTileDir(ind);
            if (temp != PathFinder.Dir.NoDir || temp != PathFinder.Dir.NoWayOut)
            {
                target = player.GetComponent<UpdatePathFind>().path.getTileTrans(new Vector2(body.position.x + 1, body.position.y));
            }
            else
            {
                flags = 0;
                velocity *= 0;
            }
        }
        else if (k == PathFinder.Dir.Up)
        {
            flags = (int)behavior.findPath;
            ind[1]++;
            PathFinder.Dir temp = player.GetComponent<UpdatePathFind>().path.getTileDir(ind);
            if (temp != PathFinder.Dir.NoDir || temp != PathFinder.Dir.NoWayOut)
            {
                target = player.GetComponent<UpdatePathFind>().path.getTileTrans(new Vector2(body.position.x, body.position.y-1));
            }
            else
            {
                flags = 0;
                velocity *= 0;
            }
        }
        else if (k == PathFinder.Dir.Down)
        {
            flags = (int)behavior.findPath;
            ind[1]--;
            PathFinder.Dir temp = player.GetComponent<UpdatePathFind>().path.getTileDir(ind);
            if (temp != PathFinder.Dir.NoDir || temp != PathFinder.Dir.NoWayOut)
            {
                target = player.GetComponent<UpdatePathFind>().path.getTileTrans(new Vector2(body.position.x , body.position.y+1));
            }
            else
            {
                flags = 0;
                velocity *= 0;
            }
        }
        else
        {
            flags = 0;
            velocity *= 0;
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
        LayerMask mask = LayerMask.GetMask("ObjectLayer");
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
        //print(CollState);
    }
    public Vector2 getPosition() // tulee jokaselle
    {
        return body.position;
    }
    public virtual void KnockBack(float knockPercent = 1f)
    {
        if (!knocked)
        {
            this.knockPercent = knockPercent;
            knocked = true;
            rotation.Lock = true; 
            resetValues();
            knock = body.position - player.GetComponent<DetectEnemies>().getPosition();
            knock.Normalize();
            knock *= knockDist;
            flags = (int)behavior.seek;
            Physics._maxSpeed = MaxSpeed * 5;
            Physics._maxSteeringForce = MaxSteeringForce * 100;

        }

    }
    protected void knocktimer()
    {
        knockCounter += Time.deltaTime;

        if(knockCounter < knockTime * knockPercent)
        {
            float t = knockCounter / knockTime * knockPercent;
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
            knockPercent = 1f;
            Physics._maxSteeringForce = MaxSteeringForce;
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
    public virtual void SlowRune(float time,float slowPercent,bool reset = false)
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
        if (reset && slow) // ei testattu ominaisuus
        {
            slowTimer = 0;
            slowTime = time;
            GetComponentInChildren<buffParticle>().resetTime(time);
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

    float obsTime = 0.25f;
    float obsTimer = 0f;

    protected void getObstacle(Vector2 dist)
    {
        obsTimer += Time.deltaTime;
        if(obsTimer > obsTime)
        {
            int mask = LayerMask.GetMask("ObjectLayer");
            RaycastHit2D[] ob =  Physics2D.CircleCastAll(body.position, 0.5f, player.transform.position - (Vector3)body.position, dist.magnitude, mask);
            if(ob.Length == 0)
            {
                obc = false;
            }
            else
            {
                obc = true;
            }
            obsTimer = 0f;
        }

    }
    public abstract void resetValues();
    public abstract bool killMyself();
}
