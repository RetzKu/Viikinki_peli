using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : weaponStats
{
    private GameObject Weapon;

    private void FixedUpdate()
    {
        if (duration <= 0)
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

    public void Reposition(Transform Hand)
    {
        Weapon = transform.gameObject;

        if (Hand.name == "u_l_hand") { Weapon.transform.localRotation = Quaternion.Euler(0, 0, 90); Weapon.GetComponent<SpriteRenderer>().sortingOrder = 0; }
        if (Hand.name == "d_r_hand") { Weapon.transform.localRotation = Quaternion.Euler(0, 0, -90); }
        if (Hand.name == "s_l_hand") { Weapon.transform.localRotation = Quaternion.Euler(0, 0, -67); Weapon.GetComponent<SpriteRenderer>().sortingOrder = 1; }
        
        if (Weapon.transform.localScale.x < 0){ Vector3 Scale = Weapon.transform.localScale; Scale.x *= -1; Weapon.transform.localScale = Scale;}

    }
}
