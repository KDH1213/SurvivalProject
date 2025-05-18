using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon.asset", menuName = "Attack/PlayerWeapon")]
public class PlayerWeapon : AttackDefinition
{
    [field: SerializeField]
    public Vector3 CreateSize { get; private set; }

    [SerializeField]
    public LayerMask WeaponLayerMask;

    [SerializeField]
    public Collider[] AttackTargets = new Collider[20];

    [SerializeField]
    public LayerMask wallLayerMask;

    private PriorityQueue<CharactorStats, float> attackTagetQueue = new PriorityQueue<CharactorStats, float>();

    [SerializeField]
    private int maxAttackCount = 5;

    private Ray ray = new Ray();
    private RaycastHit[] raycastHit = new RaycastHit[1];

    public void StartAttack(Transform attackPoint, GameObject owner)
    {
        int index = Physics.OverlapBoxNonAlloc(attackPoint.position, CreateSize, AttackTargets, owner.transform.rotation, WeaponLayerMask);

        ray.origin = owner.transform.position;

        attackTagetQueue.Clear();

        if (index <= maxAttackCount)
        {
            for (int i = 0; i < index; ++i)
            {
                var target = AttackTargets[i].GetComponent<CharactorStats>();

                if (target != null)
                {
                    Excute(owner, target);
                }
            }
        }
        else
        {
            CharactorStats target = null;

            for (int i = 0; i < index; ++i)
            {
                target = AttackTargets[i].GetComponent<CharactorStats>();

                if (target == null || target.IsDead || !target.CanHit)
                {
                    continue;
                }
                attackTagetQueue.Enqueue(target, (target.transform.position - owner.transform.position).sqrMagnitude);
            }

            int attackCount = 0;
            for (int i = 0; i < attackTagetQueue.Count; ++i)
            {
                target = attackTagetQueue.Dequeue();

                if (Excute(owner, target))
                {
                    ++attackCount;
                }

                if (attackCount == maxAttackCount)
                {
                    break;
                }
            }
        }
    }

    public override void Execute(GameObject attacker, GameObject defender)
    {
        CharactorStats aStats = attacker.GetComponent<CharactorStats>();
        CharactorStats dStats = defender.GetComponent<CharactorStats>();

        if (dStats.IsDead || !dStats.CanHit)
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

    public bool Excute(GameObject attacker, CharactorStats dStats)
    {
        CharactorStats aStats = attacker.GetComponent<CharactorStats>();

        var direction = (ray.origin - attacker.transform.position);

        ray.direction = direction.normalized;
        direction.y = 0;
        float distance = direction.magnitude;

        if (Physics.RaycastNonAlloc(ray, raycastHit, distance, wallLayerMask) != 0)
        {
            return false;
        }

        DamageInfo attack = CreateAttack(aStats, dStats);
        IAttackable[] attackables = dStats.GetComponents<IAttackable>();

        foreach (var attackable in attackables)
        {
            attackable.OnAttack(attacker, attack);
        }


        return true;
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
