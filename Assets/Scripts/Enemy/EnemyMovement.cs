using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//pate
public class EnemyMovement /*: MonoBehaviour*/
{
    [HideInInspector]
    public float spawnX;
    [HideInInspector]
    public float spawnY;

    //private Rigidbody2D body;
    [HideInInspector]
    public float MaxSpeed = 0.02f;
    [HideInInspector]
    public float MaxSteeringForce = 0.001f; // higher = better steering
    [HideInInspector]
    public float ArriveRadius = 0.3f;      // slowdown beginning

    //ai
    [HideInInspector]
    public float IdleRadius = 60.0f;
    [HideInInspector]
    public float IdleBallDistance = 100.0f;
    //[HideInInspector]
    //public int IdleRefreshRate = 100;
    //private int counter;
    //bool GiveStartTarget = true;
    [HideInInspector]
    public float desiredseparation = 0.7f;
    [HideInInspector]
    public float alingmentDistance = 1.0f;

    [HideInInspector]
    public float sepF = 0.1f;
    [HideInInspector]
    public float aliF = 0.2f;
    [HideInInspector]
    public float cohF = 0.1f;



    Vector2 velocity = new Vector2(); //An object’s PVector velocity will remain constant if it is in a state of equilibrium.
    Vector2 acceleration = new Vector2(); //summs upp by all forces 
    Vector2 target = new Vector2();
    Vector2 bodyPosition = new Vector2();

    public void InitRules(float sepF, float aliF,float cohF,float desiredseparation,float alingmentDistance,float IdleRadius,float IdleBallDistance,float ArriveRadius, float MaxSteeringForce,float MaxSpeed)
    {
        this.sepF = sepF;
        this.aliF = aliF;
        this.cohF = cohF;
        this.desiredseparation = desiredseparation;
        this.alingmentDistance = alingmentDistance;
        this.IdleRadius = IdleRadius;
        this.IdleBallDistance = IdleBallDistance;
        this.ArriveRadius = ArriveRadius;
        this.MaxSteeringForce = MaxSteeringForce;
        this.MaxSpeed = MaxSpeed;
    }

    //maps dictanses
    static public float map(float value, float istart, float istop,
                              float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }

    void GiveWanderingTarget()
    {
        Vector2 temp = new Vector2(0f, 0f);
        temp = bodyPosition + velocity;
        temp.Normalize();
        temp *= IdleBallDistance;

        target = temp;
    }
    void Wander(bool changeDir) /// gets random targets
    {
        if (changeDir)
        {
            Vector2.Angle(target, target);
            //counter = 0;
            float angle = Random.Range(0.0f, 1.0f) * Mathf.PI * 2;
            float x = Mathf.Cos(angle) * IdleRadius;
            float y = Mathf.Sin(angle) * IdleRadius;

            Vector2 arm = new Vector2(x + target.x, y + target.y);
            Vector2 desired = new Vector2();
            desired = arm - bodyPosition;
            desired.Normalize();
            desired *= IdleBallDistance;
            target = bodyPosition + desired;
            //seek(target);
        }
        //else
        //{
        //    counter++;
        //}
    }

    public Vector2 applyBehaviors(Collider2D[] GroupMobs, Collider2D[] CollisionMobs, Vector2 Rvelocity, Vector2 Rtarget, Vector2 position,int flags)
    {
        float tempSpeed = MaxSpeed;

        acceleration *= 0;
        velocity = Rvelocity;
        target = Rtarget;
        bodyPosition = position;
        //LayerMask mask = LayerMask.GetMask("Pate");
        //var array = Physics2D.OverlapCircleAll(bodyPosition, alingmentDistance, mask); // , mask);

       
        if((flags & (int)behavior.separate) == (int)behavior.separate)
        {
            Vector2 sepaV = separate(CollisionMobs);
            sepaV *= sepF;
            applyForce(sepaV);
        }
        if ((flags & (int)behavior.alingment) == (int)behavior.alingment)
        {
            Vector2 ali = alingment(GroupMobs);
            ali *= aliF;
            applyForce(ali);
        }
        if ((flags & (int)behavior.cohesion) == (int)behavior.cohesion)
        {
            Vector2 coh = cohesion(GroupMobs);
            coh *= cohF;
            applyForce(coh);
        }
        if ((flags & (int)behavior.giveWanderingTargetSolo) == (int)behavior.giveWanderingTargetSolo)
        {
            GiveWanderingTarget();
        }
        if ((flags & (int)behavior.wander) == (int)behavior.wander)
        {
            Wander(false);
            Vector2 steer = seek(target);
            applyForce(steer);
        }
        if ((flags & (int)behavior.changeSoloDIr) == (int)behavior.changeSoloDIr)
        {
            Wander(true);
            Vector2 steer = seek(target);
            applyForce(steer);
        }
        if ((flags & (int)behavior.seek) == (int)behavior.seek)
        {
            Vector2 steer = seek(target);
            applyForce(steer);
        }
        if ((flags & (int)behavior.arrive) == (int)behavior.arrive)
        {
            tempSpeed =  arriving(MaxSpeed);
        }

        velocity += acceleration;
        // limit max speed

        if (velocity.magnitude > MaxSpeed)
        {
            velocity.Normalize();
            velocity *= tempSpeed;
        }

        return velocity;
        



        //if (array.Length > 1)
        //{
        //    Vector2 sepaV = separate(Mobs);
        //    Vector2 ali = alingment(array);
        //    Vector2 coh = cohesion(array);

        //    sepaV *= sepF;
        //    ali *= aliF;
        //    coh *= cohF;

        //    applyForce(sepaV);
        //    applyForce(ali);
        //    applyForce(coh);

        //    GiveStartTarget = true; // used for solo wander
        //}
        //else
        //{
        //    if (GiveStartTarget)
        //    {
        //        GiveWanderingTarget();
        //        GiveStartTarget = false;
        //    }
        //    Wander();
        //    Vector2 steer = seek(target);
        //    applyForce(steer);
        //}


    }

