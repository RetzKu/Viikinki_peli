using UnityEngine;

[CreateAssetMenu(menuName = "Runes/Buff/Player/DamageBoost")]
public class PlayerBuff : Buff
{
    public float DamageBoost;
    public float MovementSpeedBoost;
    public float ArmorBoost;
    public float RangedDamageBoost;
    public Sprite HudIndicator;


    public Vector3 EnlargePlayerSize;
    // duration

    public override void Apply(GameObject target)
    {
        if (target.GetComponent<PlayerBuffHandler>())
        {
        }
        else
        {
            var handler = target.AddComponent<PlayerBuffHandler>();
            handler.ApplyPlayerBuff(this, target);
        }
    }
}

[CreateAssetMenu(menuName = "Runes/Buff/Player/Flask")]
public class PlayerHeal : Buff
{
    public float TotalHealAmount;
    public int NumberOfTicks;

    public override void Apply(GameObject target)
    {
        if (target.GetComponent<PlayerBuffHandler>())
        {
        }
        else
        {
            var handler = target.AddComponent<PlayerBuffHandler>();
            handler.HealPlayer(target, TotalHealAmount, NumberOfTicks, Duration);
        }
    }
}
