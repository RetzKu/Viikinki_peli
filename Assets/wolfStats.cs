using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolfStats : enemyStats {

    float startTime;

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
        foreach (SpriteRenderer t in GetComponentsInChildren<SpriteRenderer>()) {t.color = new Color(255f, 0f, 0f, 255f); }
        
        checkAlive();
    }

    // Katotaan onko susi elossa
    public void checkAlive() 
    {
        if(hp <= 0)
        {
            GetComponent<generalAi>().killMePls();
        }
    }

    public void attack()
    {
        GameObject hitbox = GameObject.Find("head_1").gameObject;
        hitbox.layer = LayerMask.NameToLayer("notPateEnemy");
        hitbox.tag = "Enemy";
        hitbox.AddComponent<BoxCollider2D>().isTrigger = true;
        hitbox.AddComponent<wolfHeadScript>();
        DestroyObject(hitbox.GetComponent<BoxCollider2D>(), 0.4f);
    }
}
