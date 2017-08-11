using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notskaScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.tag == "Player" && GetComponent<CampFire>().type != ResourceType.campfire_noFire)
        {
            GameObject tempPlayer;
            tempPlayer = GameObject.Find("Player");
            tempPlayer.GetComponent<combat>().setHitPosition(transform.position);
            tempPlayer.GetComponent<combat>().setPlayerOnFire();
        }
    }
}
