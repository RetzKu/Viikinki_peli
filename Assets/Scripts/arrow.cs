using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : Projectile {

    private combat PlayerCombat;
    private Vector2 StartPos;

    private void Awake()
    {
        PlayerCombat = PlayerScript.Player.GetComponent<combat>();
        StartPos = transform.position;
    }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.layer == LayerMask.NameToLayer("EnemyFx"))
        {
            PlayerCombat.setHitPosition(transform.position);
            PlayerCombat.takeDamage(2); 
        }
        else
        {
            collision.GetComponent<enemyStats>().takeDamage(2);
        }
        Destroy(transform.GetComponent<BoxCollider2D>());
    }
}
