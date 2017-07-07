using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : Projectile {

    float lerpTime = 0.3f;
    float currentLerpTime;
    float moveDistance = 8f;

    Vector2 start;
    Vector2 end;
    bool started = false;

    void Start()
    {
        init();
        Player = GameObject.FindGameObjectWithTag("Player");
        print(Player.transform.position);

        body.transform.position = Player.transform.position;
        start = body.transform.position;
    }



    void Update()
    {
        if (started)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                currentLerpTime = 0f;
            }
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float t = currentLerpTime / lerpTime;
            //t = t * t * t * (t * (6f * t - 15f) + 10f);
            //t = t * t * (3f - 2f * t);
            //t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
            //t *= t;


            Vector2 d = lerPate(start, end, t);
            body.MovePosition(d);
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float angle = Vector2.Angle(start, end);
                
                Vector2 temp = end - start;
                temp.Normalize();
                temp *= moveDistance;
                end = start + temp;
                //end.Normalize();
                //end *= moveDistance;
                started = true;
                //transform.LookAt((end - (Vector3)start).normalized);

                //transform.rotation = Quaternion.LookRotation(end);

                transform.up = (Vector3)end - transform.position;
            }
        }
    }
}
