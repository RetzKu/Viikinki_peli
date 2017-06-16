using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonScript : MonoBehaviour {


    private bool inventory_cd = false;
    private GameObject TextObject;
    private GameObject Hud;

    void Start()
    {
        GameObject Hud = GameObject.Find("Hud");
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
