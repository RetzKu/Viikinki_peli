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

    public inventory(int _InventorySize) { InventorySize = _InventorySize; InventoryData = new List<GameObject>(InventorySize); EquipData = new Equipped();}

    internal bool Changed = false;

    public void AddToInventory(GameObject Item)
    {
        if(EquipData.Tool == null) { EquipData.SetTool(Item); Item.SetActive(false); Changed = true; }
        else if(InventoryData.Count < InventorySize) { InventoryData.Add(Item); Item.SetActive(false); }
    }

    public void EquipItem(int Slot)
    { 
        if (InventoryData.ElementAtOrDefault(Slot) != null)
        {
            GameObject Item = InventoryData[Slot];
            GameObject ReturnedItem = EquipData.SwapItem(Item);
            if(ReturnedItem == null) { InventoryData.RemoveAt(Slot); }
            else { InventoryData[Slot] = ReturnedItem; }
            Changed = true;
        }
    }

    public void DropItem()
    {
        GameObject DroppedTool = EquipData.EmptyHand();
        if (DroppedTool != null)
        {
            DroppedTool.transform.position = GameObject.Find("Player").transform.position;
            DroppedTool.SetActive(true);
            DroppedTool.tag = "Dropped";
            Changed = true;
        }
    }

    [System.Serializable]
    public class Equipped
    {       
        public enum WeaponType { noWeapon, meleeWeapon, longMeleeWeapon, rangedWeapon }
       
        [SerializeField]
        private GameObject _ChestPiece;
        [SerializeField]
        private GameObject _Tool;

        public GameObject Tool { get { return _Tool; } }


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
            if(Tool != null)
            {
                if (Tool.GetComponent<Ranged>() != null) { return WeaponType.rangedWeapon; }
                else if (Tool.GetComponent<longMelee>() != null) { return WeaponType.longMeleeWeapon; }
                else if (Tool.GetComponent<Melee>() != null) { return WeaponType.meleeWeapon; }
                else { return WeaponType.noWeapon; }
                
            }
            else { return WeaponType.noWeapon; }
        }
        public GameObject EmptyHand() { GameObject RemovedTool = _Tool; _Tool = null; return RemovedTool; }
        public void SetTool(GameObject Tool) { _Tool = Tool; }
    }
}
