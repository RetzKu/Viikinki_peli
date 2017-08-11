using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAi : generalAi
{


    public float leapDist = 2.0f; // ignored for now

    private float attackCounter = 5f; // EETU TRIGGER
    private float attackUptade = 5f;
    float leapSpeed = 3;
    bool bite = false;

    float currentTime = 0;
    float animTime = 0.7f;
    //float leapAnim = 8f;
    float biteRange = 2f; // just bite
    float biteTime = 1f; // kuinka kauan vain purasu kestää
    float biteTimer = 0f;
    bool justBite = false;
    float holderDist = 0;
    //generalAi AI = new generalAi();
    LayerMask mask;
    protected override void InitStart(float x, float y, EnemyType type) // jokaselle
    {
        attackDist = leapDist;/* UnityEngine.Random.Range(leapDist-1f, leapDist+1f);*/
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
        mask = LayerMask.GetMask("Enemy");
        inCave = player.GetComponent<ChunkMover>().UnderGround;
    }
    Collider2D[] environment = new Collider2D[0];
    Collider2D[] HeardArray = new Collider2D[0];
    Collider2D[] CollisionArray = new Collider2D[0];
    public override void UpdatePosition() // jokaselle
    {
        //print(inAttack);
        if (!justBite)
        {
            rotation.UpdateRotation(velocity, body.position);
            GetComponent<BearAnimatorScript>().SpriteDirection(myDir);
        }
        getFriends(ref HeardArray, ref CollisionArray, alingmentDistance, desiredseparation, mask);
        //var HeardArray = Physics2D.OverlapCircleAll(body.position, alingmentDistance, mask); // , mask);
        //var CollisionArray = Physics2D.OverlapCircleAll(body.position, desiredseparation, mask);
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
                if (!inCave)
                {
                    wander(HeardArray, ref flags, ref GiveStartTarget, ref counter, IdleRefreshRate);
                    RayCollide(ref CollState, ref velocity, collideDist, body);
                    flags = flags | (int)behavior.Collide;
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
                getEnvironment(ref environment);
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
        powers = Physics.applyBehaviors(HeardArray, CollisionArray, environment, velocity, target, body.position, flags, CollState);
        target = powers[1];
        velocity = powers[0];

        if (currentTime > animTime) //??
        {
            currentTime = animTime;
            Physics._maxSpeed = MaxSpeed * leapSpeed;
        }
        //velocity *= Time.deltaTime;
        //print(velocity.magnitude);
        body.MovePosition(body.position + (velocity * Time.deltaTime));
    }
    void leapingPattern(Vector2 dist, Vector2 playerPos) //spe
    {
        getObstacle(dist);
        if (dist.magnitude <= attackDist || inAttack || velocity.magnitude == 0)
        {
            if (!inAttack && attackCounter > attackUptade && !obc)
            {

                if (dist.magnitude < biteRange)
                {
                    rotation.rotToPl = true;
                    justBite = true;
                    inAttack = true;
                    Physics._maxSpeed = MaxSpeed * 0.3f;

                    rotation.playerPos = playerPos;
                    rotation.HardRotate(body.position, velocity);
                    GetComponent<BearAnimatorScript>().SpriteDirection(myDir);
                    GetComponent<BearAnimatorScript>().AnimationTrigger(action.Attack);
                    GetComponent<BearAnimatorScript>().AnimationState(action.Idle);

                }
                else
                {
                    Physics._maxSteeringForce = MaxSteeringForce * 100f;

                    GetComponent<BearAnimatorScript>().AnimationTrigger(action.LeapStart);
                    rotation.rotToPl = true;
                    // ENNAKOIVA HYPPY?
                    //dist.Normalize();
                    //dist *= attackDist +1 ;//5
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
                bool go = timer(); // korjaa
                //bool go = true;
                rotation.Lock = true;
                //leaping
                // Vector2 t = target - body.position;
                flags = (int)behavior.seekAndArrive;
                if ((target - body.position).magnitude < 0.5f && go)
                {
                    GetComponent<BearAnimatorScript>().AnimationTrigger(action.LeapEnd); // mee ohi
                    Physics._maxSpeed = MaxSpeed;
                    inAttack = false;
                    attackCounter = 0;
                    bite = false;
                    rotation.Lock = false;
                    Physics._maxSteeringForce = MaxSteeringForce;
                }
                else if (dist.magnitude < 1/*velocity.magnitude * leapAnim*/ && !bite && go)// muokkaa purase
                {
                    //GetComponent<WolfAnimatorScript>().AnimationTrigger(action.LeapEnd);
                    GetComponent<BearAnimatorScript>().AnimationTrigger(action.Attack);
                    GetComponent<BearAnimatorScript>().AnimationTrigger(action.LeapEnd);
                    target = body.position + (velocity * 0.1f);
                    bite = true;
                }
                //if( dist.magnitude < velocity.magnitude /** leapAnim*/ && go)
                //{
                //    GetComponent<WolfAnimatorScript>().AnimationTrigger(action.LeapEnd);
                //}
            }
            else if (justBite)
            {
                biteTimer += Time.deltaTime;
                if (biteTimer >= biteTime)
                {
                    inAttack = false;
                    justBite = false;
                    rotation.rotToPl = false;
                    Physics._maxSpeed = MaxSpeed;
                    biteTimer = 0f;
                    attackCounter = 0;
                    rotation.Lock = false;
                    GetComponent<BearAnimatorScript>().AnimationState(action.Moving);
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
                attackCounter += Time.deltaTime;
                if (!inAttack && attackCounter > attackUptade)
                {
                    findPath(ref flags, ref velocity, ref target, player, body);
                }
                else
                {
                    followPlayer(ref dist, playerPos, attackDist, ref target, ref flags, Physics, sepF);
                    if (environment != null && environment.Length != 0)
                    {
                        flags = flags | (int)behavior.CollideEnv;
                    }
                }
                //reversedFindPath(ref flags, ref velocity, ref target, player, body);
            }
        }
        else
        {
            //follow player
            attackCounter += Time.deltaTime;
            if (dist.magnitude <= attackDist)
            {
                rotation.rotToPl = false;
                if (obc)
                {
                    findPath(ref flags, ref velocity, ref target, player, body);
                }
                else
                {
                    followPlayer(ref dist, playerPos, attackDist, ref target, ref flags, Physics, sepF);
                }
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

        Gizmos.DrawLine(body.position, body.position + main); // piirretään viiva visualisoimaan toimivuutta 
        Gizmos.DrawLine(body.position, body.position + first);
        Gizmos.DrawLine(body.position, body.position + second);
        Gizmos.DrawLine(body.position, body.position + velocity);
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
            GetComponent<BearAnimatorScript>().AnimationTrigger(action.LeapEnd);
        }
        //else
        //{
        //    attackCounter = attackUptade / 1.5f;
        //}

    }
    public override void SlowRune(float time, float slowPercent, bool reset = true)//todo
    {
        if (!slow)
        {
            this.slowPercent = slowPercent;
            ParticleSpawner.instance.SpawSlow(this.gameObject, time);
            if (inAttack)
            {
                GetComponent<BearAnimatorScript>().AnimationTrigger(action.LeapEnd);
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
        if (reset && slow)
        {
            slowTimer = 0;
            slowTime = time;
            GetComponentInChildren<buffParticle>().resetTime(time);

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