using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runes/BuffRune")]
public class WeaponBuffRune : Rune
{
    private GameObject _owner;
    private RuneEffectLauncher _launcher;

    public Buff buff;

    public override void init(GameObject owner)
    {
        _owner = owner;     // Launcheriin visuaalinene efectio jos on sellainen 
    }

    public override void Fire()
    {
        Debug.Log("start");
        buff.Apply(_owner);   
    }

    //public override void OnGui(RuneBarUiController ui)
    //{
    //}
}
