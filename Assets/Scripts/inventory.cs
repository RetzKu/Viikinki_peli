using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class inventory
{
    [SerializeField]
    public List<GameObject> InventoryData;
    public Equipped EquipData;
    public int InventorySize;

    public inventory(int _InventorySize) { InventorySize = _InventorySize; InventoryData = new List<GameObject>(InventorySize); EquipData = new Equipped(); }

    internal bool Changed = false;
    internal bool ArmorEquipped = false;

    public void AddToInventory(GameObject Item)
    {
        if (InventoryData.Count < InventorySize)
        {
            InventoryData.Add(Item);
            if (Item.GetComponent<armorScript>() != null)
            {
                if (EquipData.Armor == null)
                {
                    EquipData.SetArmor(Item);
                    ArmorEquipped = true;
                }
            }
            else
            {
                if (EquipData.Tool == null)
                {
                    EquipData.SetTool(Item);
                    Changed = true;
                }
            }
            Item.SetActive(false);
        }
    }

    public void BreakArmor() { EquipData.BreakArmor(); }
    public void BreakWeapon()
    {
    GameObject tempObject = EquipData.BreakWeapon();    
        foreach (GameObject t in InventoryData)
        {
            if(t.name == tempObject.name)
            {
                GameObject.Find("Deck1").GetComponent<DeckScript>().lastBrokenWeapon(InventoryData.FindIndex(a => a.name == t.name));
                InventoryData.RemoveAt(InventoryData.FindIndex(a => a.name == t.name));
                break;
            }
        }
    }

    public void EquipItem(int Slot)
    {
        GameObject Item = InventoryData[Slot];

        if (EquipData.EquippedType(Item) == WeaponType.Armor)
        {
            EquipData.SetArmor(Item);
            ArmorEquipped = true;
        }
        else
        {
            EquipData.SetTool(Item);
            Changed = true;
        }
    }

    public void DropItem(int slot)
    {
        GameObject Item = InventoryData[slot];

        if (EquipData.Armor != null)
        {
            if (Item.name == EquipData.Armor.name)
            {
                EquipData.SetArmor(null);
                ArmorEquipped = true;
            } 
        }
        if(EquipData.Tool != null)
        {
	        if (Item.name == EquipData.Tool.name)
            {
                EquipData.SetTool(null);
                Changed = true;
            } 
        }
        Item.SetActive(true);
        Item.transform.position = GameObject.Find("Player").transform.position;
        Item.tag = "Dropped";

        InventoryData.RemoveAt(slot);
    }

    [System.Serializable]
    public class Equipped
    {

        [SerializeField]
        private GameObject _ChestPiece;
        [SerializeField]
        private GameObject _Tool;
        [SerializeField]
        private int _ArrowCount;


        public GameObject Tool { get { return _Tool; } }
        public WeaponType Type { get { return EquippedType(); } }

        public GameObject Armor { get { return _ChestPiece; } }

        public int ArrowCount { get { return _ArrowCount; } set { _ArrowCount = value; } }

        public Equipped() { _ChestPiece = null; _Tool = null; }

        public GameObject SwapItem(GameObject Item)
        {
            GameObject ReturnedItem;
            ReturnedItem = _Tool;
            _Tool = Item;
            return ReturnedItem;
        }

        public WeaponType EquippedType()
        {
            if (Tool != null)
            {
                if (Tool.GetComponent<Ranged>() != null) { return WeaponType.rangedWeapon; }
                else if (Tool.GetComponent<longMelee>() != null) { return WeaponType.longMeleeWeapon; }
                else if (Tool.GetComponent<Melee>() != null) { return WeaponType.meleeWeapon; }
                else { return WeaponType.noWeapon; }

            }
            else { return WeaponType.noWeapon; }
        }
        public WeaponType EquippedType(GameObject Item)
        {
            if (Item.GetComponent<Ranged>() != null) { return WeaponType.rangedWeapon; }
            else if (Item.GetComponent<longMelee>() != null) { return WeaponType.longMeleeWeapon; }
            else if (Item.GetComponent<Melee>() != null) { return WeaponType.meleeWeapon; }
            else if (Item.GetComponent<armorScript>() != null) { return WeaponType.Armor; }
            else { return WeaponType.noWeapon; }
        }
        public void SetArmor(GameObject Armor) { _ChestPiece = Armor; }

        public void BreakArmor() { _ChestPiece = null; }
        public GameObject BreakWeapon() { GameObject RemovedTool = _Tool; _Tool = null; return RemovedTool; }

        public GameObject EmptyHand() { GameObject RemovedTool = _Tool; _Tool = null; return RemovedTool; }
        public void SetTool(GameObject Tool) { _Tool = Tool; }
        public bool UsedArrow() { if (ArrowCount > 1) { ArrowCount -= 1; return true; } else { return false; } }
    }
}
