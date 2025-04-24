using System;
using System.Linq;
using UnityEngine;

[Serializable]
public enum TurretType
{
    Continuous,
    Timing
}

public class TurretStructure : PlacementObject
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float attackTerm;
    [SerializeField]
    private float currentTime;
    private GameObject effect;
    [SerializeField]
    private TurretAttack atd;
    [SerializeField]
    private TurretType type;

    private void Update()
    {

        if(!IsPlaced || type == TurretType.Timing)
        {
            return;
        }

        if (Time.time >= currentTime)
        {
            currentTime = Time.time + attackTerm;
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, GetLayerMasks.Monster);
            
            if (colliders.Length > 0)
            {
                var target = colliders.OrderBy(item => Vector3.Distance(transform.position, item.transform.position)).First();
                atd.Execute(gameObject, target.gameObject);
            }
        }
    }

    public override void SetData()
    {
        var table = GetComponent<StructureStats>().CurrentStatTable;
        table.Clear();
        table.Add(StatType.HP, new StatValue(StatType.HP, Hp, Hp));
        table.Add(StatType.BasicAttackPower, new StatValue(StatType.BasicAttackPower, damage));
        table.Add(StatType.AttackSpeed, new StatValue(StatType.AttackSpeed, attackTerm));
        table.Add(StatType.AttackRange, new StatValue(StatType.AttackRange, attackRange));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
