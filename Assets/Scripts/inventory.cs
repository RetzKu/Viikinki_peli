using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private GameObject PlayerGameobject;
    private List<item_script> inventory_data;
    //public Text TextObjectPrefab;

	void Start ()
    {
        PlayerGameobject = GameObject.FindGameObjectWithTag("Player");
    }
	
	void Update ()
    {
        inventory_data = PlayerGameobject.GetComponent<PlayerScript>().invetory_data;
        if(inventory_data.Count != 0)
        {
            screen_inventory();
        }
        else
        {
            //Text_object.text = "Inventory empty";
        }
    }

    void screen_inventory()
    {

    }
}
