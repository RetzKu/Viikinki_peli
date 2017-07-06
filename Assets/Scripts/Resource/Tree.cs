using UnityEngine;
using Random = UnityEngine.Random;

public class Tree : Resource
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hit(50);
        }
    }

    public override void OnDead()
    {
        StartFalling();
    }

    // NOTE: Puilla täytyy olla oikea rigidbody setup toimiakseen!!!
    // ks Pine
    void StartFalling()
    {
        var capsule = GetComponentInChildren<CapsuleCollider2D>();
        if (capsule != null)
        {
            capsule.enabled = true;
        }
        var box = GetComponentInChildren<BoxCollider2D>();
        if (box != null)
        {
            box.enabled = true;
        }

        // var go = GetComponentInChildren<Rigidbody2D>();
        // go.simulated = true;
        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = true;
        }

        foreach (var body in GetComponentsInChildren<Rigidbody2D>())
        {
            body.simulated = true;
            if (body.bodyType == RigidbodyType2D.Dynamic)
            {
                float rot = Random.Range(-2f, 2f);
                body.transform.Rotate(new Vector3(0f, 0f, rot));

                // Rigidbody2D fallingTreeRigidbody2D = body;
            }
        }

        // gameObject.SetActive(false);     // todo: pool // mutta puilla on hyvin oma sydeemi bodyen kannsa 
        transform.DetachChildren();

        // gameObject.SetActive(false);
        // Destroy(gameObject);                // todo: disable / pool
    }
}
