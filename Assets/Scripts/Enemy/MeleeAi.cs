using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAi : generalAi {

    float swingDist = 1f;
    float attackCounter = 5f; 
    private float attackUptade = 5f; // how often attack happens
    float attRefresh = 2f;
    float attCount = 0f;
    bool attack = true;

    public override void InitStart(float x, float y, EnemyType type)
    {
        attackDist = UnityEngine.Random.Range(swingDist - 0.2f, swingDist + 0.2f);
        myType = type;
        rotation.init(myType);
        body = GetComponent<Rigidbody2D>();
        spawnX = x;
        spawnY = y;
        body.MovePosition(new Vector2(spawnX, spawnY));
        velocity = new Vector2(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));
        Physics.InitRules(sepF, aliF, cohF, desiredseparation, alingmentDistance, IdleRadius, IdleBallDistance, ArriveRadius, MaxSteeringForce, MaxSpeed);
        Physics._maxSpeed = MaxSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public override void UpdatePosition(List<GameObject> Mobs)
    {
        rotation.UpdateRotation(velocity, body.position);
        LayerMask mask = new LayerMask();
        mask = LayerMask.GetMask("Enemy");

        var HeardArray = Physics2D.OverlapCircleAll(body.position, alingmentDistance, mask); // , mask);
        var CollisionArray = Physics2D.OverlapCircleAll(body.position, desiredseparation, mask);
        Vector2[] powers = new Vector2[2];

        powers = Physics.applyBehaviors(HeardArray, CollisionArray, velocity, target, body.position, flags, CollState);
        target = powers[1];
        velocity = powers[0];
        if (!agro)
        {
            wander(HeardArray, ref flags, ref GiveStartTarget, ref counter, IdleRefreshRate);
            rotation.rotToPl = false;
            rotation.Lock = false;
        }
        else if (agro)
        {
            Vector2 playerPos = player.GetComponent<DetectEnemies>().getPosition();

            Vector2 dist = body.position - playerPos;

            meleePattern(dist, playerPos);           
        }

        velocity *= Time.deltaTime;
        body.MovePosition(body.position + velocity);
    }

    void meleePattern(Vector2 dist, Vector2 playerPos)
    {
        //attackCounter += Time.deltaTime;
        clock();// sets attack
        //if (attackCounter <= attackUptade &&!inAttack)
        if (attack && dist.magnitude <= attackDist * 1.1f)
        {
            //anim
            print("ATTTTAAAAAAAACK");
            attack = false;
        }
        //{
            if(dist.magnitude > attackDist)
            {
                Physics._maxSteeringForce = MaxSteeringForce * 0.01f;
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
                followPlayer(ref dist, playerPos, attackDist, ref target, ref flags, Physics, sepF);
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
        int[] ind = player.GetComponent<UpdatePathFind>().path.calculateIndex(body.position);

        if (ind[0] < 0 || ind[0] > 59 || ind[1] < 0 || ind[1] > 59)
        {
            return true;
        }
        return false;
    }
    public override void resetValues()
    {
        attack = false;
        attCount = 0f;
        agro = true;
    }
}
