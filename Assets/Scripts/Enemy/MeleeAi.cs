﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAi : generalAi {

    //float attackCounter = 5f; 
    //private float attackUptade = 5f; // how often attack happens
    float attRefresh = 2f;
    float attCount = 0f;
    bool attack = true;

    protected override void InitStart(float x, float y, EnemyType type)
    {
        attackDist = swingDist;
        myType = type;
        rotation.init(myType);
        Physics.InitRules(sepF, aliF, cohF, desiredseparation, alingmentDistance, IdleRadius, IdleBallDistance, ArriveRadius, MaxSteeringForce, MaxSpeed);
        Physics._maxSpeed = MaxSpeed;
        inCave = player.GetComponent<ChunkMover>().UnderGround;

    }
    //Collider2D[] environment = new Collider2D[0];
    Collider2D[] HeardArray = new Collider2D[0];
    Collider2D[] CollisionArray = new Collider2D[0];
    public override void UpdatePosition()
    {
        rotation.UpdateRotation(velocity, body.position);
        //print(velocity.magnitude);
        if (myType == EnemyType.bear)
        {
            transform.GetComponent<BearAnimatorScript>().SpriteDirection(myDir);
        }
        else
        {
            transform.GetComponent<EnemyAnimator>().ChangeDirection(myDir);
        }
        LayerMask mask = new LayerMask();
        mask = LayerMask.GetMask("Enemy");
        getFriends(ref HeardArray, ref CollisionArray, alingmentDistance, desiredseparation, mask);
        //var HeardArray = Physics2D.OverlapCircleAll(body.position, alingmentDistance, mask); // , mask);
        //var CollisionArray = Physics2D.OverlapCircleAll(body.position, desiredseparation, mask);
        Vector2[] powers = new Vector2[2];

        if (slow)
        {
            SlowRuneTimer();
        }
        if (!knocked)
        {
            if (!agro)
            {
                if (!inCave)
                {
                    //flags = (int)behavior.wanderGroup;
                    wander(HeardArray, ref flags, ref GiveStartTarget, ref counter, IdleRefreshRate);
                    RayCollide(ref CollState, ref velocity, collideDist, body);
                    flags = flags | (int)behavior.Collide;
                    rotation.rotToPl = false;
                    rotation.Lock = false;
                }
                else
                {
                    Physics._maxSpeed = MaxSpeed * 0.2f;
                    findPath(ref flags, ref velocity, ref target, player, body);

                }
            }
            else if (agro)
            {

                Physics._maxSpeed = MaxSpeed;
                
                Vector2 playerPos = player.GetComponent<DetectEnemies>().getPosition();

                Vector2 dist = body.position - playerPos;

                meleePattern(dist, playerPos);           
            }
        }
        else
        {
            knocktimer();
        }
        powers = Physics.applyBehaviors(HeardArray, CollisionArray, new Collider2D[0], velocity, target, body.position, flags, CollState);
        target = powers[1];
        velocity = powers[0];
        body.MovePosition(body.position + velocity * Time.deltaTime);
    }

    void meleePattern(Vector2 dist, Vector2 playerPos)
    {
        LayerMask mask = new LayerMask();
        getObstacle(dist);
        bool find = obc;
        if (Physics2D.Raycast(body.position, player.transform.position, dist.magnitude, mask).collider != null)
        {
            find = true;
        }
        clock();// sets attack
        if(attack && find && dist.magnitude < 1f)
        {
            find = false;
        }
        if (attack && dist.magnitude <= attackDist * 1.1f && !find)
        {
            rotation.HardRotate(body.position, (Vector2)player.transform.position - body.position);
            //anim
            if(myType == EnemyType.bear)
            {
                GetComponent<BearAnimatorScript>().AnimationTrigger(action.Attack);
            }
            else
            {
                GetComponent<EnemyAnimator>().Attack();
            }
            print("ATTTTAAAAAAAACK");
            attack = false;
            rotation.Lock = false;
        }
        if (dist.magnitude > attackDist)
        {
            //follow player
            if (velocity.magnitude == 0)
            {
                rotation.playerPos = playerPos;
                rotation.rotToPl = true;
            }
            else
            {
                rotation.rotToPl = false;
            }

            if (!find)
            {
                followPlayer(ref dist, playerPos, attackDist, ref target, ref flags, Physics, sepF);
            }
            else
            {
                bool success = findPath(ref flags, ref velocity, ref target, player, body);
                if (!success)
                {
                    followPlayer(ref dist, playerPos, attackDist, ref target, ref flags, Physics, sepF);
                }
            }

        }
        else
        {
            followPlayer(ref dist, playerPos, attackDist, ref target, ref flags, Physics, sepF);
            Physics._maxSpeed = MaxSpeed * 0.4f;
            rotation.rotToPl = true;
        }

    }

    void clock()
    {
        if(attack == false)
        {
            attCount += Time.deltaTime;
            if (attCount >= attRefresh)
            {
                attCount = 0;
                attack = true;
            }
        }
    }

    public override bool killMyself()
    {
        if (!kys)
        {
            int[] ind = player.GetComponent<UpdatePathFind>().path.calculateIndex(body.position);

            if (ind[0] < 0 || ind[0] > TileMap.TotalWidth || ind[1] < 0 || ind[1] > TileMap.TotalHeight)
            {
                return true;
            }
        return false;
        }
        else
        {
            if(myType == EnemyType.bear)
            {
                GetComponent<BearAnimatorScript>().AnimationTrigger(action.Dead);
                this.gameObject.layer = 0;
            }
            else
            {
                ParticleSpawner.instance.SpawnDyingEffect(body.position);
            }
            return true;
        }
    }
    public override void resetValues()
    {
        attack = false;
        attCount = 0f;
        agro = true;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(body.position, 0.1f);
    }
}
