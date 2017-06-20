using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//pate
public class EnemyMovement : MonoBehaviour
{
    [HideInInspector]
    public float spawnX;
    [HideInInspector]
    public float spawnY;

    private Rigidbody2D body;

    public float MaxSpeed = 0.02f;
    public float MaxSteeringForce = 0.001f; // higher = better steering
    public float ArriveRadius = 0.3f;      // slowdown beginning

    //ai
    public float IdleRadius = 60.0f;
    public float IdleBallDistance = 100.0f;
    public int IdleRefreshRate = 100;
    private int counter;
    bool GiveStartTarget = true;

    public float desiredseparation = 0.3f;
    public float alingmentDistance = 1.0f;

    public float sepF = 1.5f;
    public float aliF = 1.0f;
    public float cohF = 1.0f;



    Vector2 velocity = new Vector2(); //An object’s PVector velocity will remain constant if it is in a state of equilibrium.
    Vector2 acceleration = new Vector2(); //summs upp by all forces 
    Vector2 target = new Vector2();

    public void InitStart(float x, float y)
    {
        spawnX = x;
        spawnY = y;
        body = GetComponent<Rigidbody2D>();
        body.MovePosition(new Vector2(spawnX, spawnY));
        velocity.x = Random.Range(-10f, 10f);
        velocity.y = Random.Range(-10f, 10f);

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
        temp = body.position + velocity;
        temp.Normalize();
        temp *= IdleBallDistance;

        target = temp;
    }
    void Wander() /// gets random targets
    {
        if (counter > IdleRefreshRate)
        {
            Vector2.Angle(target, target);
            counter = 0;
            float angle = Random.Range(0.0f, 1.0f) * Mathf.PI * 2;
            float x = Mathf.Cos(angle) * IdleRadius;
            float y = Mathf.Sin(angle) * IdleRadius;

            Vector2 arm = new Vector2(x + target.x, y + target.y);
            Vector2 desired = new Vector2();
            desired = arm - body.position;
            desired.Normalize();
            desired *= IdleBallDistance;
            target = body.position + desired;
        }
        else
        {
            counter++;
        }
    }

    public void applyBehaviors(List<GameObject> Mobs)
    {
        LayerMask mask = LayerMask.GetMask("Pate");
        var array = Physics2D.OverlapCircleAll(body.position, alingmentDistance, mask); // , mask);

        if (array.Length > 1)
        {
            Vector2 sepaV = separate(Mobs);
            Vector2 ali = alingment(array);
            Vector2 coh = cohesion(array);

            sepaV *= sepF;
            ali *= aliF;
            coh *= cohF;

            applyForce(sepaV);
            applyForce(ali);
            applyForce(coh);

            GiveStartTarget = true; // used for solo wander
        }
        else
        {
            if (GiveStartTarget)
            {
                GiveWanderingTarget();
                GiveStartTarget = false;
            }
            Wander();
            Vector2 steer = seek(target);
            applyForce(steer);
        }


    }

    public void MovementUpdate()
    {
        velocity += acceleration;
        // limit max speed

        if (velocity.magnitude > MaxSpeed)
        {
            velocity.Normalize();
            velocity *= MaxSpeed;
        }

        body.MovePosition(body.position + velocity);
        acceleration *= 0;
    }


    float arriving(float max_speed) // slowsdown at endpoint
    {
        Vector2 dist = (target - body.position);
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
        desiredV = TempTarget - body.position;
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
            temp = body.position - array[i].transform.GetComponent<EnemyMovement>().body.position;
            average = average + array[i].GetComponent<EnemyMovement>().velocity;
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
            temp = body.position - array[i].transform.GetComponent<EnemyMovement>().body.position;

            average = average + array[i].GetComponent<EnemyMovement>().body.position;
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


    Vector2 separate(List<GameObject> mobs)
    {
        Vector2 average = new Vector2(0, 0);
        int count = 0;

        LayerMask mask = LayerMask.GetMask("Pate");

        //var arrayb = Physics2D.OverlapCircle(body.position, desiredseparation, mask, );
        //var joku =  Physics2D.OverlapCircleAll()
        //print(arrayb);

        var array = Physics2D.OverlapCircleAll(body.position, desiredseparation, mask); // , mask);

        //print(array.Length);


        for (int i = 0; i < array.Length; i++)
        {
            Vector2 temp = body.position - array[i].transform.GetComponent<EnemyMovement>().body.position;
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
