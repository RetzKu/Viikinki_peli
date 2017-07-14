using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZLayerUpdate : MonoBehaviour
{
	void Update ()
    {
        float z = ZlayerManager.GetZFromY(transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
	}
}
