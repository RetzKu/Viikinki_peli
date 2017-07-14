using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolfHeadScript : MonoBehaviour {

    bool damageFlag = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "notPatePlayer" && !damageFlag)
        {
            dealDamage();
            damageFlag = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "notPatePlayer")
        {
            damageFlag = false;
        }
    }

    void dealDamage()
    {
        if (GameObject.Find("EnemySpawner") != null) // Poista  if "optimointivaiheessa"
        {
            if (GameObject.Find("EnemySpawner").GetComponent<MobsControl>().enemiesDealDamage) // Poista  if "optimointivaiheessa"
            {
                float temp = GetComponentInParent<enemyStats>().damage;
                GameObject.Find("Player").GetComponent<combat>().takeDamage(GetComponentInParent<enemyStats>().damage);
                Debug.Log("Wolf dealed " + GetComponentInParent<enemyStats>().damage + "damage, player has " + GameObject.Find("Player").GetComponent<combat>().hp + " hp left!");
            }
            else // Poista else "optimointivaiheessa"
            {
                Debug.Log("Toggle bool enemiesDealDamage if you want enemies to deal damage");
            }
        }
        else // Poista else "optimointivaiheessa"
        {
            Debug.LogError("Put EnemySpawner on your scene!");
        }
    }
}
