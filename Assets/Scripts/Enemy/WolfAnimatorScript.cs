using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimatorScript : MonoBehaviour
{

    private Animator WolfAnimator;
    private Transform WolfTransform;
    public float scale;
    bool flip = false;
    int Rotation = 0;
    void Awake()
    {
        WolfAnimator = transform.GetComponent<Animator>();
        WolfTransform = transform;
        AnimationState(action.Moving);
    }

    void Update()
    {
        if(WolfAnimator.GetBool("Moving") == true)
        {
            GetComponent<wolfStats>().Crittable = true;
        }
        else { GetComponent<wolfStats>().Crittable = false; }
    }

    public void AnimationState(action State)
    {
        switch(State)
        {
            case action.Moving: { WolfAnimator.SetBool("Moving", true); break; }
            case action.Idle:   { WolfAnimator.SetBool("Moving", false); break; }
        }
    }
    public void AnimationTrigger(action Action)
    {
        switch(Action)
        {
            case action.Attack:
                {
                    WolfAnimator.SetTrigger("Attack");
                    if(GetComponent<wolfStats>() != null)
                    {
                        GetComponent<wolfStats>().attack();
                    }
                    else
                    {
                        Debug.LogWarning("Missing object wolfStats.cs");
                    }
                    break;
                }
            case action.LeapStart:   { WolfAnimator.SetBool("Leap", true); break; }
            case action.LeapEnd:     { WolfAnimator.SetBool("Leap", false); break; }
        }
    }
    public void SpriteDirection(enemyDir Dir)
    {
        
        switch(Dir)
        {
            case enemyDir.Right: { flip = true;     Rotation = 0; WolfAnimator.SetBool("Moving", true); break; }
            case enemyDir.Left:  { flip = false;    Rotation = 0; WolfAnimator.SetBool("Moving", true); break; }
            case enemyDir.LD:    { flip = false;    Rotation = 45; WolfAnimator.SetBool("Moving", true); break; }
            case enemyDir.LU:    { flip = false;    Rotation = -45; WolfAnimator.SetBool("Moving", true); break; }
            case enemyDir.RU:    { flip = true;     Rotation = 45; WolfAnimator.SetBool("Moving", true); break; }
            case enemyDir.RD:    { flip = true;     Rotation = -45; WolfAnimator.SetBool("Moving", true); break; }
            case enemyDir.StillR: { flip = true; WolfAnimator.SetBool("Moving", false); break; }
            case enemyDir.StillL: { flip = false; WolfAnimator.SetBool("Moving", false); break; }
            case enemyDir.StillRU: { flip = true; Rotation = 45; WolfAnimator.SetBool("Moving", false); break; }
            case enemyDir.StillRD: { flip = true; Rotation = -45; WolfAnimator.SetBool("Moving", false); break; }
            case enemyDir.StillLU: { flip = false; Rotation = -45; WolfAnimator.SetBool("Moving", false); break; }
            case enemyDir.StillLD: { flip = false; Rotation = 45; WolfAnimator.SetBool("Moving", false); break; }


        }
        if (flip == true) { transform.localScale = new Vector3(-scale, scale, 1); }
        else { transform.localScale = new Vector3(scale, scale, 1); }

        transform.localEulerAngles = new Vector3(0, 0, Rotation);
    }
}
