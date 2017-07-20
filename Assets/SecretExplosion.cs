using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretExplosion : MonoBehaviour {

    public float fadetime = 3f;
    float fadeTimer = 0f;
    bool faded = false;
    SpriteRenderer sp;
    public ParticleSystem ex;
    bool inited = false;
	// Use this for initialization

	public void init(Vector2 position,float castTime)
    {
        transform.position = position;
        sp = GetComponent<SpriteRenderer>();
        fadetime = castTime;
        inited = true;
    }
	// Update is called once per frame
	void Update () {
        if (inited)
        {
            if (!faded)
            {
                fadeTimer += Time.deltaTime;
                if(fadeTimer > fadetime)
                {
                    faded = true;
                    print("FADED");
                    ex = Instantiate(ex);
                    ex.transform.position = transform.position;
                    ex.transform.parent = transform; 
                    fadeTimer = 0f;
                }
                else
                {
                    float t = fadeTimer / fadetime;
                    //print(t);
                    Color temp =sp.color;
                    temp.a = t;
                    sp.color = temp;
                }
            }
            else if (faded)
            {
                fadeTimer += Time.deltaTime * 2;
                if(fadeTimer > fadetime)
                {
                    Destroy(this.gameObject,4f);
                }
                else
                {
                    float t = fadeTimer / fadetime;
                    print(t);
                    Color temp = sp.color;
                    temp.a = 1f - t;
                    sp.color = temp;

                }
            }
        }
	}
}
