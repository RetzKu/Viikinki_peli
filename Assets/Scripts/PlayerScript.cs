using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum WeaponType { noWeapon, meleeWeapon, longMeleeWeapon, rangedWeapon, Armor }

public class PlayerScript : MonoBehaviour
{

    public int InventorySize;

    private GameObject InventoryChild;
    private GameObject EquipChild;

    // List<Item> invontory;

    private weaponScript current;

    [Header("Inventory Data")]
    [SerializeField]
    internal inventory Inventory;

    public inventory.Equipped EquippedTool { get { return Inventory.EquipData; } } //saa helposti equipatun tavaran tiedota, scriptien sisällöt etc.
    public int Arrows { get { return EquippedTool.ArrowCount; } }


    Vector3 startPoint;
    Vector3 endPoint;

    public bool running = true;

    public Transform SidewaysHand;
    public Transform UpwardsHand;
    public Transform DownwardsHand;

    private ItemManager Hand;
    public GameObject weaponInHand { get { return Hand.Copy; } } //jos haluaa collisioniin koskea tai tehdä collision tarkastuksia

    private List<SpriteRenderer> Torsos;
    public List<Sprite> DefaultTorsos;
    public int Direction;
    internal CanvasController HpCanvas;

    void Start()
    {
        HpCanvas = new CanvasController();
        Torsos = new List<SpriteRenderer>(3);
        /*Get Inventory parents*/
        InventoryChild = gameObject.transform.Find("Inventory").gameObject;
        EquipChild = gameObject.transform.Find("Equip").gameObject;
        Inventory = new inventory(InventorySize);

        /*Get Player Gameobjects hands*/
        SidewaysHand = transform.Find("s_c_torso").Find("s_l_upper_arm").GetChild(0).GetChild(0);
        UpwardsHand = transform.Find("u_c_torso").Find("u_l_upper_arm").GetChild(0).GetChild(0);
        DownwardsHand = transform.Find("d_c_torso").Find("d_r_upper_arm").GetChild(0).GetChild(0);
        //pate on paras
        Hand = new ItemManager(SidewaysHand);
        //Damage = 30
        Torsos.Add(transform.Find("d_c_torso").GetComponent<SpriteRenderer>());
        Torsos.Add(transform.Find("s_c_torso").GetComponent<SpriteRenderer>());
        Torsos.Add(transform.Find("u_c_torso").GetComponent<SpriteRenderer>());
    }

    void Update()
    {
        tmpswing();
        Side();
        Direction = GetComponent<AnimatorScript>().PlayerDir();
        if(Inventory.ArmorEquipped == true) { EquipArmor(); Inventory.ArmorEquipped = false; }
        RefreshHand();
        InventoryInput();
        if(Inventory.EquipData.Armor != null) { HpCanvas.ToggleArmorImage(true); } else { HpCanvas.ToggleArmorImage(false); }
    }

    public void LoseDurability()
    {
        //Inventory.EquipData.Tool.GetComponent<weaponStats>().useDuration();
        Inventory.InventoryData.Find(a => a.name == EquippedTool.Tool.name).GetComponent<weaponStats>().useDuration();
        weaponInHand.GetComponent<weaponStats>().useDuration();
    }

    public void BreakArmor()
    {
        Inventory.BreakArmor();
        for (int i = 0; i < 3; i++)
        {
            Torsos[i].sprite = DefaultTorsos[i];
        }
    }

    public void BreakWeapon()
    {
        Inventory.BreakWeapon();
        GetComponent<AnimatorScript>().ResetStates();
        GetComponent<AnimatorScript>().Type = EquippedTool.Type;
        transform.GetComponent<FxScript>().Default();
        Hand.EmptyHand();
    }

