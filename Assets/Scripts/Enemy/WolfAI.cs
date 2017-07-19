using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAI : generalAi
{


    public float leapDist = 1.0f;

    private float attackCounter = 5f; // EETU TRIGGER
    private float attackUptade = 5f;
    float leapSpeed = 4;
    bool bite = false;

    float currentTime = 0;
    float animTime = 0.7f;
    float leapAnim = 8f;
    float biteRange = 2f; // just bite
    float biteTime = 1f; // kuinka kauan vain purasu kestää
    float biteTimer = 0f; 
    bool justBite = false;
    float holderDist = 0;
    //generalAi AI = new generalAi();

    public override void InitStart(float x, float y, EnemyType type,GameObject player) // jokaselle
    {
        attackDist = UnityEngine.Random.Range(leapDist-1f, leapDist+1f);
        //print(attackDist);
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
    }

    public override void UpdatePosition() // jokaselle
    {
        //print(inAttack);
        if (!justBite)
        {
            rotation.UpdateRotation(velocity, body.position);
            GetComponent<WolfAnimatorScript>().SpriteDirection(myDir);
        }
        LayerMask mask = new LayerMask();

        mask = LayerMask.GetMask("Enemy");

        var HeardArray = Physics2D.OverlapCircleAll(body.position, alingmentDistance, mask); // , mask);
        var CollisionArray = Physics2D.OverlapCircleAll(body.position, desiredseparation, mask);
        Vector2[] powers = new Vector2[2];

        if (agro && HeardArray.Length > 1)   // agro jokainen vihu lähellä
        {
            //print(HeardArray[0].transform.name);
            //print(HeardArray[1].transform.name);

            for (int i = 0; i < HeardArray.Length; i++)
            {
                HeardArray[i].GetComponent<generalAi>().agro = true; // check if eetu lies
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
                wander(HeardArray, ref flags, ref GiveStartTarget, ref counter, IdleRefreshRate);
                rotation.rotToPl = false;
                rotation.Lock = false;
            }
            else if (agro)
            {
                Vector2 playerPos = player.GetComponent<DetectEnemies>().getPosition();

                Vector2 dist = body.position - playerPos;
                //if(dist.magnitude < biteRange*0.2f)
                //{
                //    player.GetComponent<Movement>().max_spd = 1;
                //}
                //else
                //{
                //    player.GetComponent<Movement>().max_spd = 3;
                //}
                leapingPattern(dist, playerPos);
            }
        }
        else
        {
            knocktimer();
        }
        powers = Physics.applyBehaviors(HeardArray, CollisionArray, velocity, target, body.position, flags, CollState);
        target = powers[1];
        velocity = powers[0];

        if (currentTime > animTime) //??
        {
            currentTime = animTime;
            Physics._maxSpeed = MaxSpeed * leapSpeed;            
        }
        velocity *= Time.deltaTime;
        //print(velocity.magnitude);
        body.MovePosition(body.position + velocity);
    }
    void leapingPattern(Vector2 dist, Vector2 playerPos) //spe
    {
        if (dist.magnitude <= attackDist || inAttack || velocity.magnitude == 0)
        {
            if (!inAttack && attackCounter > attackUptade)
            {

                if (dist.magnitude < biteRange)
                {
                    rotation.rotToPl = true;
                    justBite = true;
                    inAttack = true;
                    Physics._maxSpeed = MaxSpeed * 0.3f;
                    rotation.playerPos = playerPos;
                    rotation.HardRotate( body.position, velocity);
                    GetComponent<WolfAnimatorScript>().SpriteDirection(myDir);
                    GetComponent<WolfAnimatorScript>().AnimationTrigger(action.Attack);
                    GetComponent<WolfAnimatorScript>().AnimationState(action.Idle);

                }
                else
                {
                    GetComponent<WolfAnimatorScript>().AnimationTrigger(action.LeapStart);
                    rotation.rotToPl = true;
                    //Physics._maxSpeed = MaxSpeed * 4;
                    //start leap
                    //if (dist.magnitude > 1.2f)  // velocityn mukaan leap
                    //{
                    //    Vector2 plVec = player.GetComponent<Rigidbody2D>().velocity;
                    //    playerPos += plVec * 0.5f; // muokkaa
                        
                    //    dist = body.position - playerPos;
                    //}



                    dist.Normalize();
                    dist *= attackDist +1 ;//5
                    dist *= -1.0f;
                    target = body.position + dist;
                    flags = (int)behavior.seek;
                    inAttack = true;
                    currentTime = 0;
                    justBite = false;
                }
                //GetComponent<WolfAnimatorScript>().AnimationState(action.Moving);
            }
            else if (inAttack && !justBite)
            {
                bool go = timer();
                rotation.Lock = true;
                //leaping
                Vector2 t = target - body.position;
                flags = (int)behavior.seekAndArrive;
                if (velocity.magnitude == 0 && go)
                {
                    GetComponent<WolfAnimatorScript>().AnimationTrigger(action.LeapEnd); // mee ohi
                    Physics._maxSpeed = MaxSpeed;
                    inAttack = false;
                    attackCounter = 0;
                    bite = false;
                    rotation.Lock = false;
                }
                else if (dist.magnitude < velocity.magnitude * leapAnim && !bite && go )// muokkaa purase
                {
                    //GetComponent<WolfAnimatorScript>().AnimationTrigger(action.LeapEnd);
                    GetComponent<WolfAnimatorScript>().AnimationTrigger(action.Attack);
                    target = body.position + (velocity * 4.5f);
                    bite = true;
                }
                if( dist.magnitude < velocity.magnitude * leapAnim && go)
                {
                    GetComponent<WolfAnimatorScript>().AnimationTrigger(action.LeapEnd);
                }
            }
            else if (justBite)
            {
                biteTimer += Time.deltaTime;
                if(biteTimer >= biteTime)
                {
                    inAttack = false;
                    justBite = false;
                    rotation.rotToPl = false;
                    Physics._maxSpeed = MaxSpeed;
                    biteTimer = 0f;
                    attackCounter = 0;
                    rotation.Lock = false;
                    GetComponent<WolfAnimatorScript>().AnimationState(action.Moving);
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
                attackCounter+= Time.deltaTime;
                followPlayer(ref dist, playerPos, attackDist, ref target, ref flags, Physics, sepF);
            }
        }
        else
        {
            //follow player
            attackCounter += Time.deltaTime;
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

    bool timer() // leapin start hidastus
    {
        currentTime += Time.deltaTime;
        if (currentTime > animTime)
        {
            currentTime = animTime;
            Physics._maxSpeed = MaxSpeed * leapSpeed;
            return true;
        }
        else
        {
            Physics._maxSpeed = MaxSpeed * 0.1f;
            return false;
        }

    }

    public override void resetValues()
    {
        agro = true;
        inAttack = false;
        justBite = false;
        bite = false;
        biteTimer = 0f;
        currentTime = 0;
        attackCounter = 0f;
        if (inAttack)
        {
            GetComponent<WolfAnimatorScript>().AnimationTrigger(action.LeapEnd);
        }
        //else
        //{
        //    attackCounter = attackUptade / 1.5f;
        //}

    }
    public override void SlowRune(float time, float slowPercent)
    {
        this.slowPercent = slowPercent;
        if (!slow)
        {
            ParticleSpawner.instance.SpawSlow(this.gameObject, time);
            if (inAttack)
            {
                GetComponent<WolfAnimatorScript>().AnimationTrigger(action.LeapEnd);
                inAttack = false;
                attackCounter = 0;
                rotation.Lock = false;
            }
            MaxSpeed *= slowPercent;
            Physics._maxSpeed = MaxSpeed;
            slowTime = time;
            slow = true;
            holderDist = attackDist;
            attackDist *= slowPercent;
            print(attackDist);
        }
    }
    protected override void SlowRuneTimer()
    {
        slowTimer += Time.deltaTime;
        if (slowTimer > slowTime)
        {
            //print("freed");
            Physics._maxSpeed = MaxSpeed /= slowPercent;
            slowTimer = 0f;
            slow = false;
            attackDist = holderDist;
        }
    }
}