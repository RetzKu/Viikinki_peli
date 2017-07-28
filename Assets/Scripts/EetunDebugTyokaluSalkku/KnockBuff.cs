using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runes/Buff/Aibuff/Knock")]
public class KnockBuff : Buff
{
    // public float KnockAmount;
    [Range(0f, 1f)]
    public float KnockPercent;

    public override void Apply(GameObject target)
    {
        target.GetComponent<generalAi>().KnockBack( /**/);
    }
}