    void EquipArmor()
    {
        if (Inventory.EquipData.Armor != null)
        {
            transform.Find("d_c_torso").GetComponent<SpriteRenderer>().sprite = Inventory.EquipData.Armor.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            transform.Find("s_c_torso").GetComponent<SpriteRenderer>().sprite = Inventory.EquipData.Armor.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
            transform.Find("u_c_torso").GetComponent<SpriteRenderer>().sprite = Inventory.EquipData.Armor.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite; 
        }
        else
        {
            BreakArmor();
        }
    }
    void RefreshHand()
    {
        if (Inventory.Changed == true)
        {
            if (Inventory.EquipData.Tool != null)
            {
                if (Hand.Copy == null)
                {
                    GetComponent<FxScript>().FxUpdate(Inventory.EquipData.Tool.GetComponent<weaponStats>().weaponEffect);
                    Hand.Equip(Inventory.EquipData.Tool);
                }
                else if (Inventory.EquipData.Tool.name != Hand.Copy.name)
                {
                    GetComponent<FxScript>().FxUpdate(Inventory.EquipData.Tool.GetComponent<weaponStats>().weaponEffect);
                    Hand.Equip(Inventory.EquipData.Tool);
                }
            }
            else { transform.GetComponent<FxScript>().Default(); Hand.EmptyHand(); }

            GetComponent<AnimatorScript>().ResetStates();
            GetComponent<AnimatorScript>().Type = EquippedTool.Type;
            Inventory.Changed = false;
        }
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
    void InventoryInput()
    {
        
        if(Input.GetKey("f") == true)
        {
            for (int i = 1; i <= InventorySize; i++)
            {
                if (Input.GetKeyDown(i.ToString()) == true) { Inventory.DropItem(i - 1); }
            }
        }
        else
        {
            for (int i = 1; i <= InventorySize; i++)
            {
                if (Input.GetKeyDown(i.ToString()) == true) { Inventory.EquipItem(i - 1); }
            }
        }
    }

    public class CanvasController
    {
        private Transform HpCanvas;
        private GameObject Sword;
        private float Length;

        private bool ArmorImage { set { HpCanvas.Find("Armor").GetComponent<Image>().enabled = value; } get { return ArmorImage; } }

        public CanvasController()
        {
            HpCanvas = GameObject.Find("Canvas 2").transform.GetChild(1);
            Sword = HpCanvas.GetChild(2).GetChild(0).gameObject;
        }

        public void ToggleArmorImage(bool Toggle)
        {
            ArmorImage = Toggle;
        }

        public IEnumerator TakeDamage(int Damage)
        {
            Sword.SetActive(true);
            Sword.GetComponent<Animator>().Play("DamageToArmor");
            yield return new WaitForSeconds(0.4f);
            Sword.SetActive(false);
        }
    }


    /*WHEN TIME, TRANSFER DEFAULT METHODS TO THIS CLASS*/
    public class ItemManager
    {

        private Transform Hand;
        public int Handstate = 0;
        public GameObject Copy;

        public ItemManager(Transform _Hand) { Hand = _Hand; } //default builder requires atleast 1 hand at the start

        public void Equip(GameObject Item)
        {
            if (Hand.transform.childCount > 0)
            {
                EmptyHand();
            }

            Copy = Instantiate(Item) as GameObject;
            Copy.SetActive(true);
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
                Quaternion rotation = Quaternion.Euler(0, 0, 103.594f);
                
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
                Copy.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (Copy.GetComponent<Ranged>() != null) { Copy.GetComponent<Ranged>().Reposition(Hand); }
            if (Copy.GetComponent<longMelee>() != null) { Copy.GetComponent<longMelee>().Reposition(Hand); }
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
                    Copy.GetComponent<SpriteRenderer>().enabled = true;
                    switch (Hand.transform.name)
                    {
                        case "s_l_hand":
                        {
                            Quaternion rotation = Quaternion.Euler(0, 0, -90);
                            Copy.transform.position = Hand.position;
                            Copy.transform.localRotation = rotation;
                            Copy.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                            Copy.GetComponent<SpriteRenderer>().sortingOrder = 20;
                            if (Copy.GetComponent<Ranged>() != null) { Copy.GetComponent<Ranged>().Reposition(Hand); }
                            if (Copy.GetComponent<longMelee>() != null) { Copy.GetComponent<longMelee>().Reposition(Hand); }
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
                            if (Copy.GetComponent<Ranged>() != null) { Copy.GetComponent<Ranged>().Reposition(Hand); }
                            if (Copy.GetComponent<longMelee>() != null) { Copy.GetComponent<longMelee>().Reposition(Hand); }
                            break;
                        }
                        case "d_r_hand":
                        {
                            Quaternion rotation = Quaternion.Euler(0, 0, 103.594f);
                            Copy.transform.SetParent(Hand);
                            Copy.transform.position = Hand.position;
                            Copy.transform.localRotation = rotation;
                            Copy.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                            Copy.GetComponent<SpriteRenderer>().sortingOrder = 16;
                            if (Copy.GetComponent<Ranged>() != null) { Copy.GetComponent<Ranged>().Reposition(Hand); }
                            if (Copy.GetComponent<longMelee>() != null) { Copy.GetComponent<longMelee>().Reposition(Hand); }
                            break;
                        }
                    }
                }
            }
        }
    }
}