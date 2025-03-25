using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack.asset", menuName = "Attack/BaseAttack")]
public class AttackDefinition : ScriptableObject
{
    [field: SerializeField]
    public float CoolDown {  get; private set; }
    [field: SerializeField]
    public float Range { get; private set; }
    [field: SerializeField]
    public float MinDamage { get; private set; }
    [field: SerializeField]
    public float MaxDamage { get; private set; }
    [field: SerializeField]
    public float CriticalChanse { get; private set; }
    [field: SerializeField]
    public float CriticalMultipler { get; private set; }

    public DamageInfo CreateAttack(CharactorStats charactorStats, CharactorStats targetStats)
    {
        DamageInfo damageInfo = new DamageInfo();
        float damage = charactorStats.GetStatValue(StatType.BasicAttackPower);
        damage += Random.Range(MinDamage, MaxDamage);
        damageInfo.critical = CriticalChanse > Random.value;

        if (damageInfo.critical)
        {
            damage*= CriticalMultipler;
        }
        damageInfo.damage = Mathf.RoundToInt(damage);

        if (targetStats != null)
        {
            damageInfo.damage -= charactorStats.GetStatValue(StatType.Defense);
        }
        return damageInfo;
    }

    public virtual void Execute(GameObject attacker, GameObject defender)
    {

    }
}
