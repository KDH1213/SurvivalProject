using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon.asset", menuName = "Attack/PlayerWeapon")]
public class PlayerWeapon : AttackDefinition
{

    [field: SerializeField]
    public Vector3 CreateSize { get; private set; }

    [SerializeField]
    public Collider[] FindTargetColliders = new Collider[5];

    [SerializeField]
    public LayerMask WeaponLayerMask;

    [SerializeField]
    public Collider[] AttackTargets = new Collider[20];

    private PriorityQueue<CharactorStats, float> attackTagetQueue = new PriorityQueue<CharactorStats, float>();

    [SerializeField]
    private int maxAttackCount = 5;

    public void StartAttack(Transform attackPoint, GameObject owner)
    {
        int index = Physics.OverlapBoxNonAlloc(attackPoint.position, CreateSize, AttackTargets, owner.transform.rotation, WeaponLayerMask);

        if(index <= maxAttackCount)
        {
            for (int i = 0; i < index; ++i)
            {
                var target = AttackTargets[i].GetComponent<CharactorStats>();

                if (target != null)
                {
                    Execute(owner.gameObject, target.gameObject);
                }
            }
        }
        else
        {
            CharactorStats target = null;

            for (int i = 0; i < index; ++i)
            {
                target = AttackTargets[i].GetComponent<CharactorStats>();


            }

            for (int i = 0; i < maxAttackCount; ++i)
            {
                target = attackTagetQueue.Dequeue();

                if (target != null)
                {
                    Execute(owner.gameObject, target.gameObject);
                }
            }
        }

        attackTagetQueue.Clear();
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
