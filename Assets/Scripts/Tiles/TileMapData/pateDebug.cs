using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pateDebug : MonoBehaviour {

    // Use this for initialization
    Rigidbody2D body;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("o"))
        {
            print("knockingback");
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 temp = (Vector2)pz - body.position;
            GetComponent<Movement>().KnockBack(pz);
           // GetComponent<Movement>()._Lock = true;
        }
    }
}
