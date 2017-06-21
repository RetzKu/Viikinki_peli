using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class tilemapTearingFix : MonoBehaviour
{
    public GameObject Player;

    void Start ()
    {
    }
	
    void Update ()
    {
	}

    void FixedUpdate()
    {
        // transform.position = Player.transform.position;
        //  transform.position = new Vector3((Mathf.RoundToInt(gameObject.transform.position.x)), 
            // (Mathf.RoundToInt(gameObject.transform.position.y)), 
            // (Mathf.RoundToInt(gameObject.transform.position.z)));

        // transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
}
