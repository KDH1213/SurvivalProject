using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon.asset", menuName = "Attack/Weapon")]
public class Weapon : AttackDefinition
{
    public GameObject weaponPrefab;

    [SerializeField]
    public LayerMask weaponLayerMask;

    [SerializeField]
    [Range(0f, 1f)]
    public float attackDot;

    public override void Execute(GameObject attacker, GameObject defender)
    {
        if(defender == null)
        {
            return;
        }

        var distance = Vector3.Distance(attacker.transform.position, defender.transform.position);

        if(distance > Range)
        {
            return;
        }

        var toTarget = (defender.transform.position - attacker.transform.position).normalized;
        float dot = Vector3.Dot(attacker.transform.forward, toTarget);

        if(dot < attackDot)
        {
            return;
        }

        CharactorStats aStats = attacker.GetComponent<CharactorStats>();
        CharactorStats dStats = defender.GetComponent<CharactorStats>();

        DamageInfo attack = CreateAttack(aStats, dStats);

        IAttackable[] attackables = defender.GetComponents<IAttackable>();

        foreach (var attackable in attackables)
        {
            attackable.OnAttack(attacker, attack);
        }
    }
}
