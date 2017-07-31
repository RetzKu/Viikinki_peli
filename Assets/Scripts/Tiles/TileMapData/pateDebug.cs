using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pateDebug : MonoBehaviour {

    // Use this for initialization
    Rigidbody2D body;
    public GameObject door;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("o"))
        {
           // GetComponent<ScreenShake>().Shake();
            //print("shake dat booty");
            //ParticleSpawner.instance.SpawnFireExplosion(Camera.main.ScreenToWorldPoint(Input.mousePosition),Random.Range(1f,2f));
            //ParticleSpawner.instance.CastSpell(this.gameObject);
            door.GetComponent<door>().Activate();
        }
    }
}
