using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : weaponStats
{
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

        if (Hand.name == "u_l_hand") {}
        if (Hand.name == "d_r_hand") {}
        if (Hand.name == "s_l_hand") {}
    }

}
