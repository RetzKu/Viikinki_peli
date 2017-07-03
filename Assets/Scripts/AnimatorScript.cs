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
        Debug.Log("Movin state: " + playerRun);
    }

    public class SpriteChanger
    {

        enum Directions { Left, Right, Down, Up }

        Transform PlayerTransform;
            List<SpriteRenderer[]> Sprites;
        Rigidbody2D PlayerRb;

        public SpriteChanger(Transform Player, Rigidbody2D Rb)
        {
            PlayerTransform = Player; PlayerRb = Rb;
            Sprites = new List<SpriteRenderer[]>(3);
            Sprites.Add(PlayerTransform.Find("s_c_torso").GetComponentsInChildren<SpriteRenderer>());
            Sprites.Add(PlayerTransform.Find("d_c_torso").GetComponentsInChildren<SpriteRenderer>());
            Sprites.Add(PlayerTransform.Find("u_c_torso").GetComponentsInChildren<SpriteRenderer>());
        }

        void DirectionCheck()
        {
            if (PlayerRb.velocity.x > 0) // X positive
            {
                if (PlayerRb.velocity.x < PlayerRb.velocity.y) // 1,1
                {
                    //spritesup
                }
                if (PlayerRb.velocity.x < PlayerRb.velocity.y * -1) //1,-1
                {
                    //spritesdown
                }
                else
                {
                    //spritesright
                }
            }
            if (PlayerRb.velocity.x < 0) // X negative
            {
                if (PlayerRb.velocity.x > PlayerRb.velocity.y * -1) // -1,1
                {
                    //spritesup
                }
                if (PlayerRb.velocity.x > PlayerRb.velocity.y) //-1,-1
                {
                    //spritesdown
                }
                else
                {
                    //spritesleft

                }
            }
        }
    }
}
