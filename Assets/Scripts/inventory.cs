using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour {

    public List<GameObject> Tools;
    internal GameObject ChestPiece;

	void Start ()
    {
        Tools = new List<GameObject>();
    }

    void Update()
    {

    }
    
    internal void AddItem(GameObject Item)
    {
        Tools.Add(Item);
        Destroy(Item);
    }
}
