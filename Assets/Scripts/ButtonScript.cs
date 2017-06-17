using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    private GameObject InventoryObject;
    private GameObject Ui;
    private Button InventoryButton;

    void Start()
    {
        Ui = GameObject.FindGameObjectWithTag("Ui");
        InventoryObject = Ui.transform.Find("InventoryObject").gameObject;
        InventoryButton = GetComponent<Button>();
        InventoryButton.onClick.AddListener(Inventory_toggle);
        InventoryObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update()
    {
        
    }

    void Inventory_toggle()
    {
        bool InventoryButtonBool = InventoryObject.activeSelf;
        switch (InventoryButtonBool)
        {
            case true:
                {
                    InventoryObject.SetActive(false);
                    break;
                }
            case false:
                {
                    InventoryObject.SetActive(true);
                    break;
                }
        }
    }
}
