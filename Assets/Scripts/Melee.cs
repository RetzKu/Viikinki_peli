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

    public override float CalculateDuration()
    {
        float DurationPrecent = duration / (float)MaxDuration;
        return DurationPrecent;
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
        if (Weapon.transform.localScale.x < 0) { Vector3 Scale = Weapon.transform.localScale; Scale.x *= -1; Weapon.transform.localScale = Scale; }

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
                    Vector3 Scale = Weapon.transform.localScale; Scale.x *= -1; Weapon.transform.localScale = Scale; 
                    break;
                }
        }
    }
    public void Reposition(Transform Hand,bool enemy)
    {

        GameObject Weapon = transform.gameObject;
        if (Weapon.transform.localScale.x < 0) { Vector3 Scale = Weapon.transform.localScale; Scale.x *= -1; Weapon.transform.localScale = Scale; }

        switch (Hand.transform.name)
        {
            case "s_l_hand":
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, -90);
                    Weapon.transform.position = Hand.position;
                    Weapon.transform.localRotation = rotation;
                    Weapon.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    break;
                }
            case "u_l_hand":
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, 32.8f);
                    Weapon.transform.SetParent(Hand);
                    Weapon.transform.position = Hand.position;
                    Weapon.transform.localRotation = rotation;
                    Weapon.GetComponent<SpriteRenderer>().sortingOrder = -4;
                    break;
                }
            case "d_r_hand":
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, 103.594f);
                    Weapon.transform.position = Hand.position;
                    Weapon.transform.localRotation = rotation;
                    Weapon.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    Vector3 Scale = Weapon.transform.localScale; Scale.x *= -1; Weapon.transform.localScale = Scale;
                    break;
                }
        }
    }
}
