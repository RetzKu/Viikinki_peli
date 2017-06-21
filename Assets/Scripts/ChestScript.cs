using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestScript : MonoBehaviour {

    private TextMesh ChestText;
    private Button ChestButton;

    private Vector2 ChestLocation;
    private Vector2 PopUp;

    private GameObject ChestParent;

    void Start()
    {
        ChestLocation = transform.position;
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D Player)
    { 
        if(Player.gameObject.tag == "Player")
        {
            Debug.Log("Triggered");
            AddTextObject();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(ChestText);
        Destroy(ChestButton);
    }
    
    void AddTextObject()
    {
        ChestText = transform.GetChild(0).gameObject.AddComponent<TextMesh>();
        ChestText.text = "Kukkuu"; ChestText.characterSize = 0.1f; ChestText.fontSize = 40;
        ChestButton = ChestText.gameObject.AddComponent<Button>();
        ChestButton.onClick.AddListener(ButtonFunction);

        //Instantiate(ChestPopUp, transform.GetChild(0)).transform.position = ChestParent.transform.position;
        print("luotu");
    }
    void ButtonFunction()
    {
        Debug.Log("buttonia painettu");
    }
}
