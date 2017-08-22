using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DropScript))]

public class Fish : MonoBehaviour
{
    public GameObject _waterEffect;

    private Rigidbody2D _body;
    private Vector2 _startPosition;
    private readonly float startOffsetY = -0.6f;

    public void Start()
    {
        Init();
        var go = Instantiate(_waterEffect);
        go.transform.position = transform.position;
    }

    public void Init()
    {
        _body = GetComponent<Rigidbody2D>();
        _startPosition = transform.position; 
        _startPosition.y += startOffsetY;

        // GetComponent<SpriteRenderer>().sprite = Corpse.CorpseSprites[Random.Range(0, Corpse.CorpseSprites.Length - 1)];
        transform.localScale = new Vector3(2f, 2f, 2f);
    }

    public void Launch(Vector2 force)
    {
        // _body.AddForce(force, ForceMode2D.Impulse);
        // var laucher = gameObject.AddComponent<ObjectFaller>();
        // laucher.StartFreeFall(3f);

        _body.AddForce(new Vector2(Random.Range(-3f, 3f), 8f), ForceMode2D.Impulse);
    }

    void Update()
    {
        var fishDirection = _body.velocity.normalized;
        transform.right = -fishDirection;

        // monnit oikein päin!
        transform.localScale = fishDirection.x >= 0f ? new Vector3(2f, -2f, 2f) : new Vector3(2f, 2f, 2f);

        if (transform.position.y <= _startPosition.y)
        {
            var go = Instantiate(_waterEffect);
            go.transform.position = transform.position;
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        // drop
        // if (other.gameObject.layer)
        // {
        // }
        //if (other.gameObject.layer)
        //{
        //}
      
    }

    public void Hit()
    {
        // lennä ja jotain
        GetComponent<DropScript>().Drop();
    }
}
