using System.Collections;
using UnityEngine;


public class TreeHP : MonoBehaviour
{
    public int hp = 100;

	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (hp <= 0 || Input.GetKeyDown(KeyCode.Space))
		{
		    StartFalling();
		}
	}

    // hyvin puu spesifistä koodia !
    // toimii vain puilla joilla on prefabs
    void StartFalling()
    {
        GetComponentInChildren<CapsuleCollider2D>().enabled = true;
        GetComponentInChildren<BoxCollider2D>().enabled = true;
        var go = GetComponentInChildren<Rigidbody2D>();
        go.simulated = true;
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
                Rigidbody2D fallingTreeRigidbody2D = body;
                StartCoroutine(AnimateTreeFalling(fallingTreeRigidbody2D)); // no workings
            }
        }

        // gameObject.SetActive(false);    // todo: pool
        transform.DetachChildren();
        Destroy(gameObject);                // todo: disable / pool
    }

    IEnumerator AnimateTreeFalling(Rigidbody2D body)
    {
        yield return new WaitForSeconds(3.5f);
        Destroy(body.gameObject);
    }
}
