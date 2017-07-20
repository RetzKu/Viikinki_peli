using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public struct AoeEffectData
{
    // Buff BuffToApply
    public Buff BuffToApply;
    public float ExpansionTime;
    public float TotalRotation;
    public bool LeavesEffectArea;
    public float StartScale;
    public float EndScale;
    public int Frames;
    public Rune AfterEffect;
// Donezo
    public Vector2 MovementDir;
    public float Speed;
}

[CreateAssetMenu(menuName = "Runes/Aoe rune")]
public class FreezeRune : Rune
{
    private GameObject _owner;
    private RuneEffectLauncher _launcher;
    private LayerMask _collisionMask;

    // public Tyyppi tyyppi;
    public AoeEffectData EffectData;
    [Header("TODO: frameille parempi korvaus (aika)")]

    // esim
    // spublic Buff EffectAreaBuff;
    // public float EffectAreaApplyTimer;

    [Header("if size == 0; mask = Enemy")]
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
        // launcheri tekee likaisen työn maailman kanssa visuaalinen collision
        _launcher.FireAoeEffect(sprite, EffectData, _collisionMask);
        Vector2 pos = _owner.transform.position;

        Debug.Log("FreezeRune lähettää terveisensä!");

        // Laukaise cast efecti
        // tyyppi.cast(_owner);
    }
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
