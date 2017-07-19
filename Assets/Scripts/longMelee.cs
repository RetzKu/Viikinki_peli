﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class longMelee : weaponStats
{
    private GameObject Weapon;

    public void Reposition(Transform Hand)
    {
        Weapon = transform.gameObject;

        if (Hand.name == "u_l_hand") { Weapon.transform.localRotation = Quaternion.Euler(0, 0, 6); Weapon.GetComponent<SpriteRenderer>().sortingOrder = 0;}
        if (Hand.name == "d_r_hand") { Weapon.transform.localRotation = Quaternion.Euler(0, 0, 163.6f); Weapon.GetComponent<SpriteRenderer>().sortingOrder = 11;}
        if (Hand.name == "s_l_hand") { Weapon.transform.localRotation = Quaternion.Euler(0, 0,-90); Weapon.GetComponent<SpriteRenderer>().sortingOrder = 11; }

    }
}
