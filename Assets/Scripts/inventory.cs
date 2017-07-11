using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class inventory
{
    public List<GameObject> Inventory;
    public int InventorySize;
    public Equipped EquipInventory;

    
    public inventory(int _InventorySize) { InventorySize = _InventorySize; Inventory = new List<GameObject>(InventorySize); EquipInventory = new Equipped();}

    public void AddToInventory(GameObject Item)
    {
        if(EquipInventory.EquippedTool() == null) { EquipInventory.SetTool(Item); }
        else if(Inventory.Count < InventorySize) { Inventory.Add(Item); }
        Item.SetActive(false);
    }

    public void EquipItem(int Slot)
    {
        GameObject Item = Inventory[Slot];
        Inventory[Slot] = EquipInventory.SwapItem(Item);
    }

    public void DropItem()
    {

    }

    public class Equipped
    {
        private GameObject _ChestPiece;
        private GameObject _Tool;

        public Equipped() { _ChestPiece = null; _Tool = null; }

        public GameObject SwapItem(GameObject Item)
        {
            GameObject ReturnedItem;

            ReturnedItem = _Tool;
            _Tool = Item;

            return ReturnedItem;
        }
        public GameObject EquippedTool() { return _Tool; }
        public void SetTool(GameObject Tool) { _Tool = Tool; }
    }
}
