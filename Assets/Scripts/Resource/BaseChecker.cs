using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChecker : MonoBehaviour {

    public float CraftRange = 4f;
	
	void Update ()
	{
	    int mask = LayerMask.GetMask("Base");
	    var hit = Physics2D.CircleCast(transform.position, CraftRange, Vector2.zero, mask);

	    if (hit)
	    {
            // TODO: Callback crafting thingy	        
	    }
	    else
	    {
            // TODO: Stop crafting, Ui, touchcontrols, hud
	    }
	}
}
