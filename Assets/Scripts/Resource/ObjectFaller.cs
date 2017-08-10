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
    private static int _playerLayer;

    private float _escapeTimer;
    private static readonly float _escapeTreshold = 0.2f;

    public void Start()
    {
        _playerLayer = LayerMask.NameToLayer("Player");
        _escapeTimer = Time.time + _escapeTreshold;
    }

    public void StartFreeFall(float timer)
    {
        StartCoroutine(FallToGround(timer, new Vector2(-1, 1)));
        body = GetComponent<Rigidbody2D>();
        rotation = Random.Range(-4f, 4f);

    }

    public void StartFreeFall(float timer, Vector2 maxDirections)
    {
        StartCoroutine(FallToGround(timer, maxDirections));
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

    IEnumerator FallToGround(float timer, Vector2 maxDirections)
    {
        var body = gameObject.AddComponent<Rigidbody2D>();
        Vector2 randomForceDircetion = new Vector2(Random.Range(maxDirections.x, maxDirections.y), Random.Range(0f, 1f)).normalized;
        body.AddForce(randomForceDircetion * Random.Range(450f, 600f));

        yield return new WaitForSeconds(timer);

        Destroy(body);
        Destroy(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time > _escapeTimer)
        {
            if (other.gameObject.layer == _playerLayer)
            {
                Destroy(body);
                Destroy(this);
            }
        }
    }
}













