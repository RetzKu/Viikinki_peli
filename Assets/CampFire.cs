using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CampFire : Resource
{
    private Animator FireAnimation;


    public enum FireState
    {
        OnFire,
        Ember,
        NoFire,
    }

    void Start()
    {
        FireAnimation = GetComponent<Animator>();
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
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetFire(test);
            test++;
            if ((int)test == 3)
            {
                test = 0;
            }
        }
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
