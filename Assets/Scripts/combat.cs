using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat : MonoBehaviour {

    [Header("Player base stats")]

    // Pelaajan default statsit
    public float hp = 100.0f;
    public float dmgBase = 5.0f;
    public float rangedBaseDmg = 0.0f;
    public float movementSpeed = 100.0f;
    public float attackSpeed = 1.0f;
    public float armor = 1.0f;
    public float DefaultAttackLength = 0.2f;

    // Attack cd muuttuja, muuta attackSpeed muuttujaa jos haluat muokata attackspeediä
    private float attackSpeedTime = 0.0f;
    [HideInInspector]
    public float atmAttackTime = 0.0f;
    // Flagi jolla tarkistetaan onko lyönnillä JO tehty damagea
    private bool damageDone = false;
    
    private enum Directions { Left, Down, Up, Right }
    private Directions Direction;

    private string dirObjectName;
	private Sprite Fx;


	void Update ()
    {
        // Tarkistetaan onko pelaaja elossa
        if(hp <= 0)
        {
            death();
        }

        // If lauseke jolla tarkistetaan lyöntiä
        // Tarkistetaan onko lyönnin CD
        if (atmAttackTime < Time.time && Time.time < (atmAttackTime + DefaultAttackLength))
        {
            // Onko pelajaalla asetta
            if (GetComponent<PlayerScript>().weaponInHand != null)
            {
                GameObject tempWeapon = GetComponent<PlayerScript>().weaponInHand;

                // Onko vihu efektin hitboxissa
                if (tempWeapon.GetComponent<weaponStats>().onRange)
                {
                    // Onko jo tehty damagea tällä lyönnillä ja vihollinen olemassa
                    if (damageDone == false && GetComponent<FxScript>().lastHittedEnemy != null)
                    {
                        // Tehään viholliseen damagea
                        GetComponent<FxScript>().lastHittedEnemy.GetComponent<enemyStats>().takeDamage(countPlayerDamage());
                        Debug.Log("Dmg given to: " + GetComponent<FxScript>().lastHittedEnemy.name + " " + countPlayerDamage() + ". HP left: " + GetComponent<FxScript>().lastHittedEnemy.GetComponent<wolfStats>().hp);
                        damageDone = true;
                    }
                }
            }
            else
            {
                // Jos ei ole asetta, pelaaja käsin
                if (GetComponent<FxScript>().handEffectOnrange)
                {
                    // Onko jo tehty damagea tällä lyönnillä ja vihollinen olemassa
                    if (damageDone == false && GetComponent<FxScript>().lastHittedEnemy != null)
                    {
                        // Tehään viholliseen damagea
                        GetComponent<FxScript>().lastHittedEnemy.GetComponent<enemyStats>().takeDamage(countPlayerDamage());
                        Debug.Log("Dmg given to: " + GetComponent<FxScript>().lastHittedEnemy.name + " " + countPlayerDamage() + ". HP left: " + GetComponent<FxScript>().lastHittedEnemy.GetComponent<wolfStats>().hp);
                        damageDone = true;
                    }
                }
            }
        }

        // Lataa pelaajalle oikein hitboxin suunnan mukaan
        dirObjectName = getDirectionObject();
    }

    // Lataa pelaajalle oikeat hitboxit
    public string getDirectionObject()
    {
        string tmpName = "";
        int tmpDir = GetComponent<AnimatorScript>().PlayerDir();

        if(tmpDir == 0 || tmpDir == 3) // Liikutaan sivulle
        { 
            tmpName = "s_c_torso";
            transform.Find(tmpName).GetComponent<Collider2D>().enabled = true;
            transform.Find("u_c_torso").GetComponent<Collider2D>().enabled = false;
            transform.Find("d_c_torso").GetComponent<Collider2D>().enabled = false;
        }
        else if (tmpDir == 2) // Liikutaan ylös
        {
            tmpName = "u_c_torso";
            transform.Find(tmpName).GetComponent<Collider2D>().enabled = true;
            transform.Find("s_c_torso").GetComponent<Collider2D>().enabled = false;
            transform.Find("d_c_torso").GetComponent<Collider2D>().enabled = false;
        }
        else if (tmpDir == 1) // Liikutaan alas
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

    //void OnTriggerEnter2D(Collider2D Trigger)
    //{
         

    //}

    //void OnTriggerStay2D(Collider2D Trigger)
    //{


    //}

    // Onko lyönti cooldownilla
    private bool isAttackLegal()
    {
        if(Time.time > attackSpeedTime)
        {
            damageDone = false;
            atmAttackTime = Time.time; // lyödään juuri tähän aikaan
            atmAttackTime -= 0.00001f;
            attackSpeedTime = Time.time + attackSpeed; // tähän lisätään aseen weight
            return true;
        }
        return false;
    }

    // Lasketaan pelaajan damage ottaen ase huomioon
    public float countPlayerDamage()
    {
        float playerDamage = dmgBase;

        if (GetComponent<PlayerScript>().Inventory.EquipData.Tool != null)
        {
            GameObject tempWeapon = GetComponent<PlayerScript>().Inventory.EquipData.Tool;
            playerDamage += tempWeapon.GetComponent<weaponStats>().damage;
        }

        return playerDamage;
    }

    // Metodi jolla tarkistetaan onko painettu lyöty ja lyönti pois CD
    public bool attackBoolean()
    {
        // Tähän voisi tehdä pari riviä koodia joka tarkistaa onko kyseessä android vai pc inputit ja sen mukaan ohjaisi
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isAttackLegal()) // PC
            {
                return true;
            }
        }
        return false;
    }


    public float hit()
    {
        float damage = dmgBase;

        

        /*
         *  Tässä kohtaa voitaisiin etsiä itemeitä / buffeja / tjsp ja lisätä se kokonais damageen, jos tarpeen
         */

        return damage;
    }

    // VANHA, UUSI VASTAAVA PLAYERSCRIPTISSÄ public GameObject weaponInHand
    /*public WeaponType weaponInHand()
    {
        WeaponType weapon = WeaponType.noWeapon;

        if (transform.Find("Equip").childCount != 0)
        {
            if (transform.Find("Equip").GetChild(0).GetComponent<Ranged>() != null) { weapon = WeaponType.rangedWeapon; }
            else if (transform.Find("Equip").GetChild(0).GetComponent<Melee>() != null) { weapon = WeaponType.meleeWeapon; }
            else if (transform.Find("Equip").GetChild(0).GetComponent<longMelee>() != null) { weapon = WeaponType.longMeleeWeapon; }
            else{ }
        }
        else { weapon = WeaponType.noWeapon; }

        return weapon;
    }*/

    // Metodi jolla pelaaja ottaa damagea
    public void takeDamage(float rawTakenDamage)
    {
        hp = hp - (rawTakenDamage / armor);
    }

    void death()
    {
        // Kuoleman jälkeiset asiat tänne
        Debug.Log("Kuolit");
    }
}
