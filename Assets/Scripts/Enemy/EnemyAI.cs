﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Wolf,
    Archer
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

    wanderGroup = separate | alingment | cohesion,
    startWanderingSolo = wander | giveWanderingTargetSolo,
    changeSoloWanderDir = wander | changeSoloDIr,
    seekAndArrive = seek | arrive,
    //getInPosition = seek | arrive
}



public class EnemyAI : MonoBehaviour
{


    [HideInInspector]
    public float spawnX;
    [HideInInspector]
    public float spawnY;
    [HideInInspector]
    public bool agro = false;
    [HideInInspector]
    public bool inAttack = false;
    public float attackDist = 3.0f;
    public float leapDist = 10.0f;

    public float MaxSpeed = 0.05f;
    public float MaxSteeringForce = 0.001f; // higher = better steering
    public float ArriveRadius = 0.3f;      // slowdown beginning
    //ai
    public float IdleRadius = 60.0f;
    public float IdleBallDistance = 100.0f;
    public int IdleRefreshRate = 100;
    private int counter = 0;
    private int attackCounter = 0;
    private int attackUptade = 100;
    bool GiveStartTarget = true;


    private Rigidbody2D body;

    public float desiredseparation = 0.7f;
    public float alingmentDistance = 1.0f;

    public float sepF = 0.1f;
    public float aliF = 0.2f;
    public float cohF = 0.1f;

    public Vector2 velocity = new Vector2(); //An object’s PVector velocity will remain constant if it is in a state of equilibrium.
    Vector2 target = new Vector2();

    int flags = 0;

    public EnemyType myType;

    EnemyMovement Physics = new EnemyMovement();
    private GameObject player;
    // Use this for initialization
    public void InitStart(float x, float y,EnemyType type)
    {
        myType = type;

        switch (myType)
        {
            case EnemyType.Wolf:
                {
                    this.attackDist = UnityEngine.Random.Range(2f, 4f);
                    this.leapDist = 5.0f;

                    this.MaxSpeed = 0.04f;
                    this.MaxSteeringForce = 0.001f; // higher = better steering
                    this.ArriveRadius = 0.3f;      // slowdown beginning
                                                   //ai
                    this.IdleRadius = 60.0f;
                    this.IdleBallDistance = 100.0f;
                    this.IdleRefreshRate = 100;
                    this.attackUptade = 300;
                    this.attackCounter = this.attackUptade;

                    this.desiredseparation = 0.7f;
                    this.alingmentDistance = 1.0f;

                    this.sepF = 0.1f;
                    this.aliF = 0.2f;
                    this.cohF = 0.1f;
                }

                break;
            case EnemyType.Archer:
                {
                    this.attackDist = UnityEngine.Random.Range(4f, 6f);/*5.0f;*/
                    this.leapDist = 0f;

                    this.MaxSpeed = 0.04f;
                    this.MaxSteeringForce = 0.001f; // higher = better steering
                    this.ArriveRadius = 0.3f;      // slowdown beginning
                                                   //ai
                    this.IdleRadius = 60.0f;
                    this.IdleBallDistance = 100.0f;
                    this.IdleRefreshRate = 100;
                    this.attackUptade = 100;

                    this.desiredseparation = 0.7f;
                    this.alingmentDistance = 1.0f;

                    this.sepF = 0.1f;
                    this.aliF = 0.2f;
                    this.cohF = 0.1f;
                }
                break;

        }


        body = GetComponent<Rigidbody2D>();
        spawnX = x;
        spawnY = y;
        body.MovePosition(new Vector2(spawnX, spawnY));
        velocity = new Vector2(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));
        Physics.InitRules(sepF, aliF, cohF, desiredseparation, alingmentDistance, IdleRadius, IdleBallDistance, ArriveRadius, MaxSteeringForce, MaxSpeed);
        Physics.MaxSpeed = MaxSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void UpdatePosition(List<GameObject> Mobs)
    {
        LayerMask mask = new LayerMask();
        if (myType == EnemyType.Wolf)
        {
            mask = LayerMask.GetMask("Wolf");
        }
        else
        {
            mask = LayerMask.GetMask("Archer");
        }
        var HeardArray = Physics2D.OverlapCircleAll(body.position, alingmentDistance, mask); // , mask);
        var CollisionArray = Physics2D.OverlapCircleAll(body.position, desiredseparation, mask);
        Vector2[] powers = new Vector2[2];

        if (agro)
        {
            for (int i = 0; i < HeardArray.Length; i++)
            {
                HeardArray[i].GetComponent<EnemyAI>().agro = true;
            }
        }


        if (!agro)
        {
            wander(HeardArray);
        }
        else if (agro)
        {
            Vector2 playerPos = player.GetComponent<DetectEnemies>().getPosition();

            Vector2 dist = body.position - playerPos;
            if (myType == EnemyType.Wolf)
            {
                leapingPattern(dist,playerPos);
            }
            else if(myType == EnemyType.Archer)
            {
                archerPattern(dist, playerPos);
            }
        }
        powers = Physics.applyBehaviors(HeardArray, CollisionArray, velocity, target, body.position, flags);
        target = powers[1];
        velocity = powers[0];
        body.MovePosition(body.position + velocity);

    }
    void archerPattern(Vector2 dist, Vector2 playerPos)
    {
        followPlayer(dist, playerPos);
        desiredseparation = 4f;
        Physics.sepF = 1f;
        if (dist.magnitude >= attackDist)
        {
            Physics.MaxSpeed = 0.06f;
        }
        else
        {
            Physics.MaxSpeed = 0.02f;
        }
        Physics.MaxSteeringForce = 0.1f;
    }

    void leapingPattern(Vector2 dist,Vector2 playerPos)
    {


        if (dist.magnitude <= attackDist || inAttack || velocity.magnitude == 0)
        {
            if (!inAttack && attackCounter > attackUptade)
            {
                //start leap
                Physics.MaxSpeed = MaxSpeed * 4;
                dist.Normalize();
                dist *= 5;
                dist *= -1.0f;
                target = body.position + dist;
                flags = (int)behavior.seek;
                inAttack = true;
            }
            else if (inAttack)
            {
                //leaping
                Vector2 t = target - body.position;
                flags = (int)behavior.seekAndArrive;
                if (velocity.magnitude == 0)
                {
                    Physics.MaxSpeed = MaxSpeed;
                    inAttack = false;
                    attackCounter = 0;
                }
            }
            else
            {
                //follow player
                attackCounter++;
                followPlayer(dist, playerPos);
            }
        }
        else
        {
            //follow player
            attackCounter++;

            followPlayer(dist, playerPos);
            //attackCounter = attackUptade;
        }

    }

    void followPlayer(Vector2 dist, Vector2 playerPos)
    {
        dist.Normalize();
        dist *= attackDist;
        target = playerPos + dist;
        flags = (int)behavior.seekAndArrive | (int)behavior.separate;
        Physics.sepF = sepF * 2;
    }
    void wander(Collider2D[] HeardArray)
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

    public Vector2 getPosition()
    {
        return body.position;
    }
}
