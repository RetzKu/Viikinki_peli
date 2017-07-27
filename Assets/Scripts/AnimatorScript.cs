using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
    private Rigidbody2D Player;
    bool Lock = false;
    public bool _Lock { set { Lock = value; } get { return Lock; } }
    private List<Animator> Animators;
    private float SpeedEdge = 0.3f;

    internal SpriteChanger Sprites;

    internal WeaponType Type;

    private List<GameObject> TorsoList;

    public GameObject Knob0;
    public GameObject Knob1;
    public GameObject Knob3;
    public GameObject Knob4;


    void Start()
    {
        /*Get RigidBody for velocity check*/
        Player = GetComponent<Rigidbody2D>();

        /*Make SpriteChanger*/
        TorsoList = new List<GameObject>(3);
        TorsoList.Add(transform.Find("s_c_torso").gameObject);
        TorsoList.Add(transform.Find("d_c_torso").gameObject);
        TorsoList.Add(transform.Find("u_c_torso").gameObject);

        Sprites = new SpriteChanger(transform, Player,TorsoList);

        /*Get Animators*/
        Animators = new List<Animator>(3);
        Animators.Add(transform.Find("s_c_torso").GetComponent<Animator>());  //0 Index
        Animators.Add(transform.Find("d_c_torso").GetComponent<Animator>());  //1 Index
        Animators.Add(transform.Find("u_c_torso").GetComponent<Animator>());  //2 Index

        Knob0 = Instantiate(Knob0);
        Knob1 = Instantiate(Knob1);
        Knob3 = Instantiate(Knob0);
        Knob4 = Instantiate(Knob1);

        Destroy(Knob0.GetComponent<BoxCollider2D>());
        Destroy(Knob1.GetComponent<BoxCollider2D>());
        Destroy(Knob3.GetComponent<BoxCollider2D>());
        Destroy(Knob4.GetComponent<BoxCollider2D>());

    }

    void Update()
    {
        Sprites.DirectionCheck();
        CheckVelocity();
        LookAt(((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized) + transform.position);
        if (Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            Attack();

        }
    }

    public int PlayerDir()
    {
        int tmp = (int)Sprites.Direction;
        return tmp;
    }

    void DirectionLock()
    {
        Vector3 Destination = new Vector3(Player.velocity.x, Player.velocity.y, 0) + transform.position;
        Destination = transform.position + (Destination - transform.position).normalized * 0.7f;
        Vector3 WrongWay = transform.position - (Destination - transform.position).normalized * 0.7f;
        Vector3 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        MousePoint.Normalize();
        MousePoint = MousePoint + transform.position;

        if(Vector3.Distance(MousePoint,WrongWay) < Vector3.Distance(MousePoint,Destination))
        {
            print("Hidasdutus");
            GetComponent<Movement>().Slowed = true;
            GetComponent<Movement>().Started = true;
        }

    }

    public void LookAt(Vector3 MouseDir)
    {
        Vector3 Up = new Vector3(transform.position.x,transform.position.y);
        Vector3 Down = new Vector3(transform.position.x, transform.position.y);
        Vector3 Right = new Vector3(transform.position.x, transform.position.y);
        Vector3 Left = new Vector3(transform.position.x, transform.position.y);

        Up.y += Vector3.Distance(transform.position, MouseDir);
        Down.y -= Vector3.Distance(transform.position, MouseDir);
        Right.x += Vector3.Distance(transform.position, MouseDir);
        Left.x -= Vector3.Distance(transform.position, MouseDir);

        float TmpDis = Vector3.Distance(Up, transform.position);
        if(Vector3.Distance(Down,transform.position) < TmpDis) { }
        if (Vector3.Distance(Left, transform.position) < TmpDis) { }
        if (Vector3.Distance(Right, transform.position) < TmpDis) { }
        if (TmpDis == Vector3.Distance(Up, transform.position)) { }

    }

    void CheckVelocity()
    {
        if (Player.velocity.x < -SpeedEdge || Player.velocity.y < -SpeedEdge || Player.velocity.x > SpeedEdge || Player.velocity.y > SpeedEdge)
        {
            foreach (Animator t in Animators) { t.SetBool(WalkType(), true); }
            foreach (Animator t in Animators) { t.SetBool("Walking", true); }
        }
        else
        {
            foreach (Animator t in Animators) { t.SetBool(WalkType(), false); }
            foreach (Animator t in Animators) { t.SetBool("Walking", false); }
        }
    }

    public void Attack()
    {
        DirectionLock();
        foreach (Animator t in Animators) { t.SetTrigger(AttackType()); }
        GetComponent<FxScript>().instantiateFx();
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

        public enum Directions { Left, Down, Up, Right }

        Transform PlayerTransform;
        List<SpriteRenderer[]> Sprites;
        Transform Torso;
        Rigidbody2D PlayerRb;

        private List<GameObject> Torsos;

        Directions LastDir;
        public Directions Direction;

        public SpriteChanger(Transform Player, Rigidbody2D Rb, List<GameObject> TorsoList)
        {
            PlayerTransform = Player; PlayerRb = Rb;
            Sprites = new List<SpriteRenderer[]>(3);
            Sprites.Add(PlayerTransform.Find("s_c_torso").GetComponentsInChildren<SpriteRenderer>());
            Sprites.Add(PlayerTransform.Find("d_c_torso").GetComponentsInChildren<SpriteRenderer>());
            Sprites.Add(PlayerTransform.Find("u_c_torso").GetComponentsInChildren<SpriteRenderer>());
            Torso = PlayerTransform.Find("s_c_torso");
            LastDir = Directions.Right;
            Torsos = TorsoList;
        }

        public void DirectionCheck()
        {
            if (PlayerRb.velocity.x > 0) // X positive
            {
                if (PlayerRb.velocity.x < PlayerRb.velocity.y) // 1,1
                {
                    Direction = Directions.Up; // ylös
                }
                else if (PlayerRb.velocity.x < PlayerRb.velocity.y * -1) //1,-1
                {
                    //spritesdown
                    Direction = Directions.Down; // alas
                }
                else
                {
                    Direction = Directions.Right; // oikea
                }
            }
            else if (PlayerRb.velocity.x < 0) // X negative
            {
                if (PlayerRb.velocity.x > PlayerRb.velocity.y * -1) // -1,1
                {
                    Direction = Directions.Up; // ylös
                }
                else if (PlayerRb.velocity.x > PlayerRb.velocity.y) //-1,-1
                {
                    Direction = Directions.Down; // alas
                }
                else
                {
                    Direction = Directions.Left; // vasen
                }
            }
            else if (PlayerRb.velocity.y != 0) // X negative
            {
                if (PlayerRb.velocity.y > 0) // -1,1
                {
                    Direction = Directions.Up; // ylös
                }
                else if (PlayerRb.velocity.y < 0) //-1,-1
                {
                    Direction = Directions.Down; // alas
                }
            }
            EnableSprites(Direction);
        }

        public void EnableSprites(Directions SpriteDir)
        {
            bool changed = false;

            if (SpriteDir != LastDir)
            {
                if (SpriteDir == Directions.Right)
                {
                    Torsos[0].GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else
                {
                    Torsos[0].GetComponent<Transform>().localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                for (int i = 0; i < 3; i++)
                {
                    if ((int)SpriteDir == i || SpriteDir == Directions.Right && changed == false)
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
}
