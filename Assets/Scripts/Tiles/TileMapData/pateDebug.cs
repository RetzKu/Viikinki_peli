using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pateDebug : MonoBehaviour {

    // Use this for initialization
    Rigidbody2D body;
    public GameObject blood;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("o"))
        {
            print("splattering");
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var newBlood = Instantiate(blood, new Vector2(0, 0), Quaternion.identity);
            newBlood.GetComponent<destroyMe>().initParticle(pz,body.position);
        }
    }
}