    //public void MovementUpdate()
    //{
    //    velocity += acceleration;
    //    // limit max speed

    //    if (velocity.magnitude > MaxSpeed)
    //    {
    //        velocity.Normalize();
    //        velocity *= MaxSpeed;
    //    }

    //    body.MovePosition(bodyPosition + velocity);
    //    acceleration *= 0;
    //}


    float arriving(float max_speed) // slowsdown at endpoint
    {
        Vector2 dist = (target - bodyPosition);
        if (dist.magnitude < ArriveRadius)
        {
            if (dist.magnitude < 0.01f)
            {
                max_speed = 0;
            }
            else
            {
                float speed = map(dist.magnitude, 0f, ArriveRadius, 0f, 1f);
                max_speed *= speed;
            }
        }
        return max_speed;
    }


    Vector2 seek(Vector2 TempTarget) // seeks the target
    {
        Vector2 desiredV = new Vector2(0, 0);
        desiredV = TempTarget - bodyPosition;
        desiredV.Normalize();
        desiredV = desiredV * MaxSpeed;
        Vector2 steer = new Vector2(0, 0);
        steer = desiredV - velocity;
        if (steer.magnitude > MaxSpeed)
        {
            steer.Normalize();
            steer = steer * MaxSteeringForce;
        }
        return steer;
    }





    void applyForce(Vector2 force)
    {
        acceleration += force;
    }


    Vector2 alingment(Collider2D[] array)
    {
        Vector2 average = new Vector2(0, 0);
        for (int i = 0; i < array.Length; i++)
        {
            Vector2 temp = new Vector2(0f, 0f);
            temp = bodyPosition - array[i].transform.GetComponent<EnemyAI>().getPosition();
            average = average + array[i].GetComponent<EnemyAI>().velocity;
        }


        if (array.Length > 1)
        {
            average = average / array.Length;
            average.Normalize();
            average = average * MaxSpeed;

            Vector2 steer = new Vector2(0, 0);
            steer = average - velocity;

            if (steer.magnitude > MaxSteeringForce)
            {
                steer.Normalize();
                steer *= MaxSteeringForce;
            }

            return steer;
        }
        else
        {
            return new Vector2(0f, 0f);
        }
    }

    Vector2 cohesion(Collider2D[] array)
    {
        Vector2 average = new Vector2(0, 0);

        for (int i = 0; i < array.Length; i++)
        {
            Vector2 temp = new Vector2(0f, 0f);
            temp = bodyPosition - array[i].transform.GetComponent<EnemyAI>().getPosition();

            average = average + array[i].GetComponent<EnemyAI>().getPosition();
        }

        if (array.Length > 1)
        {
            average = average / array.Length;
            return seek(average);
        }
        else
        {
            return new Vector2(0, 0);
        }
    }


    Vector2 separate(Collider2D[] array)
    {
        Vector2 average = new Vector2(0, 0);
        int count = 0;

        //LayerMask mask = LayerMask.GetMask("Pate");

        //var arrayb = Physics2D.OverlapCircle(bodyPosition, desiredseparation, mask, );
        //var joku =  Physics2D.OverlapCircleAll()
        //print(arrayb);

        //var array = Physics2D.OverlapCircleAll(bodyPosition, desiredseparation, mask); // , mask);

        //print(array.Length);


        for (int i = 0; i < array.Length; i++)
        {
            Vector2 temp = bodyPosition - array[i].transform.GetComponent<EnemyAI>().getPosition();
            float d = temp.magnitude;
            //print(d);

            if (d > 0)
            {
                //print("colliding");
                temp.Normalize();
                temp = temp / d;
                average = average + temp;
                count++;
            }

        }
        if (count > 0)
        {
            average = average / count;
            average *= MaxSpeed;
            Vector2 steer = new Vector2(0, 0);
            steer = average - velocity;
            return steer;
        }
        return new Vector2(0f, 0f);
    }

}
