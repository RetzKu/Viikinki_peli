using System.Collections;
using System.Collections.Generic;
using DarkConfig;
using UnityEngine;

public class ConfigInit : MonoBehaviour {

	// Use this for initialization
	void Awake () {

        UnityPlatform.Setup();
        Config.FileManager.AddSource(new ResourcesSource(hotload: true));
        Config.Preload();
	    Config.OnPreload += () =>
	    {
	        // load the rest of the game here
	    };
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
