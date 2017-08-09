using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armorScript : MonoBehaviour {

    public float ArmorMultiplier = 2f;
    public float duration = 10;

    // Tällä lisätään pelaajalle armoria
    public void addArmorStats()
    {
        GameObject.Find("Player").GetComponent<combat>().armor += (int)ArmorMultiplier;
    }
    public void RemoveArmorStats()
    {
        GameObject.Find("Player").GetComponent<combat>().armor -= (int)ArmorMultiplier;
    }

    public void UseDurability(float amount)
    {
        duration -= amount;
        if(duration < 1) { GameObject.Find("Player").GetComponent<PlayerScript>().BreakArmor(); }
    }
}
