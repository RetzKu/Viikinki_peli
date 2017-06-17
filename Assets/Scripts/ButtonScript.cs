using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    private GameObject InventoryObject;
    private GameObject Ui;
    private Button InventoryButton;

    private bool inventory_cd;

    void Start()
    {
        Ui = GameObject.FindGameObjectWithTag("Ui"); //Finds Gameobject with UI which is main UI element for game
        InventoryObject = Ui.transform.Find("InventoryObject").gameObject; //Finds child element which works as inventory menu for game
        InventoryButton = GetComponent<Button>(); //new button object created
        InventoryButton.onClick.AddListener(ActiveSwitcher); //Make it so each click will call activeswitcher function
        InventoryObject.SetActive(false);
    }
    void Update()
    {
        InventoryToggle();
    }

    void InventoryToggle() // same as button system but made for hotkey i for pc
    {
        if (inventory_cd == false)
        {
            if (Input.GetAxisRaw("Inventory") == 1)
            {
                inventory_cd = true;
                ActiveSwitcher();
            }
        }
        if (Input.GetAxisRaw("Inventory") == 0) { inventory_cd = false; }
    }

    void ActiveSwitcher()
    {
        switch (InventoryObject.activeSelf) //is inventory screen open aka active and switcheruu its state
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
