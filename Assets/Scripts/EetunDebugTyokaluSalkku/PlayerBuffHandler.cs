using System.Collections;
using UnityEngine;

public class PlayerBuffHandler : MonoBehaviour
{
    public void ApplyPlayerBuff(PlayerBuff buff, GameObject target)
    {
        StartCoroutine(StartBuff(buff, target));
    }

    public void HealPlayer(GameObject target, float totatHealAmount, int numberOfTicks, float duration)
    {
        StartCoroutine(Flask(target, totatHealAmount, numberOfTicks, duration));
    }

    IEnumerator StartBuff(PlayerBuff buff, GameObject target)
    {
        Vector3 startScale = target.transform.localScale;
        target.transform.localScale = buff.EnlargePlayerSize;


        print("buffed");
        var stats = target.GetComponent<combat>();
        float startD = stats.dmgBase;
        float startA = stats.armor;
        float startR = stats.rangedBaseDmg;

        stats.dmgBase += buff.DamageBoost;
        stats.armor += buff.ArmorBoost;
        stats.rangedBaseDmg += buff.RangedDamageBoost;

        var movement = target.GetComponent<Movement>();
        // movement.

        yield return new WaitForSeconds(buff.Duration);

        stats.dmgBase -= buff.DamageBoost;
        stats.armor -= buff.ArmorBoost;
        stats.rangedBaseDmg -= buff.RangedDamageBoost;

        Debug.LogFormat("alku: {0}, {1}, {2} loppu: {3}, {4}, {5}", startD, startA, startR, stats.dmgBase, stats.armor, stats.rangedBaseDmg);


        target.transform.localScale = startScale;
        Destroy(this);
    }

    IEnumerator Flask(GameObject target, float totalHealAmount, int numberOfTicks, float duration)
    {
        var stats = target.GetComponent<combat>();
        float healAmount = totalHealAmount / numberOfTicks;
        float tickRate = duration / numberOfTicks;

        for(int i = 0; i < numberOfTicks; i++)
        {
            stats.hp += healAmount;
            if (stats.hp > 100f) // TODO: PElaajan health voi vaihtua
            {
                stats.hp = 100f;
            }

            print("healed for " + healAmount);
            ParticleSpawner.instance.CastSpell(this.gameObject);
            yield return new WaitForSeconds(tickRate);
        }

        Destroy(this);
    }
}
