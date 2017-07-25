using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
    private Rigidbody2D Player;
    private List<Animator> Animators;
    private float SpeedEdge = 0.3f;

    internal SpriteChanger Sprites;

    internal WeaponType Type;

    private Vector3 MovementDir;

    public GameObject Knob0;
    public GameObject Knob1;

    void Start()
    {
        /*Get RigidBody for velocity check*/
        Player = GetComponent<Rigidbody2D>();

        /*Make SpriteChanger*/
        Sprites = new SpriteChanger(transform, Player);

        /*Get Animators*/
        Animators = new List<Animator>(3);
        Animators.Add(transform.Find("s_c_torso").GetComponent<Animator>());  //0 Index
        Animators.Add(transform.Find("d_c_torso").GetComponent<Animator>());  //1 Index
        Animators.Add(transform.Find("u_c_torso").GetComponent<Animator>());  //2 Index

        Knob0 = Instantiate(Knob0);
        Knob1 = Instantiate(Knob1);

        Destroy(Knob0.GetComponent<BoxCollider2D>());
        Destroy(Knob1.GetComponent<BoxCollider2D>());

    }

    void Update()
    {
        Sprites.DirectionCheck();
        CheckVelocity();
        Attack();
        DirectionLock();
    }

    public int PlayerDir()
    {
        int tmp = Sprites.Index;
        return tmp;
    }

    void DirectionLock()
    {
        Vector3 NormalizedVelocity = new Vector3(Player.velocity.x, Player.velocity.y, 0).normalized;
        Vector3 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition).normalized;
        bool triggered = false;

        Vector2 RightEdge = new Vector2(NormalizedVelocity.y * -1, NormalizedVelocity.x) * 0.7f;
        Vector2 LeftEdge = new Vector2(NormalizedVelocity.y, NormalizedVelocity.x * -1) * 0.7f;

        Vector2 WrongWay = new Vector2(NormalizedVelocity.x, NormalizedVelocity.y) * -1;

        if(MousePoint.y < 0)
        {
            if (MousePoint.x > 0)
            {
                if(MousePoint.x > WrongWay.x && MousePoint.y < WrongWay.y)
                {

                }
            }
        }
        else
        {

        }
   
        Knob0.transform.position = RightEdge + (Vector2)transform.position;
        Knob1.transform.position = LeftEdge + (Vector2)transform.position;
        //Debug.Log(RightEdge + "right side");
        //Debug.Log(LeftEdge + "left side");
    }

    void CheckVelocity()
    {
        if (Player.velocity.x < -SpeedEdge || Player.velocity.y < -SpeedEdge || Player.velocity.x > SpeedEdge || Player.velocity.y > SpeedEdge)
        {
            foreach (Animator t in Animators) { t.SetBool(WalkType(), true); }
            foreach(Animator t in Animators) { t.SetBool("Walking", true); }
        }
        else
        {
            foreach (Animator t in Animators) { t.SetBool(WalkType(), false); }
            foreach (Animator t in Animators) { t.SetBool("Walking", false); }
        }
    }

    void Attack()
    {
        if (GetComponent<combat>().attackBoolean() == true)
        {
            foreach (Animator t in Animators) { t.SetTrigger(AttackType()); }
            GetComponent<FxScript>().instantiateFx();
        }
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
        switch(Type)
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

        enum Directions { Left, Right, Down, Up }

        Transform PlayerTransform;
        List<SpriteRenderer[]> Sprites;
        Transform Torso;
        Rigidbody2D PlayerRb;

        int LastSpriteNum;
        public int Index;

        public SpriteChanger(Transform Player, Rigidbody2D Rb)
        {
            PlayerTransform = Player; PlayerRb = Rb;
            Sprites = new List<SpriteRenderer[]>(3);
            Sprites.Add(PlayerTransform.Find("s_c_torso").GetComponentsInChildren<SpriteRenderer>());
            Sprites.Add(PlayerTransform.Find("d_c_torso").GetComponentsInChildren<SpriteRenderer>());
            Sprites.Add(PlayerTransform.Find("u_c_torso").GetComponentsInChildren<SpriteRenderer>());
            Torso = PlayerTransform.Find("s_c_torso");
        }

        public void DirectionCheck()
        {
            if (PlayerRb.velocity.x > 0) // X positive
            {
                if (PlayerRb.velocity.x < PlayerRb.velocity.y) // 1,1
                {
                    Index = 2; // ylös
                }
                else if (PlayerRb.velocity.x < PlayerRb.velocity.y * -1) //1,-1
                {
                    //spritesdown
                    Index = 1; // alas
                }
                else
                {
                    Index = 3; // oikea
                }
            }
            else if (PlayerRb.velocity.x < 0) // X negative
            {
                if (PlayerRb.velocity.x > PlayerRb.velocity.y * -1) // -1,1
                {
                    Index = 2; // ylös
                }
                else if (PlayerRb.velocity.x > PlayerRb.velocity.y) //-1,-1
                {
                    Index = 1; // alas
                }
                else
                {
                    Index = 0; // vasen
                }
            }
            else if (PlayerRb.velocity.y != 0) // X negative
            {
                if (PlayerRb.velocity.y > 0) // -1,1
                {
                    Index = 2; // ylös
                }
                else if (PlayerRb.velocity.y < 0) //-1,-1
                {
                    Index = 1; // alas
                }
            }
            EnableSprites(Index);
        }

        public void EnableSprites(int SpriteNum)
        {
            bool changed = false;
            if (SpriteNum != LastSpriteNum)
            {
                if (SpriteNum == 0)
                {
                    Torso.GetComponent<Transform>().localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    Torso.GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                for (int i = 0; i < 3; i++)
                {
                    if (SpriteNum == i || SpriteNum == 3 && changed == false)
                    {
                        changed = true;
                        foreach (SpriteRenderer t in Sprites[i])
                        {
                            t.enabled = true;
                        }
                    }
                    else
                    {
                        foreach (SpriteRenderer t in Sprites[i])
                        {
                            t.enabled = false;
                        }
                    }
                }
            }
            LastSpriteNum = SpriteNum;
        }
    }
}
