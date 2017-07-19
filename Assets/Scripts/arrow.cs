using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : Projectile {

    public override void UpdateMovement()
    {

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
            killMyself = true;
        }

        float t = currentLerpTime / lerpTime;
        //t = t * t * t * (t * (6f * t - 15f) + 10f); SUPER SMOOTH KAAVA
        //t = t * t * (3f - 2f * t);
        //t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
        //t *= t;


        Vector2 d = lerPate(from, where, t);
        body.MovePosition(d);

    }
}
