using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_c_torsoScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(alive()) // Suoritetaan jos hitbox on ylipäätään elossa
        {
            
        }
	}

    bool alive()
    {
        if(GetComponent<Collider2D>().enabled == true)
        {
            return true;
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Tässä vaiheessa pitäisi tarkistaa keneltä viholliselta otetaan damagea
            // --> Paten scriptiä ei ole vielä commitettu
            // if (getcomponent<generalaitjsp>().myType == jokuEnum.archer){} 

            // Nämä damaget ladataan myöhemmin suoraan vihollisten prefabeista
            if (trig.gameObject.tag == "puu")
            {
                GetComponentInParent<combat>().takeDamage(20.0f);
            }
        }


        if (trig.transform.tag == "Item")
        {
            if(trig.transform.name == "Arrow")
            {
                GetComponent<PlayerScript>().EquippedTool.ArrowCount++;
                Destroy(trig.gameObject);
            }
            else
            { 
               GetComponentInParent<PlayerScript>().Inventory.AddToInventory(trig.gameObject);
               Debug.LogWarning(trig.transform.name + " Picked up");
            }
        }
    }
    void OnTriggerExit2D(Collider2D Trig)
    {
        if (Trig.transform.tag == "Dropped")
        {
            Trig.transform.tag = "Item";
            print("escaped dropped item");
        }
    }
}
