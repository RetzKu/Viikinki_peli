using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

// vaikka joku oop hökötys joskus
public enum ResourceType
{
    Tree,
    Stone,
}

public class TreeHP : MonoBehaviour
{
    public int hp = 100;
    public ResourceType type;
    private static readonly float TileWidth = 1f;
    public static GameObject EffectEmitters = null;    // pool

    void Start()
    {
        if (EffectEmitters == null)
        {
            // EffectEmitters = GameObject.FindGameObjectWithTag("ResourceEffectEmitter");
        }
    }
	
    // Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //OnDied();
            StartCoroutine(Fibrate(3f, TileWidth / 35f));
            //StartCoroutine(EmitParticles(transform.position, 0f, 2f));
        }
    }

    void OnDied()
    {
        if (hp <= 0)
        {
            switch (type)
            {
                case ResourceType.Tree:
                    StartFalling();
                    break;
                case ResourceType.Stone:
                    break;
                default:
                    Debug.LogWarning("UNIMPLEMENTED ResourceType: " + type.ToString() + "in TreeHP.cs / resource.cs");
                    break;
            }
        }
    }

    // objectlayeriin ja combatit katsoo layeriin
    void GetHit(GameObject hitter)
    {
        // TODO: kun implementattu
        //  hitter.GetComponent<Combat>()   
        // ota statsit
        // GetDamage();

        hp -= 50;
        if (hp <= 0)
        {
            OnDied();
        }
        else
        {
           StartCoroutine(Fibrate(3f, TileWidth / 25f));
        }
    }
    
    // lisää body aloita kaatuminen
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

    IEnumerator Fibrate(float seconds, float fibrationRange)  // TODO: MIETI iteraatiot oikein perf
    {
        int iterations = 45;
        float waitTime = seconds / iterations;

        Vector3 startingPosition = transform.position;

        for (int i = 0; i < iterations; i++)
        {
            if (i % 2 == 0) // värise
            {
                transform.Translate(Random.Range(-fibrationRange, fibrationRange), Random.Range(-fibrationRange, fibrationRange), 0f);
            }
            else
            {
                transform.Translate(Random.Range(-fibrationRange, fibrationRange), Random.Range(-fibrationRange, fibrationRange), 0f);
            }
            yield return new WaitForSeconds(waitTime);
        }

        transform.position = startingPosition;
    }

    IEnumerator EmitParticles( Vector3 hitPosition, float intensity, float seconds)
    {
        EffectEmitters.transform.position = hitPosition; // HIT KOHTAAN halutaan
        var particleSystem = EffectEmitters.GetComponent<ParticleSystem>();
        particleSystem.Play();

        yield return new WaitForSeconds(seconds);   // Voi hoitaa kokonaan ParticleSystemissä
        particleSystem.Stop();
    }
}
   