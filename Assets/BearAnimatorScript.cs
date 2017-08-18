using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAnimatorScript : MonoBehaviour
{
    private Animator AnimatorCP; //animator
    private Transform WolfTransform; //transform

    public float scale;
    bool flip = false;
    float Rotation = 0;

    void Awake()
    {
        AnimatorCP = transform.GetComponent<Animator>();
        WolfTransform = transform;
        transform.localScale = new Vector3(-scale, scale, 1);
    }

    void Update()
    {
    }

    public void AnimationState(action State)
    {
        switch (State)
        {
            case action.Moving: { AnimatorCP.SetBool("Moving", true); break; }
            case action.Idle: { AnimatorCP.SetBool("Moving", false); break; }
        }
    }
    public void AnimationTrigger(action Action)
    {
        switch (Action)
        {
            case action.Attack:
                {
                    StartCoroutine(GetComponent<BearStats>().attack());
                    
                    AnimatorCP.SetTrigger("Attack");
                    break;
                }
            case action.Roar:
                {
                    GetComponent<Animator>().SetTrigger("Roar");
                    break;
                }
            case action.Dead:
                {
                    GetComponent<Animator>().SetBool("Moving", false);
                    GetComponent<Animator>().SetBool("Dead", true);

                    break;
                }
        }

    }
    public void SpriteDirection(enemyDir Dir)
    {
        
        switch (Dir)
        {
            case enemyDir.Right:     { flip = true;  Rotation = 0;   AnimatorCP.SetBool("Moving", true); break; }
            case enemyDir.StillR:    { flip = true;  Rotation = 0;   AnimatorCP.SetBool("Moving", false); break; }
            case enemyDir.Left:      { flip = false; Rotation = 0;   AnimatorCP.SetBool("Moving", true); break; }
            case enemyDir.StillL:    { flip = false; Rotation = 0;   AnimatorCP.SetBool("Moving", false); break; }
            case enemyDir.LD:        { flip = false; Rotation = 45;  AnimatorCP.SetBool("Moving", true); break; }
            case enemyDir.StillLD:   { flip = false; Rotation = 45;  AnimatorCP.SetBool("Moving", false); break; }
            case enemyDir.LU:        { flip = false; Rotation = -45; AnimatorCP.SetBool("Moving", true); break; }
            case enemyDir.StillLU:   { flip = false; Rotation = -45; AnimatorCP.SetBool("Moving", false); break; }
            case enemyDir.RU:        { flip = true;  Rotation = 45;  AnimatorCP.SetBool("Moving", true); break; }
            case enemyDir.StillRU:   { flip = true;  Rotation = 45;  AnimatorCP.SetBool("Moving", false); break; }
            case enemyDir.RD:        { flip = true;  Rotation = -45; AnimatorCP.SetBool("Moving", true); break; }
            case enemyDir.StillRD:   { flip = true;  Rotation = -45; AnimatorCP.SetBool("Moving", false); break; }


        }
       
        if (flip == true) { transform.localScale = new Vector3(-scale, scale, 1); }
        else { transform.localScale = new Vector3(scale, scale, 1); }
        transform.localEulerAngles = new Vector3(0, 0, Rotation);
    }
    
}
