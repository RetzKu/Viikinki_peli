using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ÄLÄ KÄYTÄ TÄTÄ TAI TULEN JA SUUTUN SINULLE
 * ÄLÄ KÄYTÄ TÄTÄ TAI TULEN JA SUUTUN SINULLE
 * ÄLÄ KÄYTÄ TÄTÄ TAI TULEN JA SUUTUN SINULLE
 * ÄLÄ KÄYTÄ TÄTÄ TAI TULEN JA SUUTUN SINULLE
 * ÄLÄ KÄYTÄ TÄTÄ TAI TULEN JA SUUTUN SINULLE t:pate
 * ÄLÄ KÄYTÄ TÄTÄ TAI TULEN JA SUUTUN SINULLE
 * ÄLÄ KÄYTÄ TÄTÄ TAI TULEN JA SUUTUN SINULLE
 * ÄLÄ KÄYTÄ TÄTÄ TAI TULEN JA SUUTUN SINULLE
*/



public class EnemyAI : MonoBehaviour
{
    //enemyDir myDir {get;set;}

    float collideDist = 1f;
    collision CollState = collision.none;
    [HideInInspector]
    public float spawnX { get; set; }
    [HideInInspector]
    public float spawnY { get; set; }
    [HideInInspector]
    public bool agro = false;
    [HideInInspector]
    public bool inAttack = false;
    public bool bite = false;
    public float attackDist { get; set; }
    public float leapDist { get; set; }

    public float MaxSpeed { get; set; }
    public float MaxSteeringForce { get; set; }    // higher = better steering
    public float ArriveRadius { get; set; }    // slowdown beginning
    //ai
    public float IdleRadius { get; set; }
    public float IdleBallDistance { get; set; }
    public int IdleRefreshRate { get; set; }
    private int counter = 0;
    private int attackCounter = 0;
    private int attackUptade = 100;
    bool GiveStartTarget = true;

    public enemyDir myDir { get { return rotation._myDir; } }

    private Rigidbody2D body;

    public float desiredseparation { get; set; }
    public float alingmentDistance { get; set; }

    public float sepF { get; set; }
    public float aliF { get; set; }
    public float cohF { get; set; }

    public Vector2 velocity = new Vector2(); //An object’s PVector velocity will remain constant if it is in a state of equilibrium.
    Vector2 target = new Vector2();

    int flags = 0;

    public EnemyType myType;

    EnemyMovement Physics = new EnemyMovement();
    private GameObject player;
    EnemyRotater rotation = new EnemyRotater();
    // Use this for initialization
    public void InitStart(float x, float y,EnemyType type) // jokaselle
    {
        //var pl = GameObject.FindGameObjectWithTag("Player");

        myType = type;
        rotation.init(myType);
        //myDir = enemyDir.Still;
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
        Physics._maxSpeed = MaxSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    
    
    public void UpdatePosition(List<GameObject> Mobs) // jokaselle
    {
        rotation.UpdateRotation(velocity,body.position);
        LayerMask mask = new LayerMask();

        mask = LayerMask.GetMask("Enemy");

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
            rotation.rotToPl = false;
            rotation.Lock = false;
        }
        else if (agro)
        {
            Vector2 playerPos = player.GetComponent<DetectEnemies>().getPosition(); // .tranform.position

            Vector2 dist = body.position - playerPos;
            if (myType == EnemyType.Wolf)
            {
                leapingPattern(dist, playerPos);
            }
            else if (myType == EnemyType.Archer)
            {
                archerPattern(dist, playerPos);
            }
        }

        powers = Physics.applyBehaviors(HeardArray, CollisionArray, velocity, target, body.position, flags,CollState);
        target = powers[1];
        velocity = powers[0];
        body.MovePosition(body.position + velocity);

    }
    void archerPattern(Vector2 dist, Vector2 playerPos) // spe
    {
        Physics._sepF = 0.25f;
        if (dist.magnitude >= attackDist)
        {
            rotation.rotToPl = true;
            rotation.playerPos = playerPos;
            Physics._desiredseparation = 0.7f;
            Physics._maxSpeed = 0.06f;
            findPath();
        }
        else
        {
            if(velocity.magnitude == 0)
            {
                rotation.playerPos = playerPos;
                rotation.rotToPl = true;
            }
            else
            {
                rotation.rotToPl = false;
            }
            Physics._desiredseparation = 1.0f;
            Physics._maxSpeed = 0.04f;
            followPlayer(dist, playerPos);
        }
        Physics._maxSteeringForce = 0.1f; //EETU TRIGGER
    }

