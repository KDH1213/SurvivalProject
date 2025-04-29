using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon.asset", menuName = "Attack/Weapon")]
public class Weapon : AttackDefinition
{
    [field: SerializeField]
    public Vector3 CreateOffset { get; private set; }

    [field: SerializeField]
    public Vector3 CreateSize { get; private set; }

    [SerializeField]
    public Collider[] FindTargetColliders = new Collider[5];

    [SerializeField]
    public LayerMask WeaponLayerMask;

    [SerializeField]
    public Collider[] AttackTargets = new Collider[0];

    public void StartAttack(Transform attackPoint, GameObject owner)
    {
        int index = Physics.OverlapBoxNonAlloc(attackPoint.position, CreateSize, AttackTargets, owner.transform.rotation, WeaponLayerMask);

        for (int i = 0; i < index; ++i)
        {
            var target = AttackTargets[i].GetComponent<CharactorStats>();
            if (target != null)
            {
                Execute(owner.gameObject, target.gameObject);
            }
        }
    }

    public override void Execute(GameObject attacker, GameObject defender)
    {
        CharactorStats aStats = attacker.GetComponent<CharactorStats>();
        CharactorStats dStats = defender.GetComponent<CharactorStats>();

        if(dStats.IsDead || !dStats.CanHit)
        {
            return;
        }

        DamageInfo attack = CreateAttack(aStats, dStats);

        IAttackable[] attackables = defender.GetComponents<IAttackable>();

        foreach (var attackable in attackables)
        {
            attackable.OnAttack(attacker, attack);
        }
    }

    public void OnGizmos(Transform owner)
    {
        Gizmos.color = Color.red;
        Color prevColor = Gizmos.color;
        Matrix4x4 prevMatrix = Gizmos.matrix;

        Gizmos.color = Color.red;
        Gizmos.matrix = owner.localToWorldMatrix;

        Vector3 boxPosition = owner.position;

        // convert from world position to local position 
        boxPosition = owner.InverseTransformPoint(boxPosition);

        Gizmos.DrawWireCube(boxPosition, CreateSize);

        // restore previous Gizmos settings
        Gizmos.color = prevColor;
        Gizmos.matrix = prevMatrix;

    }
}
