using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject PlayerGameobject;
    private List<item_script> inventory_data;
    public Text Text_object;

	void Start ()
    {
        inventory_data = new List<item_script>(PlayerGameobject.GetComponent<Player_script>().InventorySize);
    }
	
	void Update ()
    {
        inventory_data = PlayerGameobject.GetComponent<Player_script>().invetory_data;
        if(inventory_data.Count != 0)
        {
            screen_inventory();
        }
        else
        {
            Text_object.text = "Inventory empty";
        }
    }

    void screen_inventory()
    {
        string inventory_string = null;
        string Name;

        for (int i = 0; i < inventory_data.Count; i++)
        {
            Name = inventory_data[i].Name;
            inventory_string += string.Format("Name:{0} \n", Name);
            
        }
        Text_object.text = inventory_string;

    }
}
