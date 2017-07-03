using System.Collections;
using System.Collections.Generic;
using DarkConfig;
using UnityEngine;

[System.Serializable]
public class Toope
{
    public string nimi;
    public int raha;
}

public class DarkConfigTutorial : MonoBehaviour
{
    public string hei;

    public Toope instance;
	// Use this for initialization

	void Start ()
	{
	    Config.ApplyThis("player", this);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    // print("olen " + instance.nimi + " ja minulla on " + instance.raha + " rahaa!");
        // print(hei);
	}
}
