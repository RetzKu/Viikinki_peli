using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 velocity;
    public float Speed { get; set; }

    void Update()
    {
        transform.Translate(velocity * Time.deltaTime * 10);

        if (transform.position.x < 0 || transform.position.x > 200 || transform.position.y < 0 ||
            transform.position.y > 200)
        {
            Destroy(this.gameObject);
        }
    }
}