using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armorScript : MonoBehaviour {

    public float ArmorMultiplier = 2f;
    public int duration = 10;
    public int MaxDuration;

    private void Start()
    {
        MaxDuration = duration;
    }

    // Tällä lisätään pelaajalle armoria
    public void addArmorStats()
    {
        GameObject.Find("Player").GetComponent<combat>().armor = (int)ArmorMultiplier;
    }
    public void RemoveArmorStats()
    {
        GameObject.Find("Player").GetComponent<combat>().armor = 0;
    }

    public float CalculateDuration()
    {
        float DurationPrecent = duration / (float)MaxDuration;
        return DurationPrecent;
    }

    public void UseDurability(int amount)
    {
        duration -= amount;
        if(duration < 1) { GameObject.Find("Player").GetComponent<PlayerScript>().BreakArmor(); }
    }
}
