using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runes/NavigationRune")]
public class NavigationRune : Rune
{
    private GameObject owner;
    private RuneEffectLauncher launcher;

    public override void init(GameObject owner)
    {
        this.owner = owner;
        this.launcher = owner.GetComponent<RuneEffectLauncher>();

        if (launcher == null)
        {
            Debug.LogError("Laita RuneEffectLaucher.cs omistajalle (pelaaja?)");
        }
    }

    public override void Fire()
    {
        Vector3 closestBase = BaseManager.Instance.GetClosestBase().transform.position;
        launcher.LaunchArrow(sprite, closestBase);
    }

    //public override void OnGui(RuneBarUiController ui)
    //{
    //}
}
