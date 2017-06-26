using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour {

    public List<Items> Tools;
    internal GameObject ChestPiece;

	void Start ()
    {
        Tools = new List<Items>();
    }

    void Update()
    {

    }
    
    internal void AddItem(GameObject Item)
    {
        Tools.Add(new Items(Item));
    }
    public class Items
    {
        public GameObject Item;

        public Items(GameObject _Item) { Item = _Item; }
    }
}
