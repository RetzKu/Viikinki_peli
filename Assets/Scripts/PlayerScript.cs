using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    internal List<ItemScript> invetory_data;
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

    enum direction
    {
        right, left, up, down
    }

    direction playerDirection = direction.down; // Pelaaja defaulttina katsoo alaspain
    bool playerMoving = false;  // Pelaaja defaulttina ei ole liikkeessa

    void Start()
    {
        invetory_data = new List<ItemScript>(InventorySize);

        InventoryChild = gameObject.transform.Find("Inventory").gameObject;
        EquipChild = gameObject.transform.Find("Equip").gameObject;

        /*Get Player Gameobjects hands*/
        SidewaysHand = transform.Find("s_c_torso").Find("s_l_upper_arm").GetChild(0).GetChild(0);
        UpwardsHand = transform.Find("u_c_torso").Find("u_l_upper_arm").GetChild(0).GetChild(0);
        DownwardsHand = transform.Find("d_c_torso").Find("d_l_upper_arm").GetChild(0).GetChild(0);

        Hand = new ItemManager(SidewaysHand);
        Damage = 30;
    }

    void Update()
    {
        Direction();
        tmpswing();
        Equip();
        Drop();
        Side();
    }

    void Direction()    // Tarkistetaan pelaajan suunta ja liikkuuko se
    {
        if (Input.GetKeyDown(KeyCode.D) == true)
        {
            playerDirection = direction.right;
            playerMoving = true;
            //Debug.Log(playerDirection);
        }

        else if (Input.GetKeyDown(KeyCode.A) == true)
        {
            playerDirection = direction.left;
            playerMoving = true;
            //Debug.Log(playerDirection);
        }

        else if (Input.GetKeyDown(KeyCode.S) == true)
        {
            playerDirection = direction.down;
            playerMoving = true;
            //Debug.Log(playerDirection);
        }

        else if (Input.GetKeyDown(KeyCode.W) == true)
        {
            playerDirection = direction.up;
            playerMoving = true;
            //Debug.Log(playerDirection);
        }

        else if(Input.GetKey(KeyCode.W) == false & Input.GetKey(KeyCode.A) == false & Input.GetKey(KeyCode.S) == false & Input.GetKey(KeyCode.D) == false)
        {
            playerMoving = false;
        }

        else
        {

        }
    }

    direction GetPlayerDirection()
    {
        return playerDirection;
    }

    void Side()
    {
        if(Input.GetKeyDown("w")) { Hand.Handstate = 2; Hand.SetHand(UpwardsHand); }
        if(Input.GetKeyDown("s")) { Hand.Handstate = 1; Hand.SetHand(DownwardsHand); }
        if (Input.GetKeyDown("a") || Input.GetKeyDown("d")) { Hand.Handstate = 0; Hand.SetHand(SidewaysHand); }
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

            transform.Find("s_c_torso").GetComponent<Animator>().SetTrigger("playerAttack");

            /*
            if (clickPosition.x < 0.0f & transform.Find("Equip").transform.childCount >= 1)

            {
                transform.Find("s_c_torso").GetComponent<Animator>().SetTrigger("playerAttack");
                //GameObject.Find("Equip").transform.GetChild(0).transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
            }

            else if (clickPosition.x > 0.0f & transform.Find("Equip").transform.childCount >= 1)
            {
                transform.Find("s_c_torso").GetComponent<Animator>().SetTrigger("playerAttack");
                //GameObject.Find("Equip").transform.GetChild(0).transform.eulerAngles = new Vector3(0.0f, 0.0f, 270.0f);
            }
            */
        }

        else if (transform.Find("Equip").transform.childCount >= 1)
        {
            //EquipChild.transform.GetChild(0).transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }

        else { }

        if (playerMoving == false)
        {
            SpriteRenderer[] sprites = GameObject.Find("s_c_torso").GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = false;
            }

            sprites = GameObject.Find("u_c_torso").GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = false;
            }

            sprites = GameObject.Find("d_c_torso").GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].enabled = true;
            }

            transform.Find("s_c_torso").gameObject.GetComponent<Animator>().SetBool("playerRun", false);
            transform.Find("d_c_torso").gameObject.GetComponent<Animator>().SetBool("playerRun", false);
            transform.Find("u_c_torso").gameObject.GetComponent<Animator>().SetBool("playerRun", false);

        }

        if (playerDirection == direction.right | playerDirection == direction.left)
        {
            if (playerMoving == true)
            {
                transform.Find("s_c_torso").gameObject.GetComponent<Animator>().SetBool("playerRun", true);
            }

            if (playerDirection == direction.left)
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


            if (playerDirection == direction.right)
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


        if (playerDirection == direction.up)
        {

            if (playerMoving == true)
            {
                transform.Find("u_c_torso").gameObject.GetComponent<Animator>().SetBool("playerRun", true);
            }

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

        if (playerDirection == direction.down)
        {

            if (playerMoving == true)
            {
                transform.Find("d_c_torso").gameObject.GetComponent<Animator>().SetBool("playerRun", true);
            }

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
            Trig.GetComponent<TreeHP>().hp -= Damage;
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
            if (Handstate == 1)
            {
                /*REQUIRES SETTINGS*/
            }
            if (Handstate == 2)
            {
                /*REQUIRES SETTINGS*/
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
            if(_Hand.transform.name != Hand.transform.name)
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
                        case "u_l_hand": { /*REQUIRES SETTINGS*/ break; }
                        case "d_l_hand": { /*REQUIRES SETTINGS*/ break; }
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