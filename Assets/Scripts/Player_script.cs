using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_script : MonoBehaviour {

    private List<Item_Values> inventory;
    private List<Item_Values> id_in_range;

    private bool cd = false;


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

        if (cd == false)
        {
          if (Input.GetAxisRaw("Interract") == 1)
          {
              if (Input.GetAxisRaw("Interract") == 1)
              {
                  Debug.Log("pressed f for respect");
                  cd = true;
                  pick_up();
              } 
          }
        if (Input.GetAxisRaw("Interract") == 0)
        {
            cd = false;
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
            in_range(Trig, true);
            Debug.Log("item found");
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
                int it = id_in_range.FindIndex(x => x.ID == Trig.GetComponent<item_script>().ID);
                id_in_range.RemoveAt(it);  
            }
        }
    }
    void pick_up()
    {
        Debug.Log(id_in_range.Count);
    }
}

class Item_Values
{
    public int ID;
    string Name;
    int Atk;

    public Item_Values(int _ID, string _Name, int _Atk) { ID = _ID; Name = _Name; Atk = _Atk; }
}

