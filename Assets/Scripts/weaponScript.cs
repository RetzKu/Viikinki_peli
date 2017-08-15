using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponScript : MonoBehaviour
{
    public GameObject[] items;
    private int itemCount = -1;
    private int luku = 0;

    // Use this for initialization
    void Start()
    {
        System.IO.DirectoryInfo myDir = new System.IO.DirectoryInfo("Assets/Resources/Items/");
        luku = myDir.GetFiles().Length; 
        // Laskee montako tavaraa on kansiossa Resources/Items/
        items = new GameObject[luku];
        luku = luku / 2;
        //Debug.Log(luku);
        items = Resources.LoadAll<GameObject>("Items"); // Lataa kaikki items kansiossa olevat prefabit
        
            
    }

    // Update is called once per frame
    void Update()
    {

    }
}
