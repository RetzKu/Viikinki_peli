using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        ObjectPool.instance.PoolObject(this.gameObject);
    }

    private Vector2 impact = Vector2.zero;
    public void AddImpact(Vector2 force)
    {
        var dir = force.normalized;
        impact += dir.normalized * force.magnitude / 1.0f;//mass
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Hit(25);
            // AddImpact((Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameObject.FindWithTag("Player").transform.position).normalized * 35f);
        }

        // if (impact.magnitude > 0.2)
        // {
            transform.Translate(impact * Time.deltaTime);
        // }
        // impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

    }
}
