using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armorScript : MonoBehaviour {

    public float ArmorMultiplier = 1.5f;
    public int duration = 10;

    // Tällä lisätään pelaajalle armoria
    public void addArmorStats()
    {
        GameObject.Find("Player").GetComponent<combat>().armor *= ArmorMultiplier;
    }
}
