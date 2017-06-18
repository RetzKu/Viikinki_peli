using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avaibleItems : MonoBehaviour {

    public GameObject[] items;
    private int itemCount = -1;
    private int luku = 0;

    // Use this for initialization
    void Start () {

        System.IO.DirectoryInfo myDir = new System.IO.DirectoryInfo("Assets/Resources/Items/");

        luku = myDir.GetFiles().Length; // Laskee montako tavaraa on kansiossa Resources/Items/

        items = new GameObject[luku];

        luku = luku / 2;

        Debug.Log(luku);

        items = Resources.LoadAll<GameObject>("Items"); // Lataa kaikki items kansiossa olevat prefabit


        //Instantiate(items[0], transform.parent);

        
    }
	
	// Update is called once per frame
	void Update () {


        if(Input.GetKeyDown(KeyCode.Q) == true)
        {
          
            Destroy(GameObject.FindGameObjectWithTag("item2"));
 

            if(itemCount < (luku-1))
            {
                itemCount++;
            }
           
            Instantiate(items[itemCount], transform.parent);
            
        }
        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            
            Destroy(GameObject.FindGameObjectWithTag("item2"));
            

            if (itemCount >= 0)
            {
                itemCount--;
            }

            if(itemCount >= 0)
            {
                Instantiate(items[itemCount], transform.parent);
            }
        }
    }
}
