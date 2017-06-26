using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDoors : MonoBehaviour {
    private Rigidbody2D body;
    float doorDistance = 1f;
    bool onTop = false;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        LayerMask mask = LayerMask.GetMask("Door");
        var array = Physics2D.OverlapCircleAll(body.position, doorDistance, mask); // , mask);
        if(array.Length > 0)
        {
            //print("DETECTING DOOR");
            if (!onTop)
            {
                array[0].transform.GetComponent<door>().Activate();
                onTop = true;
            }
        }
        else
        {
            //print("NOT DETECTING DOOR");
            onTop = false;
        }
    }
}
