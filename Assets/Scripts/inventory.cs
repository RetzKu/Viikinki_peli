using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour {

    public GameObject PlayerGameobject;
    private List<Item_Values> inventory_data;
	// Use this for initialization
	void Start ()
    {
        inventory_data = new List<Item_Values>(20);
    }
	
	// Update is called once per frame
	void Update ()
    {
        inventory_data = PlayerGameobject.GetComponent<Player_script>().invetory_data;
        
    }
}
