using UnityEngine;

[CreateAssetMenu(menuName = "Runes/Buff/Aibuff/Debuff")]
public class AiDebuff : Buff
{
    public float Time;
    public float SlowPercent;

    public override void Apply(GameObject target)
    {
        target.GetComponent<generalAi>().SlowRune(Time, SlowPercent);
        // Debug.Log("Slow buff lähettää terveisensä! \nVihollinen "+ target.gameObject.name + " slowtatu");
    }
}

