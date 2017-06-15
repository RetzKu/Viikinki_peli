using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject PlayerGameobject;
    private List<Item_Values> inventory_data;
    public Text Text_object;
	// Use this for initialization
	void Start ()
    {
        inventory_data = new List<Item_Values>(20);
    }
	
	// Update is called once per frame
	void Update ()
    {
        inventory_data = PlayerGameobject.GetComponent<Player_script>().invetory_data;
        if(inventory_data.Count != 0) { screen_inventory(); }
    }
    void screen_inventory()
    {
        //string invetory_string;
        //for(int i = 0; i < inventory_data.Count; i++)
        //{
            
        //}
    }
}