    void leapingPattern(Vector2 dist,Vector2 playerPos) //spe
    { 
        if (dist.magnitude <= attackDist || inAttack || velocity.magnitude == 0)
        {
            if (!inAttack && attackCounter > attackUptade)
            {
                rotation.rotToPl = false;
                Physics._maxSpeed = MaxSpeed * 4;
                //start leap
                if(dist.magnitude > 0.7f)
                {
                Vector2 plVec = player.GetComponent<Rigidbody2D>().velocity;
                playerPos += plVec * 0.5f; // muokkaa

                dist = body.position - playerPos;
                }



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
                if(velocity.magnitude == 0)
                {
                    rotation.playerPos = playerPos;
                    rotation.rotToPl = true;
                }
                else
                {
                    rotation.rotToPl = false;
                }
                attackCounter++;
                followPlayer(dist, playerPos);
            }
        }
        else
        {
            //follow player
            attackCounter++;
            if(dist.magnitude <= attackDist)
            {
                rotation.rotToPl = false;
                followPlayer(dist, playerPos);
            }
            else
            {
                rotation.playerPos = playerPos;
                rotation.rotToPl = true;
                findPath();
            }
            //attackCounter = attackUptade;
        }

    }

    void followPlayer(Vector2 dist, Vector2 playerPos) // gen
    {
        dist.Normalize();
        dist *= attackDist;
        target = playerPos + dist;
        flags = (int)behavior.seekAndArrive | (int)behavior.separate;
        Physics._sepF = sepF * 2;
    }
    void findPath() //gen
    {
        PathFinder.Dir k =  player.GetComponent<UpdatePathFind>().path.getTileDir(body.position);

        if (k == PathFinder.Dir.NoDir /*|| k == BreadthFirstSearch.states.wall || k == BreadthFirstSearch.states.unVisited*/)
        {
            flags = 0;
            velocity *= 0;
        }
        else if (k == PathFinder.Dir.Right)
        {
            flags = (int)behavior.findPath;
            target = new Vector2(body.position.x + 1, body.position.y);
        }
        else if (k == PathFinder.Dir.Left)
        {
            flags = (int)behavior.findPath;
            target = new Vector2(body.position.x - 1, body.position.y);
        }
        else if (k == PathFinder.Dir.Up)
        {
            flags = (int)behavior.findPath;
            target = new Vector2(body.position.x, body.position.y + 1);
        }
        else if (k == PathFinder.Dir.Down)
        {
            flags = (int)behavior.findPath;
            target = new Vector2(body.position.x, body.position.y - 1);
        }
        else
        {
            flags = 0;
            velocity *= 0;
        }
        //minne menen
    }
    void wander(Collider2D[] HeardArray) //gen
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

    public Vector2 getPosition() // tulee jokaselle
    {
        return body.position;
    }

    void RayCollide() // gen
    {
        CollState = collision.none;
        LayerMask mask = LayerMask.GetMask("Collide");
        Vector2 main = velocity;
        main.Normalize();
        main *= collideDist; // EETU TRIGGER
        Vector2 perpendicular = new Vector2(main.y, main.x*-1);
        perpendicular /= 2f;
        Vector2 first = (main + perpendicular);
        Vector2 second = (main + (perpendicular * -1));
        if(Physics2D.Raycast(body.position,main, collideDist, mask).collider != null)
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
        Gizmos.DrawLine(body.position, body.position + velocity*30);
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
                Gizmos.DrawLine(body.position, body.position + new Vector2(0,-1));
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

    public bool killMyself() //general
    {
        int[] ind =  player.GetComponent<UpdatePathFind>().path.calculateIndex(body.position);

        if(ind[0] < 0 || ind[0] > Chunk.CHUNK_SIZE || ind[1] < 0 || ind[1] > Chunk.CHUNK_SIZE )
        {
            return true;
        }
        return false;
    }

}
