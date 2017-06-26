using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{

    public GameObject GameObject;
    private TrailRenderer trail;

	void Start ()
	{
	    trail = GetComponent<TrailRenderer>();
	}
	
	void Update ()
    {
	}
}
