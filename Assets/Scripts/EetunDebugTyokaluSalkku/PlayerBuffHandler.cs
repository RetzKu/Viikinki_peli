using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerBuffHandler : MonoBehaviour
{
    public void ApplyPlayerBuff(PlayerBuff buff, GameObject target)
    {
        StartCoroutine(StartBuff(buff, target));
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

    IEnumerator Flask(GameObject target, PlayerBuff buff)
    {
        var stats = target.GetComponent<combat>();

        while (true)
        {
            stats.hp += 1f;
            yield return null;
        }
    }
}
