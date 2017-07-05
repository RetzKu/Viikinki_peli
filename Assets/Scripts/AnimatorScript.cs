using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
    private Rigidbody2D Player;

    private List<Animator> Animators;
    private bool playerRun = false;
    private float SpeedEdge = 0.3f;

    private SpriteChanger Sprites;

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

    }

    void Update()
    {
        CheckVelocity();
        Sprites.DirectionCheck();
        Attack();
    }

    public int PlayerDir()
    {
        int tmp = Sprites.Index;
        return tmp;
    }

    void CheckVelocity()
    {
        if (Player.velocity.x < -SpeedEdge || Player.velocity.y < -SpeedEdge || Player.velocity.x > SpeedEdge || Player.velocity.y > SpeedEdge)
        {
            if (playerRun == false)
            {
                playerRun = true;
                foreach (Animator t in Animators) { t.SetBool("playerRun", playerRun); }
            }
        }
        else
        {
            if (playerRun == true)
            {
                playerRun = false;
                foreach (Animator t in Animators) { t.SetBool("playerRun", playerRun); }
            }
        }
        //Debug.Log("Movin state: " + playerRun);
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)==true)
        {
            transform.Find("s_c_torso").GetComponent<Animator>().SetTrigger("playerAttack");
        }
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
                    //spritesup
                    Index = 2;
                }
                else if (PlayerRb.velocity.x < PlayerRb.velocity.y * -1) //1,-1
                {
                    //spritesdown
                    Index = 1;
                }
                else
                {
                    //spritesright
                    Index = 3;
                }
            }
            else if (PlayerRb.velocity.x < 0) // X negative
            {
                if (PlayerRb.velocity.x > PlayerRb.velocity.y * -1) // -1,1
                {
                    //spritesup
                    Index = 2;
                }
                else if (PlayerRb.velocity.x > PlayerRb.velocity.y) //-1,-1
                {
                    //spritesdown
                    print("down");
                    Index = 1;
                }
                else
                {
                    //spritesleft
                    Index = 0;
                }
            }
            EnableSprites(Index);
        }
        void EnableSprites(int SpriteNum)
        {
            bool changed = false;
            if (SpriteNum != LastSpriteNum)
            {
                if (SpriteNum == 3)
                {
                    Torso.GetComponent<Transform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else
                {
                    Torso.GetComponent<Transform>().localScale = new Vector3(-1.0f, 1.0f, 1.0f);
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
