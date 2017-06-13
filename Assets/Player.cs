using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private List<Item> inventory;

    void Start () {
        inventory = new List<Item>(10);
	}
    void Update()
    {
    }
    void OnTriggerEnter2D(Collider2D Trig)
    {
        if(Trig.gameObject.tag == "Item")
        {

            int ID = Trig.GetComponent<item_script>().ID;
            string Name = Trig.GetComponent<item_script>().Name;
            int Atk = Trig.GetComponent<item_script>().Atk;


            inventory.Add(new Item(ID,Name,Atk));

            Debug.Log("item");
        }

        //vektor.Add(new Item(1, "mieka", 12));
    }
    void OnTriggerExit2D(Collider2D Trig)
    {
        Debug.Log("item exit");
    }
}

class Item
{
    int ID;
    string Name;
    int Atk;

    public Item(int ID, string Name, int Atk) { }
}