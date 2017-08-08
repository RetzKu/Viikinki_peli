using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//pate
public class EnemyMovement 
{
    public float _maxSpeed { set { MaxSpeed = value; } }
    float MaxSpeed;

    public float _maxSteeringForce { set { MaxSteeringForce = value; } }
    float MaxSteeringForce;              // higher = better steering

    public float _arriveRadius { set { ArriveRadius = value; } }
    float ArriveRadius;     // slowdown beginning
    //ai
    public float _idleRadius { set { IdleRadius = value; } }
    float IdleRadius;

    public float _idleBallDistance { set { IdleBallDistance = value; } }
    float IdleBallDistance;

    public float _desiredseparation { set { desiredseparation = value; } }
    float desiredseparation;

    public float _alingmentDistance { set { alingmentDistance = value; } }
    float alingmentDistance;

    public float _sepF { set { sepF = value; } }
    float sepF;

    public float _aliF { set { aliF = value; } }
    float aliF;

    public float _cohF { set { cohF = value; } }
    float cohF;


    float environmentCollisionForce = 0.04f;


    Vector2 velocity = new Vector2(); //An object’s PVector velocity will remain constant if it is in a state of equilibrium.
    Vector2 acceleration = new Vector2(); //summs upp by all forces 
    Vector2 target = new Vector2();
    Vector2 bodyPosition = new Vector2();

    public void InitRules(float sepF, float aliF, float cohF, float desiredseparation, float alingmentDistance, float IdleRadius, float IdleBallDistance, float ArriveRadius, float MaxSteeringForce, float MaxSpeed)
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
            float x = Mathf.Cos(angle) * IdleRadius;        // KATO VOITKO KORJATA // En, pitäisi kirjoittaa koko koodi uudestaan t:eetu #triggered
            float y = Mathf.Sin(angle) * IdleRadius;        // KATO VOITKO KORJATA

            Vector2 arm = new Vector2(x + target.x, y + target.y);
            Vector2 desired = new Vector2();
            desired = arm - bodyPosition;
            desired.Normalize();
            desired *= IdleBallDistance;
            target = bodyPosition + desired;
            //seek(target);
        }

    }

    public Vector2[] applyBehaviors(Collider2D[] GroupMobs, Collider2D[] CollisionMobs,Collider2D[] environment, Vector2 Rvelocity, Vector2 Rtarget, Vector2 position, int flags,collision collstate)
    {
        float tempSpeed = MaxSpeed;

        acceleration *= 0;
        velocity = Rvelocity;
        target = Rtarget;
        bodyPosition = position;


        if ((flags & (int)behavior.separate) == (int)behavior.separate)
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
        if ((flags & (int)behavior.CollideEnv) == (int)behavior.CollideEnv)
        {
            Vector2 envsep = separate(environment);
            envsep *= environmentCollisionForce;
            applyForce(envsep);
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
            tempSpeed = arriving(tempSpeed);
        }
        if((flags & (int)behavior.Collide) == (int)behavior.Collide)
        {
            if((flags & (int)behavior.wanderGroup) == (int)behavior.wanderGroup)
            {
                acceleration =  CollideSteer(collstate,acceleration,true); // EETU TRIGGER
            }
            else
            {
                acceleration = CollideSteer(collstate, acceleration,true); // EETU TRIGGER
            }
        }

        velocity += acceleration;
        // limit max speed

        if (velocity.magnitude > tempSpeed)
        {
            velocity.Normalize();
            velocity *= tempSpeed;
        }

        Vector2[] powers = new Vector2[2];
        powers[0] = velocity;
        powers[1] = target;
        return powers;

    }

    Vector2 CollideSteer(collision collstate,Vector2 acc,bool change = false )
    {
        float accMag = acc.magnitude;
        Vector2 temp = velocity;
        switch (collstate)
        {
            case collision.none:
                return acc;
            case collision.Right:
                Vector2 perpendicularR = new Vector2(temp.y, temp.x *-1);
                perpendicularR /= 3f;
                temp = (temp + (perpendicularR * -1));
                break;
            case collision.Left:
                Vector2 perpendicularL = new Vector2(temp.y, temp.x * -1);
                perpendicularL /= 3f;
                temp = (temp + perpendicularL);
                break;
            case collision.Main:
                if(Mathf.Abs(velocity.x) >= Mathf.Abs(velocity.y))
                {
                Vector2 perpendicularM = new Vector2(temp.y, temp.x * -1);
                perpendicularM *= 1.5f;
                temp = (temp + perpendicularM);
                }
                else
                {
                    Vector2 perpendicularM = new Vector2(temp.y, temp.x * -1);
                    perpendicularM *= -1.5f;
                    temp = (temp + perpendicularM);
                }
                break;

        }
        temp.Normalize();
        if (change)
        {
        acc = temp;
        }
        temp *= IdleBallDistance;
        target = bodyPosition + temp;

        if (change)
        {
            acc = temp * accMag;
        }
        return acc;
    }

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
        if (steer.magnitude > MaxSteeringForce)
        {
            steer.Normalize();
            steer = steer * MaxSteeringForce;// RIKKOO MAH KORJAA HOX HOX HOX
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
            if(array[i] != null)
            {
                Vector2 temp = new Vector2(0f, 0f);
                temp = bodyPosition - array[i].transform.GetComponent<generalAi>().getPosition();
                average = average + array[i].GetComponent<generalAi>().velocity;
            }
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
            if (array[i] != null)
            {
                Vector2 temp = new Vector2(0f, 0f);
                temp = bodyPosition - array[i].transform.GetComponent<generalAi>().getPosition();

                average = average + array[i].GetComponent<generalAi>().getPosition();
            }
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


    Vector2 separate(Collider2D[] array) // qq debug
    {
        Vector2 average = new Vector2(0, 0);
        int count = 0;
        if(array != null)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null)
                {
                        //Vector2 temp = bodyPosition - array[i].transform.GetComponent<generalAi>().getPosition();
                    Vector2 temp = bodyPosition - (Vector2)array[i].transform.position;
                    float d = temp.magnitude;

                    if (d > 0)
                    {
                        temp.Normalize();
                        temp = temp / d;
                        average = average + temp;
                        count++;
                    }
                }

            }

        }
        else
        {
            return new Vector2(0f, 0f);
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
