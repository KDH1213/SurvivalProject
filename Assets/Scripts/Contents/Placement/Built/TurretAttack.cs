using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttack : AttackDefinition
{
    public GameObject weaponPrefeb;

    public override void Execute(GameObject attacker, GameObject defender)
    {
        if (defender == null)
            return;

        var distance = Vector3.Distance(attacker.transform.position, defender.transform.position);
        if (distance > Range)
            return;

        Vector3 toTarget = (defender.transform.position - attacker.transform.position).normalized;
        float dot = Vector3.Dot(attacker.transform.forward, toTarget);
        if (dot < 0.5f)
        {
            return;
        }

        CharactorStats aStats = attacker.GetComponent<CharactorStats>();
        CharactorStats dStats = defender.GetComponent<CharactorStats>();
        DamageInfo attack = CreateAttack(aStats, dStats);

        IAttackable[] attackables = defender.GetComponents<IAttackable>();
        foreach (IAttackable attackable in attackables)
        {
            attackable.OnAttack(attacker, attack);
        }

    }
}
