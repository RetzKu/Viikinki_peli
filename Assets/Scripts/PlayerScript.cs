using System.Collections.Generic;
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



        if (Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            if (clickPosition.x < 0.0f & GameObject.Find("Equip").transform.childCount >= 1)

            {

                GameObject.Find("s_c_torso").GetComponent<Animator>().SetTrigger("playerAttack");
                //GameObject.Find("Equip").transform.GetChild(0).transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
            }

            else if (clickPosition.x > 0.0f & GameObject.Find("Equip").transform.childCount >= 1)
            {
                GameObject.Find("s_c_torso").GetComponent<Animator>().SetTrigger("playerAttack");
                //GameObject.Find("Equip").transform.GetChild(0).transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
            }
        }

        else if (GameObject.Find("Equip").transform.childCount >= 1)
        {
            //EquipChild.transform.GetChild(0).transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }

        else { }

        //if (Input.GetKeyDown(KeyCode.A) == false & Input.GetKeyDown(KeyCode.S) == false & Input.GetKeyDown(KeyCode.D) == false & Input.GetKeyDown(KeyCode.W) == false);
        //{
        //    SpriteRenderer[] sprites = GameObject.Find("s_c_torso").GetComponentsInChildren<SpriteRenderer>();

        //    for (int i = 0; i < sprites.Length; i++)
        //    {
        //        sprites[i].enabled = false;
        //    }

        //    sprites = GameObject.Find("u_c_torso").GetComponentsInChildren<SpriteRenderer>();

        //    for (int i = 0; i < sprites.Length; i++)
        //    {
        //        sprites[i].enabled = false;
        //    }

        //    sprites = GameObject.Find("d_c_torso").GetComponentsInChildren<SpriteRenderer>();

        //    for (int i = 0; i < sprites.Length; i++)
        //    {
        //        sprites[i].enabled = true;
        //    }

        //}

        if(Input.GetKey(KeyCode.A) == true | Input.GetKey(KeyCode.D) == true)
        {

            transform.Find("s_c_torso").gameObject.GetComponent<Animator>().SetBool("playerRun", true);

            if (Input.GetKey(KeyCode.A) == true)
            {
                SpriteRenderer[] sprites = transform.Find("u_c_torso").gameObject.GetComponentsInChildren<SpriteRenderer>();

                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].enabled = false;
                }

                sprites = transform.Find("d_c_torso").gameObject.GetComponentsInChildren<SpriteRenderer>();

                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].enabled = false;
                }

                sprites = transform.Find("s_c_torso").gameObject.GetComponentsInChildren<SpriteRenderer>();

                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].enabled = true;
                }

                transform.Find("s_c_torso").gameObject.GetComponent<Transform>().localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }


            if (Input.GetKey(KeyCode.D) == true)
            {
                SpriteRenderer[] sprites = transform.Find("u_c_torso").gameObject.GetComponentsInChildren<SpriteRenderer>();

                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].enabled = false;
                }

                sprites = transform.Find("d_c_torso").gameObject.GetComponentsInChildren<SpriteRenderer>();

                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].enabled = false;
                }

                sprites = transform.Find("s_c_torso").gameObject.GetComponentsInChildren<SpriteRenderer>();

                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].enabled = true;
                }

                transform.Find("s_c_torso").gameObject.GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }

        else
        {
            transform.Find("s_c_torso").gameObject.GetComponent<Animator>().SetBool("playerRun", false);
        }
        

        if (Input.GetKey(KeyCode.W) == true)
        {
            SpriteRenderer[] sprites = transform.Find("u_c_torso").gameObject.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = true;
            }

            sprites = transform.Find("d_c_torso").gameObject.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = false;
            }

            sprites = transform.Find("s_c_torso").gameObject.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = false;
            }
        }

        if (Input.GetKey(KeyCode.S) == true)
        {
            SpriteRenderer[] sprites = transform.Find("u_c_torso").gameObject.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = false;
            }

            sprites = transform.Find("d_c_torso").gameObject.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = true;
            }

            sprites = transform.Find("s_c_torso").gameObject.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = false;
            }
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
        if (Trig.transform.tag == "Item")
        {
            AddToInventory(Trig);
            Debug.Log(Trig.transform.name + " Picked up");
        }

        if (Trig.gameObject.tag == "puu")
        {
            Debug.Log("BONK");
            Trig.GetComponent<TreeHP>().hp -= 25;
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
            Item.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            Item.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
            Instantiate(Item.gameObject, GameObject.Find("Equip").transform);
            EquipChild.transform.GetChild(0).name = Item.transform.name;
            Destroy(Item.gameObject);
        }
        else
        {
            if (InventoryChild.transform.childCount < InventorySize)
            {
                Item.transform.position = GameObject.Find("Player").transform.position;
                Instantiate(Item.gameObject, InventoryChild.transform);

                Debug.Log("Elseasd");

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
                EquipCopy.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                Instantiate(EquipCopy, InventoryChild.transform).transform.name = EquipCopy.transform.name;
                Destroy(EquipCopy);
            }
            InventoryCopy.transform.position = GameObject.Find("Player").transform.position;
            Instantiate(InventoryCopy, EquipChild.transform).transform.name = InventoryCopy.transform.name;
            Destroy(InventoryCopy);
        }
    }
}