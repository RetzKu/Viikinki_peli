using UnityEngine;

[CreateAssetMenu(menuName = "Runes/Buff/Aibuff/Slow")]
public class AiSlow : Buff
{
    public float Time;
    public float SlowPercent;

    public override void Apply(GameObject target)
    {
        var generalAi = target.GetComponent<generalAi>(); 
        generalAi.SlowRune(Time, SlowPercent);
    }
}
