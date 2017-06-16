using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonScript : MonoBehaviour {


    private bool inventory_cd = false;
    public GameObject TextObject;
	// Use this for initialization
	void Start()
    {
       GetComponent<Button>().onClick.AddListener(inventory);
	}
	
	// Update is called once per frame
	void Update()
    {
        
    }

    void inventory()
    {

        switch (TextObject.activeSelf)
        {
            case true:
                {
                    TextObject.SetActive(false);
                    break;
                }
            case false:
                {
                    TextObject.SetActive(true);
                    break;
                }

        }
    }
}
