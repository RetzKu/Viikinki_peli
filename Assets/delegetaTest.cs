using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delegetaTest : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //BaseManager.Instance.registerOnBaseEnter(a);		
        //BaseManager.Instance.registerOnBaseExit(b);		
        //BaseManager.Instance.registerOnBaseExit(c);		
	}

    void a()
    {
        print("hello from a");
    }

    void b()
    {
        print("hello from b");
    }

    void c()
    {
        print("C > C#");
    }


    // Update is called once per frame
	void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    BaseManager.Instance.unRegisterOnBaseExit(b);
        //}
	}
}
