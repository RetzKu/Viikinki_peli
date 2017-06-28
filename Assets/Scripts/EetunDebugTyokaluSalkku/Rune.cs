﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class Rune : ScriptableObject 
{
    [SerializeField]
    public Vec2[] Indices;

    public int Length
    {
        get { return Indices.Length; }
    }
    public AudioClip Sound;
    public Sprite sprite;

    public bool ValidateRune(Vec2[] runeIndices)
    {
        // Count tarkistetaan RuneHolderissa !

        for (int i = 0; i < Length; i++)
        {
            if (Indices[i] != runeIndices[i])
            {
                return false;
            }
        }

        return true;
    }

    public abstract void init(GameObject owner);
    public abstract void Fire();

}
