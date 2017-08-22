using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeEnemyStats : enemyStats {

    float startTime;

    void Update()
    {
        float times = (Time.time - startTime) / GetComponent<generalAi>().knockTime * 1000;
        foreach (SpriteRenderer t in GetComponentsInChildren<SpriteRenderer>()) { t.color = new Color(Mathf.SmoothStep(0, 255, times), 255f, 255f, Mathf.SmoothStep(0, 1, times)); }
    }

    public override void takeDamage(float rawDamageTaken)
    {
        startTime = Time.time;
        hp = CalculateArmor((int)hp, (int)armor, (int)rawDamageTaken);
        GetComponent<generalAi>().KnockBack();
        foreach (SpriteRenderer t in GetComponentsInChildren<SpriteRenderer>()) { t.color = new Color(255f, 0f, 0f, 255f); }
        AudioManager.instance.Play("GeneralHit");
        checkAlive();
    }

    // Katotaan onko vihu elossa
    public void checkAlive()
    {
        if (hp <= 0)
        {
            GetComponent<generalAi>().killMePls();
        }
    }

}
