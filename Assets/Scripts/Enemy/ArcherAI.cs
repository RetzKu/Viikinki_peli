using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAI : generalAi {

    private float attackCounter = 0f;
    private float attackUptade = 2f;
    float chargeCounter = 0f;
    float shootTime = 1f;
    GameObject proManager;

    public override void InitStart(float x, float y, EnemyType type,GameObject player)
    {
        attackDist = UnityEngine.Random.Range(4f, 6f);
        myType = type;
        rotation.init(myType);
        body = GetComponent<Rigidbody2D>();
        spawnX = x;
        spawnY = y;
        body.MovePosition(new Vector2(spawnX, spawnY));
        velocity = new Vector2(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));
        Physics.InitRules(sepF, aliF, cohF, desiredseparation, alingmentDistance, IdleRadius, IdleBallDistance, ArriveRadius, MaxSteeringForce, MaxSpeed);
        Physics._maxSpeed = MaxSpeed;
        this.player = player;
        proManager = GameObject.FindGameObjectWithTag("projectileManager");

    }

    public override void UpdatePosition(List<GameObject> Mobs)
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
                HeardArray[i].GetComponent<generalAi>().agro = true;
            }
        }
        if (slow)
        {
            SlowRuneTimer();
        }
        if (!knocked)
        {
            if (!agro)
            {
                wander(HeardArray,ref flags,ref GiveStartTarget,ref counter,IdleRefreshRate);
                rotation.rotToPl = false;
                rotation.Lock = false;
            }
            else if (agro)
            {
                Vector2 playerPos = player.GetComponent<DetectEnemies>().getPosition();

                Vector2 dist = body.position - playerPos;
           
                archerPattern(dist, playerPos);         
            }
        }
        else
        {
            knocktimer();
        }

        powers = Physics.applyBehaviors(HeardArray, CollisionArray, velocity, target, body.position, flags, CollState);
        target = powers[1];
        velocity = powers[0];


        velocity *= Time.deltaTime;
        //print(velocity.magnitude);
        body.MovePosition(body.position + velocity);

    }
    void archerPattern(Vector2 dist, Vector2 playerPos) // spe
    {
        attackCounter += Time.deltaTime;
        if(attackCounter < attackUptade &&  !inAttack)
        {
            if (dist.magnitude >= attackDist)
            {
                rotation.rotToPl = true;
                rotation.playerPos = playerPos;
                Physics._desiredseparation = 0.7f;
                Physics._maxSpeed = MaxSpeed * 1.5f;
                findPath(ref flags,ref velocity,ref target,player,body);
            }
            else
            {
                if (velocity.magnitude == 0)
                {
                    rotation.playerPos = playerPos;
                    rotation.rotToPl = true;
                }
                else
                {
                    rotation.rotToPl = false;
                }
                Physics._desiredseparation = 1.0f;
                Physics._maxSpeed =MaxSpeed* 0.8f;
                followPlayer(ref dist, playerPos,attackDist,ref target,ref flags,Physics,sepF);
            }
        }
        else
        {
            inAttack = true;
            attackCounter = 0;
            clock(playerPos,dist);
        }
        Physics._sepF = sepF * 1.5f;
        Physics._maxSteeringForce = MaxSteeringForce * 0.1f; //EETU TRIGGER
    }
    void clock(Vector2 playerPos,Vector2 dist)
    {
        chargeCounter += Time.deltaTime;
        if(chargeCounter > shootTime)
        {

            //print("SHOOOOOOT");
            if(dist.magnitude > 1.5f)
            {
                Vector2 r =  Random.insideUnitCircle * Random.Range(0f, 2.5f) + playerPos;
                proManager.GetComponent<ProjectileManager>().spawnProjectile(body.position, r);
            }
            else
            {
                proManager.GetComponent<ProjectileManager>().spawnProjectile(body.position, playerPos);
            }
            chargeCounter = 0;
            inAttack = false;
            rotation.Lock = false;
            Physics._maxSpeed = MaxSpeed;
        }
        else
        {
            rotation.playerPos = playerPos;
            rotation.HardRotate(body.position, velocity);
            Physics._maxSpeed = MaxSpeed * 0.1f;
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
            //case enemyDir.Still:
            //    Gizmos.DrawSphere(body.position, 0.3f);
            //    break;


        }
    }

    public override bool killMyself()
    {
        if (!kys)
        {
            int[] ind = player.GetComponent<UpdatePathFind>().path.calculateIndex(body.position);

            if (ind[0] < 0 || ind[0] > 59 || ind[1] < 0 || ind[1] > 59)
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
        attackCounter = 0f;
        chargeCounter = 0f;
        agro = true;
        inAttack = false;
    }
}
