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
    public GameObject Circle;
    public Rune rune1;/* en tiiä mitä teen näillä */
    public Rune rune2;
    public Rune rune3;
    public Rune rune4;
    public Rune MyRune;
    FireState mystate;

    bool tryActivate = false;
    public enum FireState
    {
        OnFire,
        Ember,
        NoFire,
    }

    void Awake()
    {

    }
    public void init()
    {
        FireAnimation = GetComponent<Animator>();
        BaseManager.Instance.AddBase(this.gameObject); // milloin pois ? /- ei ikinä ?
        SetFire(FireState.NoFire);
    }
    public void secureState()
    {
        SetFire(mystate);
    }
    public override void OnDead()
    {
        // Destroy(this.gameObject);
    }

    public void SetFire(FireState state)
    {
        mystate = state;
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
        //    setRuneActive();
        //    print("got input");
        //    //test++;
        //    //if ((int)test == 3)
        //    //{
        //    //    test = 0;
        //    //}
        //}
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    activateFirePlace();
        //    print("got input");
        //    //test++;
        //    //if ((int)test == 3)
        //    //{
        //    //    test = 0;
        //    //}
        //}
    }
    void setchild(GameObject go)
    {
        go.transform.position = transform.position + new Vector3(1.0f, 0, 0);
        go.transform.parent = transform; //check
    }
    public void setRuneActive()
    {
        tryActivate = true;
        SetFire(FireState.Ember);
        //spawn circle with random pic
        var go = Instantiate(Circle);
        setchild(go);
        int rand = UnityEngine.Random.Range(1, 4);
        if (rand == 1)
        {
            var bo = Instantiate(Spr1);
            setchild(bo);
            MyRune = Instantiate(rune1);
        }
        else if (rand == 2)
        {
            var bo = Instantiate(Spr2);
            setchild(bo);
            MyRune = Instantiate(rune2);
        }
        else if (rand == 3)
        {
            var bo = Instantiate(Spr3);
            setchild(bo);
            MyRune = Instantiate(rune3);
        }
        else
        {
            var bo = Instantiate(Spr4);
            setchild(bo);
            MyRune = Instantiate(rune4);
        }

    }
    public void activateFirePlace()
    {
        tryActivate = false;
        SetFire(FireState.OnFire);
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        //destroy circle + pic
    }

    public void ValidateRune(Vec2[] positions, int realisize)
    {
        if (tryActivate)
        {
            if (realisize == rune1.Length && rune1.ValidateRune(positions))
            {
                // ok t:pate
                activateFirePlace();

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
