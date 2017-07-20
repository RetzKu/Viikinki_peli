using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerStats : enemyStats {

    float startTime;

    void Update()
    {
        // Archer vahingon otosta välähdys punaisena
        float times = (Time.time - startTime) / GetComponent<generalAi>().knockTime * 1000;
        foreach (SpriteRenderer t in GetComponentsInChildren<SpriteRenderer>()) { t.color = new Color(Mathf.SmoothStep(0, 255, times), 255f, 255f, Mathf.SmoothStep(0, 1, times)); }
    }

    // Archer ottaa damagea
    public override void takeDamage(float rawDamageTaken)
    {
        startTime = Time.time;
        hp = hp - (rawDamageTaken / armor);
        GetComponent<generalAi>().KnockBack();
        foreach (SpriteRenderer t in GetComponentsInChildren<SpriteRenderer>()) { t.color = new Color(255f, 0f, 0f, 255f); }

        checkAlive();
    }

    // Katotaan onko archer elossa
    public void checkAlive()
    {
        if (hp <= 0)
        {
            GetComponent<generalAi>().killMePls();
        }
    }

    // Hyökkäys metodi
    public void attack()
    {
        
    }
}

