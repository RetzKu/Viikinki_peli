using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public int InventorySize;

    private GameObject InventoryChild;
    private GameObject EquipChild;

    Vector3 startPoint;
    Vector3 endPoint;

    public bool running = true;

    public Transform SidewaysHand;
    public Transform UpwardsHand;
    public Transform DownwardsHand;

    private ItemManager Hand;
    public int Damage;

    private int Direction;

    void Start()
    {
        /*Get Inventory parents*/
        InventoryChild = gameObject.transform.Find("Inventory").gameObject;
        EquipChild = gameObject.transform.Find("Equip").gameObject;

        /*Get Player Gameobjects hands*/
        SidewaysHand = transform.Find("s_c_torso").Find("s_l_upper_arm").GetChild(0).GetChild(0);
        UpwardsHand = transform.Find("u_c_torso").Find("u_l_upper_arm").GetChild(0).GetChild(0);
        DownwardsHand = transform.Find("d_c_torso").Find("d_r_upper_arm").GetChild(0).GetChild(0);
        //pate on paras
        Hand = new ItemManager(SidewaysHand);
        Damage = 30;
    }

    void Update()           
    {
        tmpswing();
        Equip();
        Drop();
        Side();
        Direction = transform.GetComponent<AnimatorScript>().PlayerDir();
    }

    void Side()
    {
        if (Direction == 2) { Hand.Handstate = 2; Hand.SetHand(UpwardsHand); }
        else if (Direction == 1) { Hand.Handstate = 1; Hand.SetHand(DownwardsHand); }
        else if (Direction == 0 || Direction == 3) { Hand.Handstate = 0; Hand.SetHand(SidewaysHand); }
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
            Trig.transform.parent.GetComponent<TreeHP>().hp -= Damage;
        }
    }

    void OnTriggerExit2D(Collider2D Trig)
    {
        if (Trig.transform.tag == "Dropped")
        {
            Trig.transform.tag = "Item";
            print("escaped dropped item");
        }
    }

    /*WHEN TIME, TRANSFER DEFAULT METHODS TO THIS CLASS*/
    public class ItemManager
    {
        private Transform Hand;
        public int Handstate = 0;

        public ItemManager(Transform _Hand) { Hand = _Hand; } //default builder requires atleast 1 hand at the start

        public void Equip(GameObject Item)
        {
            if (Hand.transform.childCount > 0)
            {
                EmptyHand();
            }
            GameObject Copy = Instantiate(Item) as GameObject;
            Copy.name = Item.transform.name;
            Copy.transform.SetParent(Hand);

            if (Handstate == 0)
            {
                Quaternion rotation = Quaternion.Euler(0, 0, -90);
                Copy.transform.SetParent(Hand);
                Copy.transform.position = Hand.position;
                Copy.transform.localRotation = rotation;
                Copy.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                Copy.GetComponent<SpriteRenderer>().sortingOrder = 20;
            }
            if (Handstate == 1) // downwards
            {
                Quaternion rotation = Quaternion.Euler(0,0, 103.594f);
                Copy.transform.SetParent(Hand);
                Copy.transform.position = Hand.position;
                Copy.transform.localRotation = rotation;
                Copy.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                Copy.GetComponent<SpriteRenderer>().sortingOrder = 16;
            }
            if (Handstate == 2) //upwards
            {
                Quaternion rotation = Quaternion.Euler(0, 0, 32.8f);
                Copy.transform.SetParent(Hand);
                Copy.transform.position = Hand.position;
                Copy.transform.localRotation = rotation;
                Copy.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                Copy.GetComponent<SpriteRenderer>().sortingOrder = 8;
            }
        }

        public void EmptyHand()
        {
            if (Hand.childCount > 0)
            {
                Destroy(Hand.GetChild(0).gameObject); // Destroy item from hand
            }
        }

        public void SetHand(Transform _Hand) //used to redefine hand to be used
        {
            if (_Hand.transform.name != Hand.transform.name)
            {
                if (Hand.childCount > 0)
                {
                    Hand.GetChild(0).SetParent(_Hand);
                    Hand = _Hand;
                    GameObject Copy = Hand.transform.GetChild(0).gameObject;
                    switch (Hand.transform.name)
                    {
                        case "s_l_hand":
                            {
                                Quaternion rotation = Quaternion.Euler(0, 0, -90);
                                Copy.transform.position = Hand.position;
                                Copy.transform.localRotation = rotation;
                                Copy.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                                Copy.GetComponent<SpriteRenderer>().sortingOrder = 20;
                                break;
                            }
                        case "u_l_hand":
                            {
                                Quaternion rotation = Quaternion.Euler(0, 0, 32.8f);
                                Copy.transform.SetParent(Hand);
                                Copy.transform.position = Hand.position;
                                Copy.transform.localRotation = rotation;
                                Copy.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                                Copy.GetComponent<SpriteRenderer>().sortingOrder = 8;
                                break;
                            }
                        case "d_r_hand":
                            {
                                Quaternion rotation = Quaternion.Euler(0, 0, 103.594f);
                                Copy.transform.SetParent(Hand);
                                Copy.transform.position = Hand.position;

                                Copy.transform.localRotation = rotation;
                                Copy.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                                Copy.GetComponent<SpriteRenderer>().sortingOrder = 16; break;
                            }
                    }
                }
            }
        }

    }

    void AddToInventory(Collider2D Item)
    {
        int it = 0;
        if (EquipChild.transform.childCount == 0)
        {

            GameObject Copy = Instantiate(Item.gameObject, EquipChild.transform) as GameObject;
            Copy.name = Item.transform.name;
            Hand.Equip(Copy);
            Destroy(Item.gameObject);
        }
        else
        {
            if (InventoryChild.transform.childCount < InventorySize)
            {
                Item.transform.position = GameObject.Find("Player").transform.position;
                Instantiate(Item.gameObject, InventoryChild.transform);

                switch (InventoryChild.transform.childCount)
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
        if (Input.GetKeyDown("f") == true)
        {
            if (EquipChild.transform.childCount > 0)
            {
                Hand.EmptyHand();
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
        if (Input.GetKeyDown("1") == true) { if (InventoryChild.transform.childCount > 0) { swap = true; it = 0; } }
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

            InventoryCopy.transform.position = GameObject.Find("Player").transform.position;
            GameObject Copy = Instantiate(InventoryCopy, EquipChild.transform) as GameObject;
            Copy.transform.name = InventoryCopy.name;

            Destroy(InventoryCopy);

            Hand.Equip(Copy);
        }
    }


}