using System;
using UnityEngine;

public abstract class Rune : ScriptableObject
{
    [SerializeField]
    public Vec2[] Indices;

    public int Length
    {
        get { return _runTimeIndices.Length; }
    }

    public AudioClip Sound;
    public Sprite sprite;
    public Sprite HudImage;
    public Sprite OnCdHudImage;
    public float Cd;

    private Vec2[] _runTimeIndices;

    public void InitInternalIndices()
    {
        _runTimeIndices = TouchController.GenerateInBetweenPositions(Indices, Indices.Length);

        //string s = "";
        //foreach (var v in _runTimeIndices)
        //{
        //    s += "(" + v.X + ", " + v.Y + ") ";
        //}
        //Debug.LogWarning(s);
    }

    public bool ValidateRune(Vec2[] runeIndices)
    {
        bool value1 = true, value2 = true;
        for (int i = 0; i < Length; i++)
        {
            if (_runTimeIndices[i] != runeIndices[i])
            {
                value1 = false;
            }
        }

        for (int i = 0; i < Length; i++)
        {
            if (_runTimeIndices[Length - 1 - i] != runeIndices[i])
            {
                value2 = false;
            }
        }

        return value2 || value1;
    }

    public bool ValidateRune(bool[] runeIndices, int[] touches)
    {
        int[] indiceCounts = new int[9];
        for (int i = 0; i < Indices.Length; i++)
        {
            indiceCounts[TouchController.GetBoolIndex(Indices[i])]++;
        }

        bool value = true;
        for (int i = 0; i < touches.Length; i++)
        {
            if (indiceCounts[i] != touches[i])
            {
                value = false;
                break;
            }
        }
        // Debug.Log("Rune validated!");
        return value;
    }

    public abstract void init(GameObject owner);
    public abstract void Fire();

    public virtual void OnGui(RuneBarUiController ui, int i)
    {
        ui.OnCd(i, Cd, OnCdHudImage);
    }

    public Sprite GetGuiImage()
    {
        return HudImage;
    }
}
