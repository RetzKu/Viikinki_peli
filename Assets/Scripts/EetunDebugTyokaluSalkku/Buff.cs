using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : ScriptableObject
{
    public float Duration;
    public abstract void Apply(GameObject target);
}
