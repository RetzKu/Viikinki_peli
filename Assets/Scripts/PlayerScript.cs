﻿using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    internal List<ItemScript> invetory_data;
    public int InventorySize;

    private bool interraction_cd = false;

    private GameObject InventoryChild;
    private GameObject EquipChild;

    Vector3 startPoint;
    Vector3 endPoint;

    public bool running = true;

    private GameObject text_object;


    void Start()
    {
        invetory_data = new List<ItemScript>(InventorySize);

        InventoryChild = gameObject.transform.Find("Inventory").gameObject;
        EquipChild = gameObject.transform.Find("Equip").gameObject;
    }

    void Update()
    {
        tmpswing();
        Equip();
        Drop();
    }

    void tmpswing()
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
            //print(clickPosition);
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
        if (Trig.transform.tag != "Dropped")
        {
            AddToInventory(Trig);
            print("Poimittu"); 
        }
        else
        {
            print("Dropped item on the ground");
        }
    }

    void OnTriggerExit2D(Collider2D Trig)
    { 
        if(Trig.transform.tag == "Dropped")
        {
            Trig.transform.tag = "Item";
            print("escaped dropped item");
        }
    }

    void AddToInventory(Collider2D Item)
    {
        int it = 0;
        if (EquipChild.transform.childCount == 0)
        {
            Instantiate(Item.gameObject, EquipChild.transform);
            EquipChild.transform.GetChild(0).name = Item.transform.name;
            Destroy(Item.gameObject);
        }
        else
        {
            if (InventoryChild.transform.childCount < InventorySize)
            {
                Instantiate(Item.gameObject, InventoryChild.transform);

                switch(InventoryChild.transform.childCount)
                {
                    case 1: { it = 0; break; }
                    case 2: { it = 1; break; }
                    case 3: { it = 2; break; }
                }
                InventoryChild.transform.GetChild(it).name = Item.transform.name;
                Destroy(Item.gameObject);
            }
        }
    }

    void Drop()
    {
        if(Input.GetKeyDown("f") == true)
        {
            if (EquipChild.transform.childCount > 0)
            {
                GameObject EquipCopy = EquipChild.transform.GetChild(0).gameObject;
                EquipCopy.transform.tag = "Dropped";
                Instantiate(EquipCopy, gameObject.transform.position, EquipCopy.transform.rotation).transform.name = EquipCopy.transform.name;
                Destroy(EquipCopy); 
            }
        }
    }
    
    void Equip()
    {
        bool swap = false;
        int it = 0;
        if (Input.GetKeyDown("1") == true) { if (InventoryChild.transform.childCount > 0) { swap = true; it = 0; }}
        if (Input.GetKeyDown("2") == true) { if (InventoryChild.transform.childCount > 1) { swap = true; it = 1; } }
        if (Input.GetKeyDown("3") == true) { if (InventoryChild.transform.childCount > 2) { swap = true; it = 2; } }

        if (swap == true)
        {
            GameObject InventoryCopy = InventoryChild.transform.GetChild(it).gameObject;

            if (EquipChild.transform.childCount != 0)
            {
                GameObject EquipCopy = EquipChild.transform.GetChild(0).gameObject;
                Instantiate(EquipCopy, InventoryChild.transform).transform.name = EquipCopy.transform.name;
                Destroy(EquipCopy);
            }
            Instantiate(InventoryCopy, EquipChild.transform).transform.name = InventoryCopy.transform.name;
            Destroy(InventoryCopy);
        }
    }
}