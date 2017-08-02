using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat : MonoBehaviour
{

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

    private Vector2 lastEnemyHitPosition = new Vector2(0f, 0f);

    private enum Directions { Left, Down, Up, Right }
    private Directions Direction;

    private string dirObjectName;
    private Sprite Fx;

    void Update()
    {

        // Joonan testikoodia raycastille, joiden olisi tarkoitus tulla korteille
        //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
        //Vector3 fwd = Camera.main.transform.TransformDirection(Vector3.forward);

        //if (Physics.Raycast(transform.position, fwd, Mathf.Infinity))
        //    print("There is something in front of the object!");

        // Tarkistetaan onko pelaaja elossa
        if (hp <= 0)
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
                        GetComponentInChildren<weaponStats>().useDuration();
                        Debug.Log("Dmg given to: " + GetComponent<FxScript>().lastHittedEnemy.name + " " + countPlayerDamage() + ". HP left: " + GetComponent<FxScript>().lastHittedEnemy.GetComponent<enemyStats>().hp);
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
                        Debug.Log("Dmg given to: " + GetComponent<FxScript>().lastHittedEnemy.name + " " + countPlayerDamage() + ". HP left: " + GetComponent<FxScript>().lastHittedEnemy.GetComponent<enemyStats>().hp);
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

        if (tmpDir == 0 || tmpDir == 3) // Liikutaan sivulle
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
        if (Time.time > attackSpeedTime)
        {
            damageDone = false;
            atmAttackTime = Time.time; // lyödään juuri tähän aikaan
            atmAttackTime -= 0.00001f;
            attackSpeedTime = Time.time + attackSpeed; // tähän lisätään aseen weight
            return true;
        }
        return false;
    }

    public bool IsAttacking()
    {
        return Time.time < (atmAttackTime + DefaultAttackLength);
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

    public void setPlayerOnFire()
    {
        ParticleSpawner.instance.SpawSlow(GameObject.Find("Player"), 2.5f);
        takeDamage(10f);
    }

    // Metodi jolla tarkistetaan onko painettu lyöty ja lyönti pois CD
    public void attackBoolean(Vector2 direction)
    {
        // Tähän voisi tehdä pari riviä koodia joka tarkistaa onko kyseessä android vai pc inputit ja sen mukaan ohjaisi
        if (isAttackLegal()) // PC
        {
            // VÄÄRÄSSÄ PAIKASSA ATM MUTTA TOIMII =D
            if (GetComponent<PlayerScript>().EquippedTool.Type == WeaponType.rangedWeapon)
            {
                // Ammu nuoli, vähentää pelaajan inventorystä nuolen
                if (GetComponent<PlayerScript>().EquippedTool.UsedArrow() == true)
                {
                    // Vector2 tempo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 tempo2 = new Vector2((direction.x - transform.position.x), (direction.y - transform.position.y));
                    tempo2.Normalize();

                    GameObject.Find("projectileManager").GetComponent<ProjectileManager>().spawnProjectile(transform.position, new Vector2(transform.position.x + tempo2.x * 6, transform.position.y + tempo2.y * 6));
                    GetComponentInChildren<weaponStats>().useDuration();
                    // Tähän voisi laittaa efektin vaihtumaan bowi efektiin
                }
                else
                {
                    // Tähän voisi laittaa efektin vaihtumaan lyönti efektiin
                }
            }
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

    public void setHitPosition(Vector2 position)
    {
        lastEnemyHitPosition = position;
    }

    // Metodi jolla pelaaja ottaa damagea
    public void takeDamage(float rawTakenDamage)
    {
        // Lisää tähän tsekkaus
        hp = hp - (rawTakenDamage / armor);
        GetComponent<Movement>().KnockBack(lastEnemyHitPosition);
        Debug.Log("Player has " + hp + " hp left.");
        GetComponent<DamageVisual>().TakeDamage();
        ParticleSpawner.instance.SpawSmallBlood(lastEnemyHitPosition, transform.position);
        GameObject.Find("HpBase").GetComponent<HealthBar>().RefreshHP((int)hp);
    }

    void death()
    {
        // Kuoleman jälkeiset asiat tänne
        Debug.LogError("Kuolit");
    }
}
