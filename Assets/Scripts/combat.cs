using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat : MonoBehaviour {

    [Header("Player base stats")]

    public float hp = 100.0f;
    public float dmgBase = 5.0f;
    public float rangedBaseDmg = 0.0f;
    public float movementSpeed = 100.0f;
    public float attackSpeed = 1.0f;
    public float armor = 1.0f;

    private enum Directions { Left, Down, Up, Right }
    private Directions Direction;

    public enum WeaponType { noWeapon, meleeWeapon, longMeleeWeapon, rangedWeapon }

    private string dirObjectName;
	private Sprite Fx;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(hp <= 0)
        {
            death(); // tarkistetaan onko pelaaja elossa
        }
        //weaponInHand();
        //OnTriggerEnter2D(GetComponent<Collider2D>());

        dirObjectName = getDirectionObject();
		//transform.GetComponent<FxScript>().FxUpdate(Fx); /*KUN Fx SPRITEÄ MUUTTAA FxUpdate KATSOO ONKO SE ERINLAINEN FX KUIN AIKAISEMPI JA JOS ON NIIN VAIHTAA. VOI SIIRTÄÄ MYÖS POIS UPDATESTA*/
    }

    string getDirectionObject() // Lataa pelaajalle oikeat hitboxit
    {
        string tmpName = "";
        int tmpDir = GetComponent<AnimatorScript>().PlayerDir();

        if(tmpDir == 0 || tmpDir == 3) // 0 sivu
        { 
            tmpName = "s_c_torso";
            transform.Find(tmpName).GetComponent<Collider2D>().enabled = true;
            transform.Find("u_c_torso").GetComponent<Collider2D>().enabled = false;
            transform.Find("d_c_torso").GetComponent<Collider2D>().enabled = false;
        }
        else if (tmpDir == 2) // 2 ylös
        {
            tmpName = "u_c_torso";
            transform.Find(tmpName).GetComponent<Collider2D>().enabled = true;
            transform.Find("s_c_torso").GetComponent<Collider2D>().enabled = false;
            transform.Find("d_c_torso").GetComponent<Collider2D>().enabled = false;
        }
        else if (tmpDir == 1) // 1 alas
        {
            tmpName = "d_c_torso";
            transform.Find(tmpName).GetComponent<Collider2D>().enabled = true;
            transform.Find("s_c_torso").GetComponent<Collider2D>().enabled = false;
            transform.Find("u_c_torso").GetComponent<Collider2D>().enabled = false;
        }
        else { }
        return tmpName;
    }

    public Collider2D activeCollider()
    {
        string tmpTorso = getDirectionObject();
        return transform.Find(tmpTorso).GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D Trigger)
    {
        Debug.LogWarning("Trigger111");
        print("trigger");
        if (Trigger.IsTouchingLayers(LayerMask.NameToLayer("Player")) == true)
        {
            Debug.LogWarning("Koskee pelaajaaan");
            GetComponent<PlayerScript>().AddToInventory(Trigger);
        }
    }

    public float hit()
    {
        float damage = dmgBase;

        

        /*
         *  Tässä kohtaa voitaisiin etsiä itemeitä / buffeja / tjsp ja lisätä se kokonais damageen, jos tarpeen
         */

        return damage;
    }

    public WeaponType weaponInHand()
    {
        WeaponType weapon = WeaponType.noWeapon;

        //int tmpSide = transform.GetComponent<AnimatorScript>().PlayerDir();

        if (transform.Find("Equip").childCount != 0)
        {
            if (transform.Find("Equip").GetChild(0).GetComponent<Ranged>() != null) { weapon = WeaponType.rangedWeapon; }
            else if (transform.Find("Equip").GetChild(0).GetComponent<Melee>() != null) { weapon = WeaponType.meleeWeapon; }
            else if (transform.Find("Equip").GetChild(0).GetComponent<longMelee>() != null) { weapon = WeaponType.longMeleeWeapon; }
            else{ }
        }
        else { weapon = WeaponType.noWeapon; }

        return weapon;
    }

    public void takeDamage(float rawTakenDamage)
    {
        hp = hp - (rawTakenDamage / armor);
    }

    void death()
    {
        // kuoleman jälkeiset asiat
        Debug.Log("kuolit");
    }
}
