using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFx : MonoBehaviour {

    public Sprite DefaultSprite;
    public Vector3 DefaultOffset;
    public float DefaultDistance;
    public float DefaultDuration;

    private GameObject BaseFx;
    private GameObject Fx;

    private GameObject Weapon;
    private WeaponType Type;

    private void Awake()
    {
        BaseFx = new GameObject("BaseFx");
        BaseFx.AddComponent<SpriteRenderer>().sprite = DefaultSprite;
        CheckWeapon();
        GetWeaponSettings();
        GetWeaponSprite();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) == true) { Attack(); }
    }

    private void CheckWeapon()
    {
        Weapon = transform.GetComponent<EnemyAnimator>().Weapon;
        Type = transform.GetComponent<EnemyAnimator>().Type;
    }

    private void GetWeaponSettings()
    {
        Fx = Instantiate(BaseFx);
        Fx.GetComponent<SpriteRenderer>().sprite = GetWeaponSprite();
        Fx.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
    }
    private Vector3 GetOffset()
    {
        Vector3 effectOffSet;

        switch (Type)
        {
            case WeaponType.meleeWeapon: { effectOffSet = Weapon.GetComponent<Melee>().effectOffSet; break; }
            case WeaponType.longMeleeWeapon: { effectOffSet = Weapon.GetComponent<longMelee>().effectOffSet; break; }
            case WeaponType.rangedWeapon: { effectOffSet = Weapon.GetComponent<Ranged>().effectOffSet; break; }
            case WeaponType.noWeapon: { effectOffSet = DefaultOffset; break; }
            default: { effectOffSet = DefaultOffset; break; }
        }
        return effectOffSet;
    }

    public void Attack()
    {
        GameObject TmpFx = Instantiate(Fx);
        TmpFx.AddComponent<FxFade>().Duration = DefaultDuration;
        TmpFx.layer = LayerMask.NameToLayer("EnemyFx");
        TmpFx.AddComponent<BoxCollider2D>().isTrigger = true;
        TmpFx.transform.SetParent(transform);
        Destroy(TmpFx, DefaultDuration);
        Vector3 EffectOffSet = GetOffset();
        Vector3 MousePoint = GameObject.Find("Player").transform.position; // hiiren sijainti
        Vector3 Base = transform.position + EffectOffSet; //Missä on pelaajan base jonka ympärillä efekti rotatee
        Vector3 MouseDir = (MousePoint - transform.position - EffectOffSet).normalized * DefaultDistance; //mikä on hiiren suunta
        TmpFx.transform.position = Base + MouseDir;
        GetAngleDegress(TmpFx, MouseDir);
    }

    void GetAngleDegress(GameObject Copy, Vector3 MouseDir)
    {
        Vector3 Mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var n = 0 - (Mathf.Atan2(MouseDir.y, MouseDir.x)) * 180 / Mathf.PI;
        Copy.transform.Rotate(0, 0, n * -1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<s_c_torsoScript>() != null)
        {
            GameObject.Find("Player").GetComponent<combat>().setHitPosition(transform.position);
            GameObject.Find("Player").GetComponent<combat>().takeDamage(Weapon.GetComponent<Melee>().damage);            
        }
    }

    private Sprite GetWeaponSprite()
    {
        Sprite TmpFx;

        switch(Type)
        {
            case WeaponType.meleeWeapon: { TmpFx = Weapon.GetComponent<Melee>().weaponEffect; break; }
            case WeaponType.longMeleeWeapon: { TmpFx = Weapon.GetComponent<longMelee>().weaponEffect; break; }
            case WeaponType.rangedWeapon: { TmpFx = Weapon.GetComponent<Ranged>().weaponEffect; break; }
            case WeaponType.noWeapon: { TmpFx = DefaultSprite; break; }
            default: { TmpFx = DefaultSprite; break; }
        }
        return TmpFx;
    }
    
}
