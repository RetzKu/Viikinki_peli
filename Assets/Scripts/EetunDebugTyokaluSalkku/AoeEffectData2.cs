using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runes/testDat")]
public class AoeEffectData2 : ScriptableObject
{
    public Buff BuffToApply;
    public float ExpansionTime;
    public float TotalRotation;
    public bool LeavesEffectArea;
    public float StartScale;
    public float EndScale;
    public int Frames;
    public Rune AfterEffect;
    public Vector2 MovementDir;
    public float Speed;
}
