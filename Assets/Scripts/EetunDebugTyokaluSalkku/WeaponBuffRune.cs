using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runes/BuffRune")]
public class WeaponBuffRune : Rune
{
    private GameObject _owner;
    private RuneEffectLauncher _launcher;

    public Buff buff;


    [Header("HUOMIO! vain visuaalit korjaa kun haluat tehdä 10000 lisää runea")]
    public AoeEffectData FaderEffectData;

    public override void init(GameObject owner)
    {
        _owner = owner;     // Launcheriin visuaalinene efectio jos on sellainen 
        this._launcher = owner.GetComponent<RuneEffectLauncher>();
    }

    public override void Fire()
    {
        if (sprite != null && FaderEffectData != null)
        {
            _launcher.LaunchAoeFader(sprite, FaderEffectData);
        }

        buff.Apply(_owner);   
    }

    //public override void OnGui(RuneBarUiController ui)
    //{
    //}
}
