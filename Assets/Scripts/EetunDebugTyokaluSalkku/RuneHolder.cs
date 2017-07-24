using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class RuneHolder : MonoBehaviour
{
    public enum OwnerType
    {
        Player, PlayerCrafting, Enemy
    }

    public OwnerType Ownertype;
    public Rune[] runes;
    public bool IsOwnerPlayer = true;

    private RuneBarUiController _runeBarUiController;
    private bool[] CanCast;
    private float[] cds;

    void Start()
    {
        foreach (var rune in runes)
        {
            rune.init(this.gameObject);
        }

        // ReSharper disable once AssignmentInConditionalExpression
        if (null == (_runeBarUiController = GameObject.FindWithTag("RuneBarUi").GetComponent<RuneBarUiController>()))
        {
            Debug.LogError("RuneHolder.cs: Cannot Find RuneBarUiController");
        }

        CanCast = new bool[runes.Length];
        cds = new float[runes.Length];
    }

    void Update()
    {
        for (int i = 0; i < cds.Length; i++)
        {
            cds[i] -= Time.deltaTime;
            if (cds[i] <= 0f)
            {
                CanCast[i] = true;
            }
        }
    }

    public void SendIndices(Vec2[] positions, int realSize)
    {
        for (int i = 0; i < realSize; i++)
        {
            print(positions[i].X + " Y: " + positions[i].Y);
        }

        int ii = 0;
        foreach (var rune in runes)
        {
            if (rune.Length == realSize && rune.ValidateRune(positions))
            {
                if (Ownertype != OwnerType.Enemy)
                {
                    if (CanCast[ii])
                    {
                        rune.Fire();
                        CanCast[ii] = false;
                        cds[ii] = rune.Cd;

                        if (OwnerType.Player == Ownertype)
                        {
                            rune.OnGui(_runeBarUiController, ii);
                            ParticleSpawner.instance.CastSpell(gameObject);
                        }
                    }
                }
                else
                {
                    rune.Fire();
                }
                break;
            }
            ii++;
        }
    }

    public Sprite[] GetHudImages()
    {
        Sprite[] images = new Sprite[runes.Length];
        for (int i = 0; i < runes.Length; i++)
        {
            images[i] = runes[i].HudImage;
        }
        return images;
    }
}
