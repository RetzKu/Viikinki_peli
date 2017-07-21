using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class weaponStats : MonoBehaviour
{
    [Header("Weapon stats")]

    public float damage = 1.0f;
    public float attackWeight = 10.0f;
    public int duration = 10;
    public abstract void useDuration();

    [Header("Effect details")]

    public Sprite weaponEffect;
    [HideInInspector]
    public bool onRange = false;
    public float distance;
    public Vector3 weaponEffectScale = new Vector3 (1f, 1f, 1f);
    public Vector3 effectOffSet = new Vector3(0f, 0f, 0f);

}