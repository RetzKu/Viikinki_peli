using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolfStats : enemyStats
{

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
        if (Crittable == false)
        {
            hp = CalculateArmor((int)hp, (int)armor, (int)rawDamageTaken);
        }
        else { hp = CalculateArmor((int)hp, (int)armor, (int)rawDamageTaken * 2); }
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
            flag = true;
            // ladataan kuolevan suden prefab

            var go = GameObject.Find("luola_tuho");
            GameObject deadWolf = Resources.Load<GameObject>("ScmlAnims/wolf/entity_000");
            deadWolf.transform.parent = go.transform;
            // otetaan sille suden positio kartalla
            deadWolf.transform.position = gameObject.transform.position;
            deadWolf.transform.localScale = new Vector3(3f, 3f, 1f);
            deadWolf.transform.localScale *= GetComponent<WolfAnimatorScript>().scale;
            // jos menee vasemmalle (kuollessa)
            //if (GetComponent<generalAi>().myDir == enemyDir.LD || GetComponent<generalAi>().myDir == enemyDir.LU || GetComponent<generalAi>().myDir == enemyDir.Left)
            //{
            //    deadWolf.transform.localScale = new Vector3(3f, 3f, 1f);
            //}
            // oikealle (kuollessa)
            if (GetComponent<generalAi>().myDir == enemyDir.RD || GetComponent<generalAi>().myDir == enemyDir.RU || GetComponent<generalAi>().myDir == enemyDir.Right)
            {
                deadWolf.transform.localScale = new Vector3(-1.0f * deadWolf.transform.localScale.x, deadWolf.transform.localScale.y, deadWolf.transform.localScale.z);
            }
            // Susi kuolee aina vaakatasossa (ei kulmassa)
            deadWolf.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);

            go = Instantiate(deadWolf);
            go.transform.parent = GameObject.Find("luola_tuho").transform;
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
