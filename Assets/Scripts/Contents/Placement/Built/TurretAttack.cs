using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret.asset", menuName = "Attack/TurretAttack")]
public class TurretAttack : AttackDefinition
{
    public GameObject weaponPrefeb;
    private float curTime;

    public override void Execute(GameObject attacker, GameObject defender)
    { 
        if (defender == null)
            return;

        CharactorStats aStats = attacker.GetComponent<CharactorStats>();
        CharactorStats dStats = defender.GetComponent<CharactorStats>();

        var distance = Vector3.Distance(attacker.transform.position, defender.transform.position);
        if (distance > aStats.CurrentStatTable[StatType.AttackRange].Value)
            return;

        DamageInfo attack = CreateAttack(aStats, dStats);

        IAttackable[] attackables = defender.GetComponents<IAttackable>();
        foreach (IAttackable attackable in attackables)
        {
            attackable.OnAttack(attacker, attack);
        }

    }
}
