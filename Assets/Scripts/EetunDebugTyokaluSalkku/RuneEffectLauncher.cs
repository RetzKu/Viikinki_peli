using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class RuneEffectLauncher : MonoBehaviour
{
    //private ParticleSystem _particleSystem;

    void Start()
    {
        //_particleSystem =  GetComponent<ParticleSystem>();
    }

    public void LaunchArrow(Sprite sprite, Vector3 lookAt)
    {
        StartCoroutine(LaunchArrowEffect(sprite, lookAt, transform.position));
    }

    IEnumerator LaunchArrowEffect(Sprite sprite, Vector3 lookAt, Vector3 from)
    {
        GameObject go = new GameObject("arrow :D");
        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.sortingLayerName = "RuneEffects";
        renderer.sprite = sprite;

        go.transform.position = this.transform.position;
        go.transform.localScale = new Vector3(5f, 5f);

        go.transform.up = lookAt - from;

        // fade
        for (int i = 0; i < 40; i++)
        {
            // go.transform.Translate(0f, 0.2f * Time.deltaTime, 0f); 
            go.transform.position += new Vector3(0f, 10f * Time.deltaTime, 0f);
            go.transform.up = lookAt - go.transform.position;
            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine(attack(go, lookAt));
        // Destroy(go);
    }

    IEnumerator attack(GameObject go, Vector3 lookAt)
    {
        for (int i = 0; i < 50; i++)
        {
            go.transform.Translate(new Vector3(0f, 0.4f, 0f));
            go.transform.up = lookAt - go.transform.position;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(go);
    }

    public void Fire(Sprite sprite)
    {
        StartCoroutine(LaunchEffect(sprite));
    }

    IEnumerator LaunchEffect(Sprite sprite)
    {
        GameObject go = new GameObject("Jää runiiddi :D");
        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.sortingLayerName = "RuneEffects";
        renderer.sprite = sprite;
        go.transform.position = this.transform.position;
        // rotato 

        for (int i = 0; i < 60; i++)
        {
            go.transform.localScale = new Vector3(go.transform.localScale.x + 0.10f, go.transform.localScale.y + 0.10f);
            go.transform.Rotate(new Vector3(0, 0, 360 / 60));
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        Destroy(go);
    }


    IEnumerator EmitParticles;
}
