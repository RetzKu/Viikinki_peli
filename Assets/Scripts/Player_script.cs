﻿using System.Collections.Generic;
using UnityEngine;

public class Player_script : MonoBehaviour
{

    internal List<item_script> invetory_data;
        public int InventorySize;

    private List<Item_Values> id_in_range;
    private Item_Values closest_item;

    private bool interraction_cd = false;
    private bool inventory_cd = false;

    Vector3 startPoint;
    Vector3 endPoint;

    public bool running = true;

    public GameObject text_object;


    void Start()
    {
        invetory_data = new List<item_script>(InventorySize);
        id_in_range = new List<Item_Values>(100);
    }

    void Update()
    {

        KeyCommands();
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

        if (clickPosition.x < 0.0f)
        {
            if (GameObject.Find("Lapio").GetComponent<SpriteRenderer>().flipX == false)
            {
                //GameObject.Find("Lapio").GetComponent<Transform>().position.x = -0.4; // Väittää että muuttuja olisi constant vaikkei pitäisi olla :c
                GameObject.Find("Lapio").GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        if (clickPosition.x > 0.0f)
        {
            if (GameObject.Find("Lapio").GetComponent<SpriteRenderer>().flipX == true)
            {
                GameObject.Find("Lapio").GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) == true)
        {

            GetComponentInChildren<Animator>().SetTrigger("lapioAttack");
            print(clickPosition);
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
        if (Trig.gameObject.tag == "Item")
        {
            in_range(Trig, true);
            Debug.Log("item in range");
        }
    }

    void OnTriggerExit2D(Collider2D Trig)
    {
        in_range(Trig, false);
        Debug.Log("item no longer in range");
    }

    void in_range(Collider2D Trig, bool on_off)
    {
        if (on_off == true)
        {
            id_in_range.Add(new Item_Values(Trig));
        }
        else
        {
            if (on_off == false)
            {
                int id = Trig.GetComponent<item_script>().ID;
                int it = id_in_range.FindIndex(x => x.Trig.GetComponent<item_script>().ID == id);
                id_in_range.RemoveAt(it);
            }
        }
    }

    void closest()
    {
        closest_item = null;
        if (id_in_range.Count != 0)
        {
            int it = 0;
            float closest_distance = 9999;
            for (int i = 0; i < id_in_range.Count; i++)
            {
                if(Vector2.Distance(id_in_range[i].Trig.bounds.center, transform.position)< closest_distance)
                {
                    it = i;
                }
            }
            closest_item = id_in_range[it];
            Debug.Log(closest_item.Trig.bounds.center.ToString("f4"));
        }
    }

    void pick_up()
    {
        Debug.Log(id_in_range.Count);
        if(id_in_range.Count > 0)
        {
            int it = id_in_range.FindIndex(x => x.Trig.GetComponent<item_script>().ID == closest_item.Trig.GetComponent<item_script>().ID);
            invetory_data.Add(closest_item.Trig.GetComponent<item_script>());
            closest_item = null;
            Destroy(id_in_range[it].Trig.gameObject);
        }
    }

    void KeyCommands()
    {
        Interraction();
        InventoryToggle();
    }


    /*Start of KeyCommands() internal functions*/
    void InventoryToggle()
    {
        if (inventory_cd == false)
        {
            if (Input.GetAxisRaw("Inventory") == 1)
            {
                inventory_cd = true;
                switch (text_object.activeSelf)
                {
                    case true:
                        {
                            text_object.SetActive(false);
                            break;   
                        }
                    case false:
                        {
                            text_object.SetActive(true);
                            break;
                        }
                }
            }
        }
        if (Input.GetAxisRaw("Inventory") == 0) { inventory_cd = false; }
    }
    void Interraction()
    {
        if (interraction_cd == false)
        {
            closest();
            if (Input.GetAxisRaw("Interract") == 1)
            {
                Debug.Log("pressed f for respect");
                interraction_cd = true;
                pick_up();
            }
        }
        if (Input.GetAxisRaw("Interract") == 0) { interraction_cd = false; }
    }
}

public class Item_Values
{
    public Collider2D Trig;
    public Item_Values(Collider2D _Trig) { Trig = _Trig;}
}


