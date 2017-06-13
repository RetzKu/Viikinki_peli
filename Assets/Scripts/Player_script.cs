using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_script : MonoBehaviour {

    private List<Item_Values> inventory;
    private List<Item_Values> id_in_range;

    Vector3 startPoint;
    Vector3 endPoint;
    public bool running = false;
	
    void Start () {
        inventory = new List<Item_Values>(10);
        id_in_range = new List<Item_Values>(999);
	running = true;
	}

    void Update()
    {
	    
	var mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 playerPosition = new Vector3(transform.position.x, transform.position.y, 0.0f); // Pelaajan positio
        Vector3 clickPosition = new Vector3();
        clickPosition.x = Camera.main.ScreenToWorldPoint(mousePos).x - playerPosition.x; // clickPosition on loppupiste - alkupiste
        clickPosition.y = Camera.main.ScreenToWorldPoint(mousePos).y - playerPosition.y;
        clickPosition.z = 0.0f;

        Vector3.Normalize(clickPosition);
        //print(clickPosition);

        startPoint = playerPosition; // Pelaajan positio
        endPoint = Camera.main.ScreenToWorldPoint(mousePos); // Hiiren osoittama kohta
	    
        if (Input.GetAxisRaw("Interract") == 1)
        {

        }
    }
	
    void OnDrawGizmos()
    {
        if (running == true)
        {
            Gizmos.DrawLine(startPoint, endPoint); // piirretään viiva visualisoimaan toimivuutta
        }
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
            id_in_range.Add(new Item_Values(Trig.GetComponent<item_script>().ID, Trig.GetComponent<item_script>().Name, Trig.GetComponent<item_script>().Atk));
        }
        else
        {
            if(on_off == false)
            {
                int id = Trig.GetComponent<item_script>().ID;
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

