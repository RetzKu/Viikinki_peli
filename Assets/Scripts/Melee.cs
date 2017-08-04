using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : weaponStats
{
    
    private void FixedUpdate()
    {
        if(duration <= 0)
        {
            GameObject.Find("Player").GetComponent<PlayerScript>().BreakWeapon();
        }
    }

    public override void useDuration()
    {
        duration--;
    }

    /*void OnTriggerEnter2D(Collider2D Trigger)
    {
        if (Trigger.gameObject.tag == "Enemy")
        {
            onRange = true;
        }

    }

    void OnTriggerStay2D(Collider2D Trigger)
    {
        if (Trigger.gameObject.tag == "Enemy")
        {
            onRange = true;
        }
        
   } 
    void OnTriggerExit2D(Collider2D Trigger)
    {
        onRange = false;
    }*/
    public void Reposition(Transform Hand)
    {
         
        GameObject Weapon = transform.gameObject;
        
        switch (Hand.transform.name)
        {
            case "s_l_hand":
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, -90);
                    Weapon.transform.position = Hand.position;
                    Weapon.transform.localRotation = rotation;
                    Weapon.GetComponent<SpriteRenderer>().sortingOrder = 20;
                    break;
                }
            case "u_l_hand":
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, 32.8f);
                    Weapon.transform.SetParent(Hand);
                    Weapon.transform.position = Hand.position;
                    Weapon.transform.localRotation = rotation;
                    Weapon.GetComponent<SpriteRenderer>().sortingOrder = 8;
                    break;
                }
            case "d_r_hand":
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, 103.594f);
                    Weapon.transform.position = Hand.position;
                    Weapon.transform.localRotation = rotation;
                    Weapon.GetComponent<SpriteRenderer>().sortingOrder = 16;
                    break;
                }
        }
    }
}
