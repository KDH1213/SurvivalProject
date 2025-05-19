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
    public float damage;
    [SerializeField]
    private float attackRange;
    public float attackTerm;
    public float currentTime;
    private GameObject effect;
    [SerializeField]
    private TurretAttack atd;
    [SerializeField]
    private TurretType type;
    [SerializeField]
    private TurretAttackVerdict verdict;

    private void Update()
    {

        if(!IsPlaced || type != TurretType.Timing)
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

        verdict.SetInfo(atd, attackTerm, type, this);

        if (ObjectPoolManager.Instance.ObjectPoolTable.TryGetValue(ObjectPoolType.HpBar, out var component))
        {
            var stats = GetComponent<StructureStats>();
            var hpBarObjectPool = component.GetComponent<UIHpBarObjectPool>();
            var hpBar = hpBarObjectPool.GetHpBar();
            hpBar.GetComponent<UITargetFollower>().SetTarget(transform, Vector3.zero);
            hpBar.SetTarget(stats);

            stats.deathEvent.AddListener(() => { hpBar.gameObject.SetActive(false); });
            disableEvent.AddListener(() => { if (hpBar != null) { hpBar.gameObject.SetActive(false); } });
            enableEvent.AddListener(() => { if (hpBar != null) { hpBar.gameObject.SetActive(true); } });
        }
    }

    public override void Interact(GameObject interactor)
    {
        uiController.OnOpenTurretUI(this);
    }

    public override void Load()
    {
        var data = SaveLoadManager.Data.placementSaveInfoList.Find(x => x.position == Position && x.id == ID);
        Hp = data.hp;
        var table = GetComponent<StructureStats>().CurrentStatTable;
        table[StatType.HP].SetValue(Hp);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        monsters.Enqueue(collision.gameObject, Time.time + attackTerm);
    }

    private void OnCollisionExit(Collision collision)
    {
        monsters.re(collision.gameObject);
    }*/
}
