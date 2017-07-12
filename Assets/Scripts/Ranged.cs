﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : weaponStats
{
    private GameObject Weapon;

    void start()
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            BowUse();
        }
    }

    public void Reposition(Transform Hand)
    {
        Weapon = transform.gameObject;

        if (Hand.name == "u_l_hand") { Weapon.transform.localRotation = Quaternion.Euler(0, 0, -73); Weapon.transform.localPosition = new Vector3(0.032f, 0.019f, 0); }
        if (Hand.name == "d_r_hand") { Weapon.transform.localRotation = Quaternion.Euler(0, 0, -105); }
        if (Hand.name == "s_l_hand") { Weapon.transform.localRotation = Quaternion.Euler(0, 0, -67); }

        if (Weapon.transform.localScale.x < 0){ Vector3 Scale = Weapon.transform.localScale; Scale.x *= -1; Weapon.transform.localScale = Scale;}
    }
    public void BowUse()
    {
        GameObject.Find("Player").GetComponent<AnimatorScript>().BowUse();
    }
}
