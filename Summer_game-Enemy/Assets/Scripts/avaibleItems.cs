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
      
            if(GameObject.Find("Inventory").transform.childCount >= 1 & GameObject.Find("Equip").transform.childCount < 1)
            {
                Debug.Log(GameObject.Find("Inventory").transform.childCount);
                var objectCache = GameObject.FindGameObjectWithTag("item_inventoryssa").gameObject;
                //objectCache.SetActive(true);
                objectCache.tag = "item2";
                objectCache.transform.position = GameObject.Find("hahmo").transform.position;
                
                
                Instantiate(objectCache, GameObject.Find("Equip").transform);
                Destroy(objectCache);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.E) == true)
        {

            if (GameObject.Find("Equip").transform.childCount >= 1)
            {
                var objectCache2 = GameObject.FindGameObjectWithTag("item2").gameObject;
                objectCache2.tag = "item_inventoryssa";
                //objectCache2.SetActive(false);

                string nameCache = objectCache2.name;
                int copyTest = nameCache.IndexOf("(Clone)");
                if(copyTest != -1)
                {
                    string newName = nameCache.Remove(copyTest);
                    objectCache2.name = newName;
                    copyTest = -1;
                }

                objectCache2.transform.position = new Vector3(0, 0, 0);
                Instantiate(objectCache2, GameObject.Find("Inventory").transform);
                Destroy(objectCache2);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            //GameObject.Find("Inventory").SetActiveRecursively(true);
            
        }
    }
}
