﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffParticle : MonoBehaviour {

    bool inited = false;
    float time = 0;
    float timer = 0;
    Quaternion q;
    void Start()
    {
        //GetComponent<ParticleSystem>().Pause();
    }
    void Update ()
    {
        transform.rotation = q;
        if (inited)
        {
            timer += Time.deltaTime;
            if(timer > time)
            {
                Destroy(this.gameObject);
            }
        }
	}
    //public void res
    public void init(GameObject FatherMichael,float time)
    {
        q = transform.rotation;
        print(GetComponent<ParticleSystem>().isPlaying);
        transform.parent =  FatherMichael.transform;
        transform.position = FatherMichael.transform.position;
        this.time = time;
        inited = true;
    }
    public void resetTime(float time)
    {
        timer = 0;
        this.time = time;
    }

}
