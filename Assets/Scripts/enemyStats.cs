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
    public bool Crittable = false;

    private void Start()
    {
        Player = PlayerScript.Player.GetComponent<combat>();
    }

    public int CalculateArmor(int hp, int armor, int damage)
    {
        if (damage > armor)
        {
            hp -= damage - armor;
        }

        if(hp <= 0)
        {
            GetComponent<DropScript>().Drop();
            Destroy(GetComponent<DropScript>());
        }

        if (GameObject.Find("Player").GetComponent<PlayerScript>().weaponInHand != null)
        {
            PlayerScript.Player.GetComponent<PlayerScript>().LoseDurability(); 
        }
        ParticleSpawner.instance.SpawSmallBlood(GameObject.Find("Player").transform.position, transform.position);

        return hp;
    }

    //private void OnDestroy()
    //{
    //    GetComponent<DropScript>().Drop();
    //}


    public abstract void takeDamage(float damageTaken);
}
