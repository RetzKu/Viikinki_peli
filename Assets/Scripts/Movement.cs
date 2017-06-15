using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Movement : MonoBehaviour
{
    private Vector2 movement;
    public float divider;
    public Vector2 destination;
    public float slowdown;

    Rigidbody2D body;

    // nopeus = vel * kihtyyys  
    // max pituus
    /// <summary>
    ///  
    /// </summary>


    void Start()
    {
        destination = transform.position;
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void Update()
    {
    }

    void FixedUpdate()
    {
        body.MovePosition(Vector2.Lerp(transform.position, Input_checker(), slowdown * Time.deltaTime));
        // transform.position =
    }

    Vector2 Input_checker()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        movement = movement / divider;
        destination += movement;

        float test = Vector2.Distance(destination, body.position);
        if (test > Mathf.Sqrt(2.1f))
        {
            destination = body.position;
            movement.Normalize();
            destination += movement / divider;
        }
        return destination;
    }
}
