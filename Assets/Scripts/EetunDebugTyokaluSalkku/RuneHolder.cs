using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneHolder : MonoBehaviour
{
    public Rune[] runes;

	void Start ()
    {
        foreach (var rune in runes)
        {
		    rune.init(this.gameObject);
        }
	}
	
	void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    runes[0].Fire();
        //}
	}

    public void SendIndices(Vec2[] positions, int realSize)
    {
        for (int i = 0; i < realSize; i++)
        {
            print(positions[i].X + " Y: " + positions[i].Y);
        }

        foreach (var rune in runes)
        {
            if (rune.Length == realSize && rune.ValidateRune(positions))
            {
                rune.Fire();
                break;
            }
        }
    }
}
