using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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

    wanderGroup = separate | alingment | cohesion,
    startWanderingSolo = wander | giveWanderingTargetSolo,
    changeSoloWanderDir = wander | changeSoloDIr,
    seekAndArrive = seek | arrive
}



public class EnemyAI : MonoBehaviour
{
    [HideInInspector]
    public float spawnX;
    [HideInInspector]
    public float spawnY;


    public float MaxSpeed = 0.02f;
    public float MaxSteeringForce = 0.001f; // higher = better steering
    public float ArriveRadius = 0.3f;      // slowdown beginning

    //ai
    public float IdleRadius = 60.0f;
    public float IdleBallDistance = 100.0f;
    public int IdleRefreshRate = 100;
    private int counter = 0;
    bool GiveStartTarget = true;


    private Rigidbody2D body;

    public float desiredseparation = 0.7f;
    public float alingmentDistance = 1.0f;

    public float sepF = 0.1f;
    public float aliF = 0.2f;
    public float cohF = 0.1f;

    public Vector2 velocity = new Vector2(); //An object’s PVector velocity will remain constant if it is in a state of equilibrium.
    Vector2 target = new Vector2();
    //Vector2 acceleration = new Vector2(); //summs upp by all forces 

    int flags = 0;


    EnemyMovement Physics = new EnemyMovement();

    // Use this for initialization
    public void InitStart(float x, float y)
    {
        body = GetComponent<Rigidbody2D>();
        spawnX = x;
        spawnY = y;
        body.MovePosition(new Vector2(spawnX, spawnY));
        velocity = new Vector2(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));
        Physics.InitRules(sepF, aliF, cohF, desiredseparation, alingmentDistance, IdleRadius, IdleBallDistance, ArriveRadius, MaxSteeringForce, MaxSpeed);
    }

    // Update is called once per frame
    public void UpdatePosition(List<GameObject> Mobs)
    {
        LayerMask mask = LayerMask.GetMask("Pate");
        var HeardArray = Physics2D.OverlapCircleAll(body.position, alingmentDistance, mask); // , mask);
        var CollisionArray = Physics2D.OverlapCircleAll(body.position, desiredseparation, mask);
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
                print("changing dir");
                flags = (int)behavior.changeSoloWanderDir;
                counter = 0;
            }
            else
            {
                print("moving forward");
                counter++;
            }
        }

        velocity = Physics.applyBehaviors(HeardArray, CollisionArray, velocity, target, body.position, flags);
        //print(velocity.x);
        //print(velocity.y);
       // print("uptading");

        body.MovePosition(body.position + velocity);
    }

    public Vector2 getPosition()
    {
        return body.position;
    } 
}
