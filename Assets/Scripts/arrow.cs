using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : Projectile {

    float lerpTime = 1f;
    float currentLerpTime;

    float moveDistance = 0f;

    Vector2 start;
    Vector3 end;

    void Start()
    {
        init();
        Player = GameObject.FindGameObjectWithTag("Player");
        body.MovePosition(Player.transform.position);
        start = body.position;
        end = transform.position + transform.up * moveDistance;
    }

    //public override void init()
    //{

    //}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLerpTime = 0f;
        }
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float perc = currentLerpTime / lerpTime;

        Vector2 d = lerPate(start, end, perc);

        body.MovePosition(d);
    }
}
