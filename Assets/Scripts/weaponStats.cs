using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class weaponStats : MonoBehaviour
{
    [Header("Weapon stats")]

    [SerializeField]
    protected float damage = 1.0f;
    [SerializeField]
    protected float attackWeight = 10.0f;


}



