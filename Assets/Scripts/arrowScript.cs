using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowScript : MonoBehaviour {

    // Nuolen damage
    public float damage;
    // damageFlag = true ei tee damagea
    private bool damageFlag = true;
    // GameObject johon nuoli osui
    private GameObject hittedObject;
    // Muuttuja jolla voidaan säätää aikaa sekunneissa ennenkun nuoli tekee damagea
    public float timeUntilDamage = 0.15f;
    private float time;

    void Awake()
    {
        // Delay ennen kuin nuoli tekee damagea
        time = Time.time + timeUntilDamage;
    }

    // Kun nuoli menee toisen hitboxin sisälle
    private void OnTriggerEnter2D(Collider2D Collision)
    {
        // Kun nuoli osuu pelajaan
        if(Collision.tag == "notPatePlayer" && damageFlag == false && time < Time.time)
        {
            hittedObject = Collision.gameObject;
            dealDamage();
            GetComponent<arrow>().kys();
        }

        // Kun nuoli osuu viholliseen
        if (Collision.tag == "Enemy" && damageFlag == false && time < Time.time)
        {
            hittedObject = Collision.gameObject;
            dealDamage();
            GetComponent<arrow>().kys();
        }
    }
    
    // Kun ampujan hitboxista poistutaan
    private void OnTriggerExit2D(Collider2D Collision)
    {
        if(Collision.tag == "Enemy" || Collision.tag == "notPatePlayer")
        {
            damageFlag = false;
        }
    }

    // Tehdään damagea
    void dealDamage()
    {
        // Onko kohde olemassa
        if (hittedObject != null)
        {
            // Onko pelaaja kohde
            if (hittedObject.tag == "notPatePlayer")
            {
                // Setataan knockbackin suunta
                GameObject.Find("Player").GetComponent<combat>().setHitPosition(GetComponent<arrow>()._from);
                // Annetaan pelaajalle damagea
                hittedObject.GetComponentInParent<combat>().takeDamage(damage);
            }
            // Vai vihollinen
            if (hittedObject.tag == "Enemy")
            {
                // Tehdään viholliseen damagea
                hittedObject.GetComponent<enemyStats>().takeDamage(damage);
            }
        }
    }
}
