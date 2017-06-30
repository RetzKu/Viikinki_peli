using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{
    private Rigidbody2D Player;

    private List<Animator> Animators;
        private bool playerRun = false;
        private float SpeedEdge = 0.3f;
        

    void Start()
    {
        /*Get RigidBody for velocity check*/
        Player = GetComponent<Rigidbody2D>();

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
}
