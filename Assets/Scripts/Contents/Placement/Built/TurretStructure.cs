using System.Linq;
using UnityEngine;

public class TurretStructure : PlacementObject
{
    private float damage;
    private float attackRange;
    [SerializeField]
    private float attackTerm;
    [SerializeField]
    private float currentTime;
    private GameObject effect;
    [SerializeField]
    private TurretAttack atd;

    private void Update()
    {

        if(!IsPlaced)
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
        TurretTable.Data data = DataTableManager.TurretTable.Get(ID);
        damage = data.turretAtkPower;
        attackRange = data.turretAtkRange;
        attackTerm = data.turretAtkSpeed;
        currentTime = Time.time + attackTerm;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 8);
    }
}
