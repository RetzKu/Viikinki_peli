using UnityEngine;

[System.Serializable]
public class AoeEffectData
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

