using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//pate
public class EnemyMovement : MonoBehaviour
{
    [HideInInspector]
    public int spawnX;
    [HideInInspector]
    public int spawnY;

    private Rigidbody2D body;
    public bool seeking = true;
    public bool arriving = true;
    public bool click = false;
    public bool wander = true;
 
    public float max_speedr = 0.02f;
    public float max_steering_force = 0.001f; // higher = better steering
    public float arrive_radius = 0.3f;      // slowdown beginning

    //ai
    public float idle_radius = 60.0f;
    public float idle_ball_distance = 100.0f;
    public int idle_refreh_rate = 100;
    private int counter;
    private bool uptade_wand_srt_dir = true;

    public float desiredseparation = 0.3f;
    public float alingmentDistance = 1.0f;

    public float sepF = 1.5f;
    public float aliF = 1.0f;
    public float cohF = 1.0f;



    Vector2 velocity = new Vector2(); //An object’s PVector velocity will remain constant if it is in a state of equilibrium.
    Vector2 acceleration = new Vector2(); //summs upp by all forces 
    Vector2 target = new Vector2();

    public void InitStart(int x,int y)
    {
        spawnX = x;
        spawnY = y;
        body = GetComponent<Rigidbody2D>();
        //print(body.GetInstanceID());
        body.MovePosition(new Vector2((float)spawnX, (float)spawnY));
        //target = new Vector2((float)spawnX, (float)spawnY);
        velocity.x = Random.Range(-10f,10f);
        velocity.y = Random.Range(-10f, 10f);
    }

    //maps dictanses
    static public float map(float value, float istart, float istop,
                              float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }



    public void applyBehavior(List<GameObject> Mobs)
    {
        Vector2 sepaV = separate(Mobs);
        Vector2 ali = alingment(Mobs);
        Vector2 coh = cohesion(Mobs);
        //Vector2 mousePos = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        //Vector2 seekV = seek(mousePos);
        //flock(Mobs);
        sepaV *= sepF;
        ali *= aliF;
        coh *= cohF;

        applyForce(sepaV);
        applyForce(ali);
        applyForce(coh);

        //applyForce(seekV);


    }

    public void MovementUpdate()
    {
        velocity += acceleration;
        // limit max speed

        if (velocity.magnitude > max_speedr)
        {
            velocity.Normalize();
            velocity *= max_speedr;
        }
        //print("blaablaa");
        //print(target.x);
        //print(target.y);

        // print("uptading speed");
        body.MovePosition(body.position + velocity);
        acceleration *= 0;
        //float max_speed = max_speedr;
        //if (click)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        target.x = pz.x;
        //        target.y = pz.y;

        //    }
        //}
        //else if (wander)
        //{
        //    if (uptade_wand_srt_dir)
        //    {
        //        //print("GOT HERE");
        //        int x = Random.Range(-10, 10);
        //        int y = Random.Range(-10, 10);
        //        //print(x);
        //       // print(y);
        //        Vector2 rnd_vec = new Vector2(x,y);
        //        rnd_vec *= idle_ball_distance;
        //        Vector2 small_ball = new Vector2();
        //        small_ball = rnd_vec + body.position;
        //        target = body.position + small_ball;
        //        uptade_wand_srt_dir = false;
        //    }
        //    wander_SCRPT();
        //}
        //if (arriving)
        //{
        //    max_speed = arriving_SCRPT(max_speed);
        //}
        //Vector2 desired_vel = (target - body.position); // targetVec
        //desired_vel.Normalize();
        //desired_vel *= max_speed;

        //if (seeking)
        //{
        //    Vector2 steer = steering_SCRPT(desired_vel);
        //    applyForce(steer);
        //}
        //else
        //{
        //    applyForce(desired_vel);
        //}

    }


    float arriving_SCRPT(float max_speed) // slowsdown at endpoint
    {
        Vector2 dist = (target - body.position);
        if (dist.magnitude < arrive_radius)
        {
            if (dist.magnitude < 0.01f)
            {
                max_speed = 0;
            }
            else
            {
                float speed = map(dist.magnitude, 0f, arrive_radius, 0f, 1f);
                max_speed *= speed;
            }
        }
        return max_speed;
    }


