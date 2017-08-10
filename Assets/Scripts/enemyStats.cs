using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class enemyStats : MonoBehaviour {

    [Header("Enemy stats")]

    public float damage = 10f;
    public float hp = 100f;
    public float armor = 1f;
    public float AttackArea = 1;
    internal combat Player;

    private void Start()
    {
        Player = PlayerScript.Player.GetComponent<combat>();
    }


    public abstract void takeDamage(float damageTaken);
}
