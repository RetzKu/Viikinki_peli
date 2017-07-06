using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAI : generalAi
{


    float leapDist = 5.0f;

    private int attackCounter = 300;
    private int attackUptade = 300;
   
    bool bite = false;


    
    //generalAi AI = new generalAi();

    public override void InitStart(float x, float y, EnemyType type) // jokaselle
    {
        attackDist = UnityEngine.Random.Range(2f, 4f);
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

    public override void UpdatePosition(List<GameObject> Mobs) // jokaselle
    {
        rotation.UpdateRotation(velocity, body.position);
        LayerMask mask = new LayerMask();

        mask = LayerMask.GetMask("Enemy");

        var HeardArray = Physics2D.OverlapCircleAll(body.position, alingmentDistance, mask); // , mask);
        var CollisionArray = Physics2D.OverlapCircleAll(body.position, desiredseparation, mask);
        Vector2[] powers = new Vector2[2];

        if (agro)   // agro jokainen vihu lähellä
        {
            for (int i = 0; i < HeardArray.Length; i++)
            {
                HeardArray[i].GetComponent<generalAi>().agro = true; // check if eetu lies
            }
        }
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

            leapingPattern(dist, playerPos);
        }
        powers = Physics.applyBehaviors(HeardArray, CollisionArray, velocity, target, body.position, flags, CollState);
        target = powers[1];
        velocity = powers[0];
        body.MovePosition(body.position + velocity);
    }
    void leapingPattern(Vector2 dist, Vector2 playerPos) //spe
    {
        if (dist.magnitude <= attackDist || inAttack || velocity.magnitude == 0)
        {
            if (!inAttack && attackCounter > attackUptade)
            {
                rotation.rotToPl = false;
                Physics._maxSpeed = MaxSpeed * 4;
                //start leap
                //if (dist.magnitude > 1.2f)  // velocityn mukaan leap
                //{
                //    Vector2 plVec = player.GetComponent<Rigidbody2D>().velocity;
                //    playerPos += plVec * 0.5f; // muokkaa
                    
                //    dist = body.position - playerPos;
                //}



                dist.Normalize();
                dist *= 5;
                dist *= -1.0f;
                target = body.position + dist;
                flags = (int)behavior.seek;
                inAttack = true;
            }
            else if (inAttack)
            {
                rotation.Lock = true;
                //leaping
                Vector2 t = target - body.position;
                flags = (int)behavior.seekAndArrive;
                if (velocity.magnitude == 0)
                {
                    Physics._maxSpeed = MaxSpeed;
                    inAttack = false;
                    attackCounter = 0;
                    bite = false;
                    rotation.Lock = false;
                }
                else if (dist.magnitude < velocity.magnitude * 5 && !bite)// muokkaa
                {
                    target = body.position + (velocity * 4);
                    bite = true;
                }
            }
            else
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
                attackCounter++;
                followPlayer(ref dist, playerPos, attackDist, ref target, ref flags, Physics, sepF);
            }
        }
        else
        {
            //follow player
            attackCounter++;
            if (dist.magnitude <= attackDist)
            {
                rotation.rotToPl = false;
                followPlayer(ref dist, playerPos, attackDist, ref target, ref flags, Physics, sepF);
            }
            else
            {
                rotation.playerPos = playerPos;
                rotation.rotToPl = true;
                findPath(ref flags, ref velocity, ref target, player, body);
            }
            //attackCounter = attackUptade;
        }

    }
    void OnDrawGizmos() // tulee jokaselle
    {
        Gizmos.color = Color.white;

        Vector2 main = velocity;
        main.Normalize();
        main *= collideDist; // EETU TRIGGER
        Vector2 perpendicular = new Vector2(main.y, main.x * -1);
        perpendicular /= 2f;
        Vector2 first = (main + perpendicular);
        Vector2 second = (main + (perpendicular * -1));

        //Gizmos.DrawLine(body.position, body.position+main); // piirretään viiva visualisoimaan toimivuutta 
        Gizmos.DrawLine(body.position, body.position + first);
        Gizmos.DrawLine(body.position, body.position + second);
        Gizmos.DrawLine(body.position, body.position + velocity * 30);
        if (rotation.rotToPl)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.blue;
        }
        switch (rotation._myDir)
        {
            case enemyDir.Down:
                Gizmos.DrawLine(body.position, body.position + new Vector2(0, -1));
                break;
            case enemyDir.Up:
                Gizmos.DrawLine(body.position, body.position + new Vector2(0, 1));
                break;
            case enemyDir.Left:
                Gizmos.DrawLine(body.position, body.position + new Vector2(-1, 0));
                break;
            case enemyDir.Right:
                Gizmos.DrawLine(body.position, body.position + new Vector2(1, 0));
                break;
            case enemyDir.LD:
                Gizmos.DrawLine(body.position, body.position + new Vector2(-1, -1));
                break;
            case enemyDir.LU:
                Gizmos.DrawLine(body.position, body.position + new Vector2(-1, 1));
                break;
            case enemyDir.RD:
                Gizmos.DrawLine(body.position, body.position + new Vector2(1, -1));
                break;
            case enemyDir.RU:
                Gizmos.DrawLine(body.position, body.position + new Vector2(1, 1));
                break;
            case enemyDir.Still:
                Gizmos.DrawSphere(body.position, 0.3f);
                break;


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

  
}