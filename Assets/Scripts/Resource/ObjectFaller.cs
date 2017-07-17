using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Dropperille apu luokka 
// TODO: PARAMETRISOI DROPPERILLE
public class ObjectFaller : MonoBehaviour
{
    private Rigidbody2D body;
    private static readonly float maxVelocity = 8f;
    private float rotation;

    public void StartFreeFall(float timer)
    {
        StartCoroutine(FallToGround(timer));
        body = GetComponent<Rigidbody2D>();
        rotation = Random.Range(-4f, 4f);
    }

    public void SetStartForce(Vector2 force)
    {
        body.AddForce(force);
    }

    void Update()
    {
        if (body == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            body.velocity = Vector2.ClampMagnitude(body.velocity, maxVelocity);
            body.transform.Rotate(0f, 0f, rotation, Space.Self);
        }
    }

    IEnumerator FallToGround(float timer)
    {
        var body = gameObject.AddComponent<Rigidbody2D>();
        Vector2 randomForceDircetion = new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 1f)).normalized;
        body.AddForce(randomForceDircetion * 500f);

        yield return new WaitForSeconds(timer);

        Destroy(body);
        Destroy(this);
    }
}
