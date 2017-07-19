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
            ParticleSpawner.instance.CastSpell(this.gameObject);
        }
    }
}
