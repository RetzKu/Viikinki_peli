using System.Collections;
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
        transform.GetComponent<EnemyAnimator>().ChangeDirection(myDir);
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
                if (inCave)
                {
                    Physics._maxSpeed = MaxSpeed;
                }
                Vector2 playerPos = player.GetComponent<DetectEnemies>().getPosition();

                Vector2 dist = body.position - playerPos;

                meleePattern(dist, playerPos);           
            }
        }
        else
        {
            knocktimer();
        }
       // print((behavior)flags);
        powers = Physics.applyBehaviors(HeardArray, CollisionArray, new Collider2D[0], velocity, target, body.position, flags, CollState);
        target = powers[1];
        velocity = powers[0];

        //velocity *= Time.deltaTime;
        //Vector2 r = velocity * Time.deltaTime;
        //print(velocity.magnitude);
        //if (knocked)
        //{
        //    print(velocity.magnitude);
        //}

        body.MovePosition(body.position + velocity * Time.deltaTime);
    }

    void meleePattern(Vector2 dist, Vector2 playerPos)
    {
       //findPath(ref flags, ref velocity, ref target, player, body);
        LayerMask mask = new LayerMask();
        getObstacle(dist);
        bool find = obc;
        if (Physics2D.Raycast(body.position, player.transform.position, dist.magnitude, mask).collider != null)
        {
            find = true;
        }
        //attackCounter += Time.deltaTime;
        clock();// sets attack
        //if (attackCounter <= attackUptade &&!inAttack)
        if(attack && find && dist.magnitude < 0.5f)
        {
            find = false;
        }
        if (attack && dist.magnitude <= attackDist * 1.1f && !find)
        {
            //anim
            GetComponent<EnemyAnimator>().Attack();
            print("ATTTTAAAAAAAACK");
            attack = false;
        }
        //{
        if (dist.magnitude > attackDist)
        {
            //Physics._maxSteeringForce = MaxSteeringForce * 0.01f;
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
                //findPath(ref flags, ref velocity, ref target, player, body);
            }
            else
            {
                //followPlayer(ref dist, playerPos, attackDist, ref target, ref flags, Physics, sepF);

                bool success = findPath(ref flags, ref velocity, ref target, player, body);
                if (!success)
                {
                    followPlayer(ref dist, playerPos, attackDist, ref target, ref flags, Physics, sepF);
                }
            }
            //}

            //else ?
        }
        //else
        //{
        //    attackCounter = 0;
        //    inAttack = true;
        //    //dash(dist, playerPos);
        //}
    }
    //void dash(Vector2 dist, Vector2 playerPos)
    //{
    //    if(velocity.magnitude == 0 && swings < swingAmount)
    //    {
    //        flags = (int)behavior.seekAndArrive;
    //        inSwing = true;
    //        Vector2 temp = playerPos - body.position;
    //        temp.Normalize();
    //        temp *= swingDist;
    //        target = body.position + temp;
    //        Physics._maxSpeed = MaxSpeed * 1.5f;
    //        swings++;
    //    }
    //    else if (swings == swingAmount)
    //    {
    //        Physics._maxSpeed = MaxSpeed;
    //        inAttack = false;
    //        inSwing = false;
    //    }
    //    else if(!inSwing)
    //    {
    //        followPlayer(ref dist, playerPos, swingDist, ref target, ref flags, Physics, sepF);
    //    }

    //}
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
            return true;
        }
    }
    public override void resetValues()
    {
        attack = false;
        attCount = 0f;
        agro = true;
    }
}
