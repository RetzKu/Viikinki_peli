using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdList : MonoBehaviour {

    private List<GameObject> list;
	void Start ()
    {
        list = new List<GameObject>(Resources.LoadAll<GameObject>("ItemPrefabs"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
