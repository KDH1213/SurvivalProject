using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack.asset", menuName = "Attack/BaseAttack")]
public class AttackDefinition : ScriptableObject
{

    public DamageInfo CreateAttack(CharactorStats charactorStats, CharactorStats targetStats)
    {
        DamageInfo damageInfo = new DamageInfo();
        float damage = charactorStats.AttackPower;
        damageInfo.damage = damage;

        if (targetStats != null)
        {
            damageInfo.damage -= targetStats.GetStatValue(StatType.Defense);
            damageInfo.damage = Mathf.Max(1f, damageInfo.damage);
        }
        return damageInfo;
    }

    public virtual void Execute(GameObject attacker, GameObject defender)
    {

    }
}
