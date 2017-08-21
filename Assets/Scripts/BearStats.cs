using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearStats : enemyStats
{

    float startTime;
    // flägi jolla katsotaan onko kuolemasusi jo spawnattu
    private bool flag = false;
    private Transform Fx;
    bool Crittable = false;

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
        if (Crittable == false)
        {
            hp = CalculateArmor((int)hp, (int)armor, (int)rawDamageTaken); 
        }
        else { hp = CalculateArmor((int)hp, (int)armor, (int)rawDamageTaken * 2); }
        GetComponent<generalAi>().KnockBack();
        foreach (SpriteRenderer t in GetComponentsInChildren<SpriteRenderer>()) { t.color = new Color(255f, 0f, 0f, 255f); }
        print(gameObject + " has " + hp + " hp left.");
        checkAlive();
        AudioManager.instance.Play("GeneralHit");
    }

    // Katotaan onko susi elossa
    public void checkAlive()
    {
        if (hp <= 0 && flag == false)
        {
            if (transform.localEulerAngles.z > 0) { transform.localEulerAngles = new Vector3(0, 0, 0); } else { transform.localEulerAngles = new Vector3(-180, 0, -180); }
            GetComponent<generalAi>().killMePls();
        }
    }

    // Suden hyökkäys metodi
    public IEnumerator attack()
    {
        while(Fx.gameObject.activeSelf == false)
        {
            Crittable = true;
            yield return new WaitUntil(() => Fx.gameObject.activeSelf == true);
        }
        Crittable = false;
       
        var Cast = Physics2D.CircleCast(Fx.transform.position, AttackArea, Vector3.zero, 0, LayerMask.GetMask("PlayerHitBox"));
        AudioManager.instance.Play("Bear");

        if (Cast)
        {
            Player.setHitPosition(transform.position);
            Player.takeDamage(damage);
        }
    }
}
