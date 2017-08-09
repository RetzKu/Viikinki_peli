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
    public List<Rune> runes;
    public bool IsOwnerPlayer = true;

    private RuneBarUiController _runeBarUiController;

    private List<bool> CanCast;
    private List<float> cds;

    public void AddRune(Rune rune)
    {
        rune.init(this.gameObject);
        runes.Add(rune);
        CanCast.Add(true);
        cds.Add(0f);
    }

    void Start()
    {
        for(int i = 0; i < runes.Count; i++)
        {
            if (runes[i] != null)
            {
                runes[i].init(this.gameObject);
            }
            else
            {
                runes.RemoveAt(i);
                break;
            }
        }

        // ReSharper disable once AssignmentInConditionalExpression
        if (null == (_runeBarUiController = GameObject.FindWithTag("RuneBarUi").GetComponent<RuneBarUiController>()))
        {
            Debug.LogError("RuneHolder.cs: Cannot Find RuneBarUiController");
        }

        // CanCast = new bool[runes.Count];
        // cds = new float[runes.Count];

        CanCast = new List<bool>(runes.Count);
        cds = new List<float>(runes.Count);

        for (int i = 0; i < runes.Count; i++)
        {
            CanCast.Add(true);
            cds.Add(0f);
        }
    }

    void Update()
    {
        for (int i = 0; i < cds.Count; i++)
        {
            cds[i] -= Time.deltaTime;
            if (cds[i] <= 0f)
            {
                CanCast[i] = true;
            }
        }
    }

    public void SendIndices(Vec2[] positions, int realSize ) 
    {
        for (int i = 0; i < realSize; i++)
        {
            print(positions[i].X + " Y: " + positions[i].Y);
        }

        int ii = 0;
        foreach (var rune in runes)
        {
            if (rune.Length == realSize && rune.ValidateRune(positions ))
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

    public void SendIndices(bool[] positions, int[] touchCounts)
    {
        int indiceCount = 0;
        for (int i = 0; i < touchCounts.Length; i++)
        {
            if (touchCounts[i] != 0)
                indiceCount += touchCounts[i];
        }


        //for (int i = 0; i < positions.Length; i++)
        //{
        //    print(i + ": " + positions[i]);
        //    if (positions[i])
        //        indiceCount++;
        //}

        int ii = 0;
        foreach (var rune in runes)
        {
            if (rune.Length == indiceCount && rune.ValidateRune(positions, touchCounts))
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
        Sprite[] images = new Sprite[runes.Count];
        for (int i = 0; i < runes.Count; i++)
        {
            images[i] = runes[i].HudImage;
        }
        return images;
    }
}
