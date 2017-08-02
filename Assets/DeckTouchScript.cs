using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckTouchScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        Touch[] touches = Input.touches;


        for (int i = 0; i < touches.Length; i++)
        {
            var touch = touches[i];
            if (touch.phase == TouchPhase.Began)
            {
               // reikast
            }
        }
    }
}
