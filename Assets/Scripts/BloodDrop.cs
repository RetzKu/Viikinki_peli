using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodDrop : MonoBehaviour {

    private float t = 0;
    [SerializeField]
    private float Speed = 0.5f;

    void Start()
    {
        gameObject.AddComponent<Rigidbody2D>().gravityScale = 10f;
        GetComponent<Rigidbody2D>().mass = 10f;
        DestroyObject(GetComponent<Rigidbody2D>(), 3.5f);
    }

    // Update is called once per frame
    void FixedUpdate () {
        t += Time.deltaTime * Speed;
        GetComponent<Image>().color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, t));
    }
}
