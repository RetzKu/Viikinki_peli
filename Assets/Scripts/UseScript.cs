using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UseScript : MonoBehaviour {

    private Button btn;
	// Use this for initialization
	void Start ()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(Use);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    void Use()
    {
        Debug.Log("Item used");
    }
 
}
