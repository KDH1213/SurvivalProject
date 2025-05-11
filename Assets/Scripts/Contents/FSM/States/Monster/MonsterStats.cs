using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : CharactorStats
{
    [field: SerializeField]
    public Vector3 HpBarOffsetPosition { private set; get; }


    protected override void Awake()
    {
        if (currentStatTable.Count == 0)
        {
            CreateStat();
        }
    }


    private void Start()
    {
        if (ObjectPoolManager.Instance.ObjectPoolTable.TryGetValue(ObjectPoolType.HpBar, out var component))
        {
            var hpBarObjectPool = component.GetComponent<UIHpBarObjectPool>();
            var hpBar = hpBarObjectPool.GetHpBar();
            hpBar.GetComponent<UITargetFollower>().SetTarget(transform, HpBarOffsetPosition);
            hpBar.SetTarget(this);

            deathEvent.AddListener(() => { hpBar.gameObject.SetActive(false); });
        }

        OnChangeHp();
    }

    public void OnInitialize()
    {
        currentStatTable.Clear();
        originalData.CopyStat(ref currentStatTable);
    }

    private void OnEnable()
    {
        if(IsDead)
        {
            currentStatTable[StatType.HP].SetValue(currentStatTable[StatType.HP].MaxValue);
            IsDead = false;
        }
        OnChangeHp();
    }

    private void OnDisable()
    {
        deathEvent.RemoveAllListeners();
    }


    public float Speed
    {
        get
        {
            return currentStatTable[StatType.MovementSpeed].Value;
        }
    }
    public float Hp
    {
        get
        {
            return currentStatTable[StatType.HP].Value;
        }
    }

    public float AttackSpeed
    {
        get
        {
            return currentStatTable[StatType.AttackSpeed].Value;
        }
    }

    public override float AttackPower
    {
        get
        {
            return currentStatTable[StatType.BasicAttackPower].Value;
        }
    }

    public float Defense
    {
        get
        {
            return currentStatTable[StatType.Defense].Value;
        }
    }

    public void AddStatType(StatType type, float addValue)
    {
        if (currentStatTable.TryGetValue(type, out var statValue))
        {
            statValue.AddValue(addValue);
        }
    }


    public void OnChangeHp() //*HP °»½Å
    {
        onChangeHpEvnet?.Invoke(currentStatTable[StatType.HP].PersentValue);

        if (Hp <= 0f)
        {
            IsDead = true;
        }
    }

    private void CreateStat()
    {
        currentStatTable.Clear();
        var monsterData = DataTableManager.MonsterTable.Get(GetComponent<MonsterFSM>().ID);

        currentStatTable.Add(StatType.HP, new StatValue(StatType.HP, monsterData.HP, monsterData.HP));
        currentStatTable.Add(StatType.BasicAttackPower, new StatValue(StatType.BasicAttackPower, monsterData.AttackPower, monsterData.AttackPower));
        currentStatTable.Add(StatType.Defense, new StatValue(StatType.Defense, monsterData.Defense, monsterData.Defense));
        currentStatTable.Add(StatType.MovementSpeed, new StatValue(StatType.MovementSpeed, monsterData.MovementSpeed, monsterData.MovementSpeed));
        currentStatTable.Add(StatType.AttackSpeed, new StatValue(StatType.AttackSpeed, monsterData.AttackSpeed, monsterData.AttackSpeed));
        currentStatTable.Add(StatType.AttackRange, new StatValue(StatType.AttackRange, monsterData.AttackRadius, monsterData.AttackRadius));
    }

    public void LoadStats(float hp)
    {
        if (currentStatTable.Count == 0)
        {
            CreateStat();
        }

        currentStatTable[StatType.HP].SetValue(hp);
        OnChangeHp();
    }
}
