using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(menuName = "Runes/Aoe rune")]
public class FreezeRune : Rune
{
    public float Range = 10f;


    private GameObject _owner;
    private RuneEffectLauncher _launcher;
    private LayerMask _collisionMask;

    public Buff BuffToApply;

    [Header("if length == 0 | mask = Enemy")]
    public string[] CollisionMaskValues;


    public override void init(GameObject owner)
    {
        this._owner = owner;
        this._launcher = owner.GetComponent<RuneEffectLauncher>();

        if (_launcher == null)
        {
            Debug.LogError("Laita RuneEffectLaucher.cs omistajalle (pelaaja?)");
        }

        if (CollisionMaskValues != null)
        {
            _collisionMask = LayerMask.GetMask(CollisionMaskValues);
        }
        else
        {
            _collisionMask = LayerMask.GetMask("Enemy");
        }
    }

    public override void Fire()
    {
        _launcher.Fire(sprite);
        Vector2 pos = _owner.transform.position;

        var colliders = Physics2D.CircleCastAll(pos, Range, new Vector2(0, 0), 0, _collisionMask);
        foreach (var collider in colliders)
        {
            Destroy(collider.transform.gameObject);
        }
        Debug.Log("FreezeRune lähettää terveisensä");
    }

    // Rune effect funktio, jota kutsuttaisiin Launcherista takaisin
    // OnStart Ja OnRuneEnd, joihin laitettaisiin esim buffin statsit ja rune effectiä voisi myös kutsua coroutinellä takaisin
}


[CreateAssetMenu(menuName = "Runes/BuffRune NOT IMPLEMENTED!")]
public class WeaponBuffRune : Rune
{
    // effect?
    //private GameObject owner;
    public float Range = 10f;
    public float duration;

    // statsit:
    // HP
    // Attack
    // Movement Speed 
    // joku cool buff: Immortal jne...
    // Debuffs:
    // CC
    // Statsit

    public override void init(GameObject owner)
    {
        //this.owner = owner;     // Launcheriin visuaalinene efectio jos on sellainen 
    }

    public override void Fire()
    {
        // owner.applyBuff();
        // laita timer joka lopettaa buffing keston
    }
}


