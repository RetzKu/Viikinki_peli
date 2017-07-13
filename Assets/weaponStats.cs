using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class weaponStats : MonoBehaviour
{
    [Header("Weapon stats")]

    public float maxDistance;
    public float damage = 1.0f;
    public float attackWeight = 10.0f;
    public Sprite weaponEffect;
    public bool onRange = false;
    public Vector3 weaponEffectScale = new Vector3 (1f, 1f, 1f);

    public virtual bool Get()
    {
        return false;
    }
}