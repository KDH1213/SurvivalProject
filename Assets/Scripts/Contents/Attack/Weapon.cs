using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon.asset", menuName = "Attack/Weapon")]
public class Weapon : AttackDefinition
{
    [SerializeField]
    public GameObject weaponPrefab;

    [SerializeField]
    public LayerMask WeaponLayerMask;

    [field: SerializeField]
    public Collider[] AttackTargets = new Collider[0];

    [SerializeField]
    [Range(0f, 1f)]
    public float attackDot;

    public void FindAttackTarget(Transform onwer)
    {
        //int index = Physics.OverlapSphereNonAlloc(onwer.position, Range, AttackTargets, WeaponLayerMask);

        //for (int i = 0; i < index; ++i)
        //{
        //    var target = AttackTargets[i].GetComponent<CharactorStats>();
        //    if (target != null)
        //    {
        //        MonsterFSM.Weapon.Execute(gameObject, target.gameObject);
        //    }
        //}
    }

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
