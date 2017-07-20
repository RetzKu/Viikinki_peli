using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notskaScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.tag == "notPatePlayer" && GetComponent<CampFire>().type != ResourceType.campfire_noFire)
        {
            GameObject.Find("Player").GetComponent<combat>().setPlayerOnFire();
        }
    }
}
