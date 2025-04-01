using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon.asset", menuName = "Attack/Weapon")]
public class Weapon : AttackDefinition
{
    [SerializeField]
    public GameObject weaponPrefab;

    [field: SerializeField]
    public Vector3 CreateOffset { get; private set; }

    [field: SerializeField]
    public Vector3 CreateSize { get; private set; }

    [HideInInspector]
    public Collider[] attackTargets =  new Collider[5];

    [SerializeField]
    public LayerMask WeaponLayerMask;

    [field: SerializeField]
    public Collider[] AttackTargets = new Collider[0];

    public void StartAttack(Transform onwer)
    {
        int index = Physics.OverlapBoxNonAlloc(onwer.position + CreateOffset, CreateSize, AttackTargets, onwer.transform.rotation, WeaponLayerMask);

        for (int i = 0; i < index; ++i)
        {
            var target = AttackTargets[i].GetComponent<CharactorStats>();
            if (target != null)
            {
                Execute(onwer.gameObject, target.gameObject);
            }
        }
    }

    public override void Execute(GameObject attacker, GameObject defender)
    {
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
