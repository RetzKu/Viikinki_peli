using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearStats : enemyStats
{

    float startTime;
    // flägi jolla katsotaan onko kuolemasusi jo spawnattu
    private bool flag = false;
    private Transform Fx;
    private void Awake()
    {
        Fx = transform.Find("FX_001").transform;
    }
    void Update()
    {

        // Suden vahingon otosta välähdys punaisena
        float times = (Time.time - startTime) / GetComponent<generalAi>().knockTime * 1000;
        foreach (SpriteRenderer t in GetComponentsInChildren<SpriteRenderer>()) { t.color = new Color(Mathf.SmoothStep(0, 255, times), 255f, 255f, Mathf.SmoothStep(0, 1, times)); }
    }

    // Susi ottaa damagea
    public override void takeDamage(float rawDamageTaken)
    {
        startTime = Time.time;
        hp = hp - (rawDamageTaken / armor);
        GetComponent<generalAi>().KnockBack();
        foreach (SpriteRenderer t in GetComponentsInChildren<SpriteRenderer>()) { t.color = new Color(255f, 0f, 0f, 255f); }
        print(gameObject + " has " + hp + " hp left.");
        checkAlive();
    }

    // Katotaan onko susi elossa
    public void checkAlive()
    {
        if (hp <= 0 && flag == false)
        {
            GetComponent<generalAi>().killMePls();
        }
    }

    // Suden hyökkäys metodi
    public IEnumerator attack()
    {
        while(Fx.gameObject.activeSelf == false)
        {
            yield return new WaitUntil(() => Fx.gameObject.activeSelf == true);
        }
       
        var Cast = Physics2D.CircleCast(Fx.transform.position, AttackArea, Vector3.zero, 0, LayerMask.GetMask("PlayerHitBox"));

        if(Cast)
        {
            Player.setHitPosition(transform.position);
            Player.takeDamage(damage);
        }
    }
}
