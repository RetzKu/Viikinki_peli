using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class longMelee : weaponStats
{
    public Sprite Fx;
	void Start ()
    {
		transform.Find("Player").GetComponent<FxFade>
	}

    // täällä ois animaatio pitkälle meleelle

}
