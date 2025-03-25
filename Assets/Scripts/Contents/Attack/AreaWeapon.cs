using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaWeapon.asset", menuName = "Attack/AreaWeapon")]
public class AreaWeapon : AttackDefinition
{
    public LayerMask attackLayer;

    [SerializeField]
    private Collider[] maxAttackCountColliders;
    [SerializeField]
    private GameObject vfxPrefab;

    public void StartAttack(GameObject owner)
    {
        var vfx = Instantiate(vfxPrefab, owner.transform.position, Quaternion.identity);
        Vector3 scale = Vector3.one * Range;
        scale.y = 1f;
        vfx.transform.localScale = scale;

        int count = Physics.OverlapSphereNonAlloc(owner.transform.position, Range * 0.5f, maxAttackCountColliders, attackLayer.value);
        CharactorStats targetStatus = null;
        for (int i = 0; i < count; ++i)
        {
            targetStatus = maxAttackCountColliders[i].GetComponent<CharactorStats>();

            if(targetStatus != null && owner != targetStatus.gameObject)
            {
                Execute(owner, targetStatus.gameObject);
            }
        }
    }

    public override void Execute(GameObject attacker, GameObject defender)
    {
        if (defender == null)
        {
            return;
        }

        var distance = Vector3.Distance(attacker.transform.position, defender.transform.position);

        if (distance > Range)
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