    //Vector2 steering_SCRPT(Vector2 desired_vel) // smooth steering
    //{
    //    Vector2 steer = desired_vel - velocity;
    //    //limit steering force
    //    if (steer.magnitude > max_steering_force)
    //    {
    //        steer.Normalize();
    //        steer *= max_steering_force;
    //    }
    //    return steer;
    //}

    Vector2 seek(Vector2 target) // seeks the target
    {
        Vector2 desiredV = new Vector2(0, 0);
        desiredV = target - body.position;
        desiredV.Normalize();
        desiredV = desiredV * max_speedr;
        Vector2 steer = new Vector2(0, 0);
        steer = desiredV - velocity;
        if(steer.magnitude > max_speedr)
        {
            steer.Normalize();
            steer = steer * max_steering_force;
        }
        return steer;
    }




    void wander_SCRPT() /// gets random targets
    {
        if(counter > idle_refreh_rate)
        {
            //print("uptading direction");
            counter = 0;
            float angle = Random.Range(0.0f,1.0f) * Mathf.PI * 2;
            //print(angle);
            float x = Mathf.Cos(angle) * idle_radius;
            float y = Mathf.Sin(angle) * idle_radius;
           // print(x);
           // print(y);

            Vector2 arm = new Vector2(x + target.x, y + target.y);
            Vector2 desired = new Vector2();
            desired = arm - body.position;
            desired.Normalize();
            desired *= idle_ball_distance;

            //temp = target - body.position;
            target = body.position + desired;
        }
        else
        {
            counter++;
        }
        //go = Instantiate(prefab)
        //patearray.add(AvatarIKGoal);
    }

    void applyForce(Vector2 force)
    {
        acceleration += force;
    }

    void flock(List<GameObject> Boids)
    {
        Vector2 sep = separate(Boids);
        Vector2 ali = alingment(Boids);
        Vector2 coh = cohesion(Boids);

        sep = sep * sepF;
        ali = ali * aliF;
        coh = coh * cohF;

        applyForce(sep);
        applyForce(ali);
        applyForce(coh);

    }

    Vector2 alingment(List<GameObject> Boids)
    {
        Vector2 average = new Vector2(0, 0);
        int count = 0;
        for (int i = 0;i<Boids.Count;i++)
        {
            Vector2 temp = new Vector2(0f,0f);
            temp = body.position - Boids[i].GetComponent<EnemyMovement>().body.position;
            if ((temp.magnitude > 0) && (temp.magnitude < alingmentDistance))     // EETU TRIGGER
            {
                average = average + Boids[i].GetComponent<EnemyMovement>().velocity;
                count++;
            }
        }
        if(count > 0)
        {
            average = average / count;
            average.Normalize();
            average = average * max_speedr;

            Vector2 steer = new Vector2(0, 0);
            steer = average - velocity;

            if (steer.magnitude > max_steering_force)
            {
                steer.Normalize();
                steer *= max_steering_force;
            }

            return steer;
        }
        else
        {
            return new Vector2(0f, 0f);
        }
    }

    Vector2 cohesion(List<GameObject> Boids)
    {
        Vector2 average = new Vector2(0, 0);
        int count = 0;
        for(int i = 0;i < Boids.Count; i++)
        {
            Vector2 temp = new Vector2(0f, 0f);
            temp = body.position - Boids[i].GetComponent<EnemyMovement>().body.position;
            if ((temp.magnitude > 0) && (temp.magnitude < alingmentDistance))     // EETU TRIGGER
            {
                average = average + Boids[i].GetComponent<EnemyMovement>()./*velocity*/body.position;
                count++;
            }
        }

        if(count> 0)
        {
            average = average / count;
            return seek(average);
        }
        else
        {
            return new Vector2(0, 0);
        }
    }


    Vector2 separate(List<GameObject> mobs)
    {
        Vector2 average = new Vector2(0,0);
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
            average *= max_speedr;
            Vector2 steer = new Vector2(0, 0);
            steer = average - velocity;
            return steer;
        }
        return new Vector2(0f, 0f);
    }

}
