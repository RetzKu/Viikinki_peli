using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxScript : MonoBehaviour {

    public Sprite BareHandSprite;
    private GameObject Fx;
    private GameObject CopyFx;
    [Header("Effect settings")]
    public float LifeTime;
    public float MaxDistance;
    public Vector3 SpriteScale = new Vector3(1f, 1f, 1f);
    [Header("Default x: 0, y: 0.3, z: 0")]
    public Vector3 EffectOffSet;

    private Vector3 MousePoint;
    private Vector3 Base;
    private Vector3 MouseDir;

    public bool handEffectOnrange = false;
    public GameObject lastHittedEnemy;

	void Start ()
    {
        Fx = new GameObject("Fx");
        Fx.AddComponent<SpriteRenderer>().sprite = BareHandSprite;
        Fx.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
        Fx.transform.localScale = SpriteScale;
	}

    public void Default()
    {
        Fx.GetComponent<SpriteRenderer>().sprite = BareHandSprite;
    }

    public void FxUpdate(Sprite NewFx)
    {
        if(NewFx != null && NewFx.name != Fx.GetComponent<SpriteRenderer>().name)
        {
            Fx.GetComponent<SpriteRenderer>().sprite = NewFx;
        } 
    }

    public void instantiateFx()
    {
        CopyFx = Instantiate(Fx);
        
        Destroy(CopyFx, LifeTime);
        CopyFx.AddComponent<FxFade>().Duration = LifeTime;
        CopyFx.AddComponent<BoxCollider2D>().isTrigger = true;
        ObjectPosition(CopyFx);
        CopyFx.transform.SetParent(transform);
        if (GetComponent<PlayerScript>().weaponInHand != null)
        {
            GameObject tempWeapon = GetComponent<PlayerScript>().weaponInHand;
            CopyFx.transform.localScale = tempWeapon.GetComponent<weaponStats>().weaponEffectScale;
        }
        else
        {
            CopyFx.transform.localScale = SpriteScale;
        }
    }

    void ObjectPosition(GameObject Copy)
    {
        MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); // hiiren sijainti
        Base = transform.position + EffectOffSet; //Missä on pelaajan base jonka ympärillä efekti rotatee
        if (GetComponent<PlayerScript>().weaponInHand != null)
        {
            GameObject tempWeapon = GetComponent<PlayerScript>().weaponInHand;
            MaxDistance = tempWeapon.GetComponent<weaponStats>().distance;
        }
        else
        {
            MaxDistance = 0.3f;
        }
        if (GetComponent<PlayerScript>().weaponInHand != null)
        {
            GameObject tempWeapon = GetComponent<PlayerScript>().weaponInHand;
            EffectOffSet = tempWeapon.GetComponent<weaponStats>().effectOffSet;
        }
        else
        {
            EffectOffSet = new Vector3(0f, 0.3f, 0f);
        }

#if UNITY_EDITOR
        MouseDir = (MousePoint - transform.position - EffectOffSet).normalized * MaxDistance; //mikä on hiiren suunta
        //rotatePate(MouseDir);
        GetComponent<AnimatorScript>().LookAt();
#elif UNITY_ANDROID
        MouseDir = TouchController.AttackDir.normalized * MaxDistance;
        GetComponent<AnimatorScript>().LookAt(MouseDir);
#endif

        Copy.transform.position = Base + MouseDir; 
        GetAngleDegress(Copy);
    }
    //void OnDrawGizmos()
    //{
    //     Gizmos.DrawLine(Base, Base + MouseDir); // piirretään viiva visualisoimaan toimivuutta
    //}
    void GetAngleDegress(GameObject Copy)
    {
        Vector3 Mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var n = 0 - (Mathf.Atan2(MouseDir.y, MouseDir.x)) * 180 / Mathf.PI;
        //var n = 0 - (Mathf.Atan2(Base.y - Base.y + MouseDir.y,Base.x - Base.x + MouseDir.x)) * 180 / Mathf.PI; //origin - target
        //print(n);
        Copy.transform.Rotate(0, 0, n * -1);
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Enemy")
        {
            lastHittedEnemy = trig.gameObject;

            if(GetComponent<PlayerScript>().weaponInHand != null)
            {
                GetComponentInChildren<weaponStats>().onRange = true;
            }
            else
            {
                handEffectOnrange = true;
            }
        }
        else if (trig.gameObject.layer == LayerMask.NameToLayer("ObjectLayer"))
        {
            if( GetComponent<combat>().IsAttacking())
            {
                var resourceGo = trig.gameObject.GetComponent<Resource>();
                // puuPrefabit toimivat hieman eri tavalla kuin muut resurrsit / sama pitää tällä tavalla niin tulevaisuudessa jos tulee eksoottinen prefab
                if (resourceGo == null)
                {
                    trig.gameObject.transform.parent.GetComponent<Resource>()
                        .Hit((int) GetComponent<combat>().countPlayerDamage());
                }
                else
                {
                    resourceGo.Hit((int) GetComponent<combat>().countPlayerDamage());
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Enemy")
        {
            lastHittedEnemy = trig.gameObject; 
            if (GetComponent<PlayerScript>().weaponInHand != null)
            {
                GetComponentInChildren<weaponStats>().onRange = true;
            }
            else
            {
                handEffectOnrange = true;
            }
        }
    }
    void OnTriggerExit2D(Collider2D trig)
    {
        if (GetComponent<PlayerScript>().weaponInHand != null)
        {
            GetComponentInChildren<weaponStats>().onRange = false;
        }
        else
        {
            handEffectOnrange = false;
        }
    }
}
