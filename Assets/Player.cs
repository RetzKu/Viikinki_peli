using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private List<Item> inventory;
    private List<int> id_in_range;

    void Start () {
        inventory = new List<Item>(10);
        id_in_range = new List<int>(999);
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

            in_range(Trig, true);
            //inventory.Add(new Item(ID,Name,Atk));

            Debug.Log("item");
        }

        //vektor.Add(new Item(1, "mieka", 12));
    }
    void OnTriggerExit2D(Collider2D Trig)
    {
        Debug.Log("item exit");
        in_range(Trig, false);
        
    }
    void in_range(Collider2D Trig,bool on_off)
    {
        if (on_off == true)
        {
            id_in_range.Add(Trig.GetComponent<item_script>().ID);
        }
        else
        {
            if(on_off == false)
            {
                int id = Trig.GetComponent<item_script>().ID;
                int it = id_in_range.IndexOf(id);
                Debug.Log(it);
                //id_in_range.Remove(it);
            }
        }
        if (Input.GetAxisRaw("Interract") == 1)
        {

        }
    }
}

class Item
{
    int ID;
    string Name;
    int Atk;

    public Item(int ID, string Name, int Atk) { }
}

