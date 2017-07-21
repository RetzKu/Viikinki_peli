using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolfStats : enemyStats {

    float startTime;
    // flägi jolla katsotaan onko kuolemasusi jo spawnattu
    private bool flag = false;

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
        print(gameObject + " has " + hp + " hp left.");
        checkAlive();
    }

    // Katotaan onko susi elossa
    public void checkAlive() 
    {
        if(hp <= 0 && flag == false)
        {
            flag = true;
            // ladataan kuolevan suden prefab
            GameObject deadWolf = Resources.Load<GameObject>("ScmlAnims/wolf/entity_000");
            // otetaan sille suden positio kartalla
            deadWolf.transform.position = gameObject.transform.position;
            // jos menee vasemmalle (kuollessa)
            if(GetComponent<generalAi>().myDir == enemyDir.LD || GetComponent<generalAi>().myDir == enemyDir.LU || GetComponent<generalAi>().myDir == enemyDir.Left)
            {
                deadWolf.transform.localScale = new Vector3(3f, 3f, 1f);
            }
            // oikealle (kuollessa)
            else
            {
                deadWolf.transform.localScale = new Vector3(-3f, 3f, 1f);
            }
            // Susi kuolee aina vaakatasossa (ei kulmassa)
            deadWolf.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
            Instantiate(deadWolf);
            // Tapetaan susi paten metodin avulla
            GetComponent<generalAi>().killMePls();
            print(gameObject.name + "died.");
            
            
        }
    }

    // Suden hyökkäys metodi
    public void attack()
    {
        // Etsitään suden pää gameobject
        if (gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject != null)
            {
            // 3x GetChild(0) tarkoittaa Wolf -> back -> neck -> head_1
            GameObject hitbox = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            hitbox.layer = LayerMask.NameToLayer("notPateEnemy");
            hitbox.tag = "Enemy";
            // Lisätään hitboxi ja scripti päälle
            hitbox.AddComponent<BoxCollider2D>().isTrigger = true;
            hitbox.AddComponent<wolfHeadScript>();
            // Tuhotaan molemmat gameobjectit ajan mukaan
            DestroyObject(hitbox.GetComponent<BoxCollider2D>(), 0.2f);
            DestroyObject(hitbox.GetComponent<wolfHeadScript>(), 0.2f);
            GameObject.Find("Player").GetComponent<combat>().setHitPosition(transform.localPosition);
            }
    }
}
