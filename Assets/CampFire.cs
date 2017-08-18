using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CampFire : Resource
{
    private Animator FireAnimation;
    public GameObject Spr1;
    public GameObject Spr2;
    public GameObject Spr3;
    public GameObject Spr4;
    public GameObject Spr5;
    public GameObject Circle;
    public enum FireState
    {
        OnFire,
        Ember,
        NoFire,
    }

    void Start()
    {
        FireAnimation = GetComponent<Animator>();
        BaseManager.Instance.AddBase(this.gameObject); // milloin pois ? /- ei ikinä ?
    }
     
    public override void OnDead()
    {
        // Destroy(this.gameObject);
    }

    public void SetFire(FireState state)
    {
        FireAnimation.SetInteger("state", (int)state);

        if (state == FireState.OnFire)
        {
            type = ResourceType.campfire_fire;
        }
        else if (state == FireState.Ember)
        {
            type = ResourceType.campfire_ember;
        }
        else if (state == FireState.NoFire)
        {
            type = ResourceType.campfire_noFire;
        }
    }

    private FireState test = FireState.OnFire;
    public void Update()
    {


        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    SetFire(test);
        //    test++;
        //    if ((int)test == 3)
        //    {
        //        test = 0;
        //    }
        //}
    }
    public void setRuneActive()
    {
        SetFire(FireState.Ember);
        
    }
    public override void Init(bool destroyedVersion)
    {
        type = destroyedVersion ? ResourceType.campfire_noFire : ResourceType.campfire_fire;

        if (type == ResourceType.campfire_fire)
        {
            SetFire(FireState.OnFire);
        }
        else
        {
            SetFire(FireState.NoFire);
        }
    }
}
