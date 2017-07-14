using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolfHeadScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D triggeriino)
    {
        if(triggeriino.tag == "notPatePlayer")
        {
            // TÄMÄ TOIMII ATM
            // TÄHÄN METODI TEHDÄ PELAAJAAN DAMAGEA
            // EASYPEASY
        }
    }
}
