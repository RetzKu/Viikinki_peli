using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Movement : MonoBehaviour
{
    private Vector2 movement;
    public float divider;
    public Vector2 destination;
    public float slowdown;

    void Start()
    {
        destination = transform.position;
    }

    // Update is called once per frame

    void Update()
    {
    }

    void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, Input_checker(),slowdown * Time.deltaTime);
    }

    Vector2 Input_checker()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        movement = movement / divider;
        destination += movement;
        return destination;
    }

}
