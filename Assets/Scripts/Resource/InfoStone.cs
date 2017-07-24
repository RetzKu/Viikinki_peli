using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoStone : Resource
{
    public Rune RuneToTeach;

    public override void OnDead()
    {
        Destroy(gameObject);
    }

    public override void Init(bool destroyedVersion)
    {
    }
}
