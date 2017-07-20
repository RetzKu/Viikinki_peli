using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChecker : MonoBehaviour
{

    public float CraftRange = 4f;

    public Action OnCampFireEnter;
    public Action OnCampFireExit;

    private bool lastFrameOnBase = false;

    void Start()
    {
        // 
    }

    void Update()
    {
        int mask = LayerMask.GetMask("Base");
        var hit = Physics2D.CircleCast(transform.position, CraftRange, Vector2.zero, 0f, mask);

        if (hit)
        {
                // TODO: Callback crafting thingy	        
                if (!lastFrameOnBase && OnCampFireEnter != null)
                {
                    print("Base on range");
                    OnCampFireEnter();
                    lastFrameOnBase = true;
                }
        }
        else
        {
            // TODO: Stop crafting, Ui, touchcontrols, hud

            if (lastFrameOnBase && OnCampFireExit != null)
            {
                OnCampFireExit();
                lastFrameOnBase = false;
                print("Base out of range");
            }
        }
    }
}
