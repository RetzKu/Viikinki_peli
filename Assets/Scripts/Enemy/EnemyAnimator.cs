using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class EnemyAnimator : MonoBehaviour {

    private HandRoot Hand;
    private List<Transform> Hands;

    [SerializeField]
    private GameObject Weapon;
    private WeaponType Type;

    private List<GameObject> Torsos;
    private List<Animator> Animators;
    public bool Movin;

    private SpriteChanger SpriteController;

    private List<Sprite> Heads;
    private List<Sprite> Chests;


    private void Awake()
    {
        /*Finding enemy hands*/
        Hands = new List<Transform>(3);
        Hands.Add(transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0));
        Hands.Add(transform.GetChild(1).GetChild(3).GetChild(0).GetChild(0));
        Hands.Add(transform.GetChild(2).GetChild(3).GetChild(0).GetChild(0));

        /*Finding enemy Torsos*/
        Torsos = new List<GameObject>(3);
        Torsos.Add(transform.GetChild(0).gameObject);
        Torsos.Add(transform.GetChild(1).gameObject);
        Torsos.Add(transform.GetChild(2).gameObject);

        /*Finding Animators*/
        Animators = new List<Animator>(3);
        foreach (GameObject t in Torsos) { Animators.Add(t.GetComponent<Animator>()); }

        Type = CheckWeaponType();

        SpriteController = new SpriteChanger(Torsos);

        /*Building New HandRoot Component*/
        Hand = new HandRoot(Hands[0], SpriteFinderScript.Instance.RandomMeleeWeapon(),Hands);

        Heads = SpriteFinderScript.Instance.RandomHead();
        Chests = SpriteFinderScript.Instance.RandomTorso();

        transform.Find("s_c_torso").Find("s_c_head").GetComponent<SpriteRenderer>().sprite = Heads[0];
        transform.Find("u_c_torso").Find("u_c_head").GetComponent<SpriteRenderer>().sprite = Heads[1];
        transform.Find("d_c_torso").Find("d_c_head").GetComponent<SpriteRenderer>().sprite = Heads[2];

        transform.Find("s_c_torso").GetComponent<SpriteRenderer>().sprite = Chests[0];
        transform.Find("u_c_torso").GetComponent<SpriteRenderer>().sprite = Chests[1];
        transform.Find("d_c_torso").GetComponent<SpriteRenderer>().sprite = Chests[2];

    }

    private void Update()
    {
        CheckVelocity();
    }

    public void ChangeDirection(enemyDir Dir)
    {
        switch(Dir)
        {
            case enemyDir.StillDown: { Hand.SwapHand(enemyDir.Down); SpriteController.EnableSprites(enemyDir.Down); Movin = false;    break; }
            case enemyDir.StillL: { Hand.SwapHand(enemyDir.Left); SpriteController.EnableSprites(enemyDir.Left); Movin = false;    break; }
            case enemyDir.StillR: { Hand.SwapHand(enemyDir.Right); SpriteController.EnableSprites(enemyDir.Right); Movin = false;    break; }
            case enemyDir.StillUp: { Hand.SwapHand(enemyDir.Up); SpriteController.EnableSprites(enemyDir.Up); Movin = false;    break; }
            default: { Hand.SwapHand(Dir); SpriteController.EnableSprites(Dir); Movin = true; break; }
        }
    }

    private WeaponType CheckWeaponType()
    {
        if (Weapon != null)
        {
            if (Weapon.GetComponent<Ranged>() != null) { return WeaponType.rangedWeapon; }
            else if (Weapon.GetComponent<longMelee>() != null) { return WeaponType.longMeleeWeapon; }
            else if (Weapon.GetComponent<Melee>() != null) { return WeaponType.meleeWeapon; }
            else { return WeaponType.noWeapon; }
        }
        else { return WeaponType.noWeapon; }
    }

    void CheckVelocity()
    {
        if (Movin == true)
        {
            foreach (Animator t in Animators) { t.SetBool(WalkType(), true); }
            foreach (Animator t in Animators) { t.SetBool("Walking", true); }
        }
        else
        {
            ResetStates();
            foreach (Animator t in Animators) { t.SetBool(WalkType(), false); }
            foreach (Animator t in Animators) { t.SetBool("Walking", false); }
        }
    }

    public void Attack()
    {
        foreach (Animator t in Animators) { t.SetTrigger(AttackType()); }
    }

    public void ResetStates()
    {
        foreach (Animator t in Animators) { t.SetBool("MeleeWalk", false); }
        foreach (Animator t in Animators) { t.SetBool("LongMeleeWalk", false); }
        foreach (Animator t in Animators) { t.SetBool("RangedWalk", false); }
        foreach (Animator t in Animators) { t.SetBool("FistWalk", false); }
    }

    public string AttackType()
    {
        switch (Type)
        {
            case WeaponType.meleeWeapon: { return "MeleeAttack"; }
            case WeaponType.longMeleeWeapon: { return "LongMeleeAttack"; }
            case WeaponType.rangedWeapon: { return "RangedAttack"; }
        }
        return "Fist";
    }

    public string WalkType()
    {
        switch (Type)
        {
            case WeaponType.meleeWeapon: { return "MeleeWalk"; }
            case WeaponType.longMeleeWeapon: { return "LongMeleeWalk"; }
            case WeaponType.rangedWeapon: { return "RangedWalk"; }
        }
        return "FistWalk";
    }

    public class SpriteChanger
    {
        enemyDir LastDir;
        private List<GameObject> Torsos;

        public SpriteChanger(List<GameObject> TorsoList) { LastDir = enemyDir.Right; Torsos = TorsoList; }

        public void EnableSprites(enemyDir SpriteDir)
        {
            bool changed = false;

            if (SpriteDir != LastDir)
            {
                if (SpriteDir == enemyDir.Right)
                {
                    Torsos[0].GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else
                {
                    Torsos[0].GetComponent<Transform>().localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                for (int i = 0; i < 3; i++)
                {
                    if ((int)SpriteDir == i || SpriteDir == enemyDir.Right && changed == false)
                    {
                        changed = true;
                        foreach (SpriteRenderer t in Torsos[i].GetComponentsInChildren<SpriteRenderer>())
                        {
                            t.enabled = true;
                        }
                    }
                    else
                    {
                        foreach (SpriteRenderer t in Torsos[i].GetComponentsInChildren<SpriteRenderer>())
                        {
                            t.enabled = false;
                        }
                    }
                }
            }
            LastDir = SpriteDir;
        }
    }

    private class HandRoot
    {
        private Transform _Hand;
        private GameObject _Weapon;
        private List<Transform> Hands;

        public HandRoot(Transform Hand, GameObject Weapon, List<Transform> HandsList) {
            _Hand = Hand;
            _Weapon = Instantiate(Weapon);
            DestroyObject(_Weapon.GetComponent<Collider2D>());
            Hands = HandsList;
        }

        public void SwapHand(enemyDir Direction)
        {
            switch (Direction)
            {
                case enemyDir.Left:   { _Hand = Hands[0];  break; }
                case enemyDir.Down:   { _Hand = Hands[1]; break; }
                case enemyDir.Up:     { _Hand = Hands[2]; break; }
                case enemyDir.Right:  { _Hand = Hands[0]; break; }
            }

            _Weapon.transform.SetParent(_Hand.transform); //antaa uuden parent transformin
            _Weapon.transform.position = _Hand.transform.position; //siirtää aseen uuden käden sijaintiin
            WeaponSettings(); //antaa asekohtaiset asetukset mitä aseen scripteistä löytyy
        }

        private void WeaponSettings()
        {
            if(_Weapon.GetComponent<Melee>() != null) { _Weapon.GetComponent<Melee>().Reposition(_Hand); }
            else if (_Weapon.GetComponent<longMelee>() != null) { _Weapon.GetComponent<longMelee>().Reposition(_Hand); }
            else if (_Weapon.GetComponent<Ranged>() != null) { _Weapon.GetComponent<Ranged>().Reposition(_Hand); }
        }

    }
}
