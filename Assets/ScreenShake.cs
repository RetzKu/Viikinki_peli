using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {
    public GameObject Camera;
    Vector3 orgpos = new Vector3();
    bool shaking = false;
    int times = 5;
    int timCount = 0;
    float time = 0.01f;
    float timer = 0f;
    
    void Update()
    {
        if(shaking)
        {
            timer += Time.deltaTime;
            if(timer > time && timCount < times)
            {
                Camera.transform.localPosition = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Camera.transform.position.z);
                print("SHAKE");
                timer = 0;
                timCount++;

            }
            else if(timCount >= times)
            {
                shaking = false;
                timer = 0;
                timCount = 0;
                Camera.transform.localPosition = orgpos;
            }
        }
        
    }
	
    public void Shake()
    {
        if (!shaking)
        {
            orgpos = Camera.transform.localPosition;
            shaking = true;
            timer = 0;
            timCount = 0;
        }

    }
}
