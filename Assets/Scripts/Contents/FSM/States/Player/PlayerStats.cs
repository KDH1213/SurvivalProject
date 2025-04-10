using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharactorStats, ISaveLoadData
{
    [SerializeField]
    private PenaltyController survivalStats;

    [SerializeField]
    private LifeStat lifeStat;

    [SerializeField]
    private Slider HpBarSlider;

    private float currentSpeed = 1f;
    private float currentAttackSpeed = 1f;

    protected override void Awake()
    {
        originalData.CopyStat(ref currentStatTable);

        if (survivalStats == null)
        {
            survivalStats = GetComponent<PenaltyController>();
        }

        if (SaveLoadManager.Data != null)
        {
            Load();
        }

        currentStatTable[StatType.MovementSpeed].OnChangeValueAction(OnChangeSpeedValue);
        currentStatTable[StatType.AttackSpeed].OnChangeValueAction(OnChangeAttackSpeedValue);
        currentSpeed = currentStatTable[StatType.MovementSpeed].Value;
        currentAttackSpeed = currentStatTable[StatType.AttackSpeed].Value;
        OnChangeHp();

        lifeStat.OnChangeSkillLevelEvent.AddListener(OnChangeLifeStat);
    }

    public float Speed
    {
        get
        {
            return currentSpeed * survivalStats.CalculatePenaltySpeed();
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
            return currentAttackSpeed * survivalStats.CalculatePenaltyAttackSpeed();
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
    public void OnChangeHp() //*HP ����
    {
        HpBarSlider.value = Hp / currentStatTable[StatType.HP].MaxValue;
    }

    public void OnEquipmentItem(ItemData itemData)
    {
        var statInfoList = itemData.GetItemInfoList();

        foreach (var statInfo in statInfoList)
        {
            if (statInfo.statType == StatType.AttackSpeed)
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

    public void OnChangeLifeStat(LifeSkillType type)
    {
        switch (type)
        {
            case LifeSkillType.Damage:
                break;
            case LifeSkillType.MoveSpeed:
                currentSpeed = currentStatTable[StatType.MovementSpeed].Value + lifeStat.MoveSpeed;
                break;
            case LifeSkillType.AttackSpeed:
                currentAttackSpeed = currentStatTable[StatType.AttackSpeed].Value + lifeStat.AttackSpeed;
                break;
            case LifeSkillType.Hungur:
                break;
            case LifeSkillType.Thirst:
                break;
            case LifeSkillType.End:
                break;
            default:
                break;
        }
    }

    public void OnChangeSpeedValue(float value)
    {
        currentSpeed = value + lifeStat.MoveSpeed;
    }

    public void OnChangeAttackSpeedValue(float value)
    {
        currentAttackSpeed = value + lifeStat.AttackSpeed;
    }

    public void Save()
    {
        lifeStat.Save();
        survivalStats.Save();
    }

    public void Load()
    {
        // lifeStat.Load();
        originalData.StatTable[StatType.HP].SetValue(SaveLoadManager.Data.playerSaveInfo.hp);
    }
}
