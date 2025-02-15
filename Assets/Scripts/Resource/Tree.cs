﻿using UnityEngine;
using Random = UnityEngine.Random;

public class Tree : Resource
{
    public static Sprite[] _treeShadows;



    public override void Init(bool destroyed)
    {
        SetCollidersInChilds(false);
        var trunk = transform.GetChild(0);
        trunk.GetComponent<CircleCollider2D>().enabled = true;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        GetComponent<DropScript>().enabled = true;
        GetComponent<Tree>().enabled = true;

        // ZlayerManager.SetSortingOrder(transform, spriteRenderer);

        var rigid = trunk.GetComponent<Rigidbody2D>();
        rigid.simulated = true;
        rigid.bodyType = RigidbodyType2D.Kinematic;

        dead = destroyed;
        if (destroyed)
        {
            type = ResourceManager.Instance.GetTrunk(type);
            StubInit();
        }

        spriteRenderer.sortingOrder = 0;

        float z = ZlayerManager.GetZFromY(transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);

        //// Hyi vittu
        //if (transform.childCount == 3)
        //{
        //    GameObject shadowGo = new GameObject("shadow");
        //    shadowGo.transform.parent = transform;
        //    var shadowRenderer = shadowGo.AddComponent<SpriteRenderer>();
        //    shadowRenderer.sortingLayerID = SortingLayer.NameToID("ObjectShadow");
        //    shadowRenderer.sprite = _treeShadows[ResourceManager.TreeToShadow(type)];
        //    shadowGo.transform.position = transform.position;
        //}
        //else
        //{
        //    var shadow = transform.GetChild(3);
        //    var shadowRenderer = shadow.gameObject.GetComponent<SpriteRenderer>(); 
        //    shadowRenderer.sprite = _treeShadows[ResourceManager.TreeToShadow(type)];
        //    shadowRenderer.sortingLayerID = SortingLayer.NameToID("ObjectShadow");
        //    shadowRenderer.enabled = true;

        //    shadow.transform.position = transform.position;
        //}
    }

    public void StubInit()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<DropScript>().enabled = false;
        GetComponent<Tree>().enabled = false;

        // Warning  prefabien eka on trunk
        var trunk = transform.GetChild(0);
        trunk.GetComponent<CircleCollider2D>().enabled = true;
        trunk.GetComponent<SpriteRenderer>().enabled = true;
        var rigid = trunk.GetComponent<Rigidbody2D>();
        rigid.simulated = true;
        rigid.bodyType = RigidbodyType2D.Kinematic;

        var platform = transform.GetChild(1);
        platform.gameObject.SetActive(false);

        var falling = transform.GetChild(2);
        falling.gameObject.SetActive(false);
    }


    SpriteRenderer GetShadowRender()
    {
        // return transform.GetChild(3).GetComponent<SpriteR// enderer>(
        return null;
    }

    public void CopyStub()
    {
        var go = Instantiate(transform.GetChild(0));
        go.transform.position = transform.position;
        // TODO: set parent
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hit(50);
        }
    }

    public override void Hit(int damage)
    {
        DefaultHit(damage, "Tree");
    }

    public override void OnDead()
    {
        type = ResourceManager.Instance.GetTrunk(type);
        StartFalling();

        GetComponent<DropScript>().Drop();

        // GetShadowRender().sprite = ResourceManager.GetFallenTreeSprite();
    }

    public override void DeActivate()
    {
        StubInit();
    }

    private void SetCollidersInChilds(bool state)
    {
        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = state;
        }

        foreach (var body in GetComponentsInChildren<Rigidbody2D>())
        {
            body.simulated = state;
        }
    }
    // NOTE: Puilla täytyy olla oikea rigidbody setup toimiakseen!!!
    // ks Pine
    private void StartFalling()
    {
        var falling = transform.GetChild(2);
        var fader = falling.gameObject.AddComponent<Fader>();
        fader.StartFading(deathTimer - 0.1f, 2f ,falling.GetComponent<SpriteRenderer>());

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

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<DropScript>().enabled = false;
        GetComponent<Tree>().enabled = false;
        StartCoroutine(StartDropTimer());
    }
}
