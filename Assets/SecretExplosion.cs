using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretExplosion : MonoBehaviour {

    float fadetime = 3f;
    float fadeTimer = 0f;
    bool faded = false;
    SpriteRenderer sp;
	// Use this for initialization
	void Start () {
        sp = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!faded)
        {
            fadeTimer += Time.deltaTime;
            if(fadeTimer > fadetime)
            {
                faded = true;
                print("FADED");
            }
            else
            {
                float t = fadeTimer / fadetime;
                print(t);
                Color temp =sp.color;
                temp.a = t;
                sp.color = temp;
            }
        }
	}
}
