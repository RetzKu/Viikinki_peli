using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private List<Item_Values> inventory;
    private List<Item_Values> id_in_range;

    void Start () {
        inventory = new List<Item_Values>(10);
        id_in_range = new List<Item_Values>(999);
	}
    void Update()
    {
        if (Input.GetAxisRaw("Interract") == 1)
        {

        }
    }
    void OnTriggerEnter2D(Collider2D Trig)
    {
        if(Trig.gameObject.tag == "Item")
        {

            int ID = Trig.GetComponent<item>().ID;
            string Name = Trig.GetComponent<item>().Name;
            int Atk = Trig.GetComponent<item>().Atk;

            in_range(Trig, true);
            //inventory.Add(new Item(ID,Name,Atk));

            Debug.Log("item");
        }
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
            id_in_range.Add(new Item_Values(Trig.GetComponent<item>().ID, Trig.GetComponent<item>().Name, Trig.GetComponent<item>().Atk));
        }
        else
        {
            if(on_off == false)
            {
                int id = Trig.GetComponent<item>().ID;
                //int it = id_in_range.IndexOf(id);
                //Debug.Log(it);
                //id_in_range.Remove(it);
            }
        }
    }
}

class Item_Values
{
    int ID;
    string Name;
    int Atk;

    public Item_Values(int ID, string Name, int Atk) { }
}

