using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharactorStats
{
    [SerializeField]
    private PenaltyController survivalStats;

    [SerializeField]
    private LifeStat lifeStat;

    [SerializeField]
    private Slider HpBarSlider;

    protected override void Awake()
    {
        originalData.CopyStat(ref currentStatTable);

        if (survivalStats == null)
        {
            survivalStats = GetComponent<PenaltyController>();
        }

        if (SaveLoadManager.Data != null)
        {
            originalData.StatTable[StatType.HP].SetValue(SaveLoadManager.Data.playerSaveInfo.hp);
        }

        OnChangeHp();
    }

    public float Speed
    {
        get
        {
            return (currentStatTable[StatType.MovementSpeed].Value + lifeStat.MoveSpeed) * survivalStats.CalculatePenaltySpeed();
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
            return (currentStatTable[StatType.AttackSpeed].Value + lifeStat.AttackSpeed) * survivalStats.CalculatePenaltyAttackSpeed();
        }
    }

    public override float AttackPower
    {
        get
        {
            return currentStatTable[StatType.BasicAttackPower].Value + lifeStat.Damage;
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
        HpBarSlider.value = Hp / currentStatTable[StatType.HP].MaxValue;
    }

    public void OnEquipmentItem(ItemData itemData)
    {
        var statInfoList = itemData.GetItemInfoList();

        foreach (var statInfo in statInfoList)
        {
            if(statInfo.statType == StatType.AttackSpeed)
            {
                currentStatTable[statInfo.statType].SetValue(statInfo.value);
            }
            else
            {
                currentStatTable[statInfo.statType].AddValue(statInfo.value);
            }
        }
    }

    public void OnUnEquipmentItem(ItemData itemData)
    {
        var statInfoList = itemData.GetItemInfoList();

        foreach (var statInfo in statInfoList)
        {
            if (statInfo.statType == StatType.AttackSpeed)
            {
                currentStatTable[statInfo.statType].SetValue(originalData.StatTable[statInfo.statType].Value);
            }
            else
            {
                currentStatTable[statInfo.statType].AddValue(-statInfo.value);
            }
        }
    }
}
