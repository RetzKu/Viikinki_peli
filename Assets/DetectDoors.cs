using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDoors : MonoBehaviour {
    private Rigidbody2D body;
    float doorDistance = 1f;
    bool onTop = false;
    LayerMask mask;
    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
        mask = LayerMask.GetMask("Door");
        StartCoroutine(UpdateDoor());
    }

    // Update is called once per frame
    IEnumerator UpdateDoor () {
        for (;;)
        {
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
            yield return new WaitForSeconds(0.3f);
        }
    }
}
