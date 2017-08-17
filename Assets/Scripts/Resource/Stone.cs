using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stone : Resource
{
    //[System.Serializable]
    //public static Sprite[] Sprites;
    public override void Init(bool destroyed)
    {
        dead = destroyed;
    }

    public override void OnDead()
    {
        // TODO: ERIKOISTA
        GetComponent<DropScript>().Drop();

        // ObjectPool.instance.PoolObject(this.gameObject);
        // transform.gameObject.gameObject.SetActive(false);
        StartCoroutine(FadeAway(deathTimer));
    }

    private Vector2 impact = Vector2.zero;
    public void AddImpact(Vector2 force)
    {
        var dir = force.normalized;
        impact += dir.normalized * force.magnitude / 1.0f;//mass
    }

    private IEnumerator FadeAway(float totalTime)
    {
        var fader = gameObject.AddComponent<Fader>();
        fader.StartFading(totalTime, 0f, GetComponent<SpriteRenderer>());

        yield return new WaitForSeconds(totalTime);
        gameObject.SetActive(false);

        bool looping = true;
        int i = 0;
    }

    Action foo()
    {
        return () => { Debug.Log("hello"); };
    }
    

    void Update()
    {
        // if (Input.GetMouseButtonDown(1))
        // {
            // Hit(25);
            // AddImpact((Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameObject.FindWithTag("Player").transform.position).normalized * 35f);
        // }

        // if (impact.magnitude > 0.2)
        // {
        // }
        // impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
        // transform.Translate(impact * Time.deltaTime);
    }
}
