using Unity.VisualScripting;
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
    private float currentDamage = 1f;
    private float currentDefence = 0f;

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
        currentStatTable[StatType.BasicAttackPower].OnChangeValueAction(OnChangeDamageValue);
        currentStatTable[StatType.Defense].OnChangeValueAction(OnChangeDefenceValue);

        currentSpeed = currentStatTable[StatType.MovementSpeed].Value;
        currentAttackSpeed = currentStatTable[StatType.AttackSpeed].Value;
        currentDamage = currentStatTable[StatType.BasicAttackPower].Value;
        currentDefence = currentStatTable[StatType.Defense].Value;

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
            return currentStatTable[StatType.HP].Value + lifeStat.HP;
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
            return currentDamage;
        }
    }

    public float Defense
    {
        get
        {
            return currentStatTable[StatType.Defense].Value + lifeStat.Defence;
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

    public void OnUseItem(ItemData itemData)
    {
        var itemUseEffectInfoList = itemData.ItemUseEffectInfoList;

        int count = itemUseEffectInfoList.Count;

        for (int i = 0; i < count; ++i)
        {
            switch (itemUseEffectInfoList[i].itemUseEffectType)
            {
                case ItemUseEffectType.None:
                    break;
                case ItemUseEffectType.Hp:
                    currentStatTable[StatType.HP].AddValue(itemUseEffectInfoList[i].value);
                    OnChangeHp();
                    break;
                case ItemUseEffectType.Fatigue:
                    survivalStats.PenaltyTable[SurvivalStatType.Fatigue].SubPenaltyValue(itemUseEffectInfoList[i].value);
                    break;
                case ItemUseEffectType.Hunger:
                    survivalStats.PenaltyTable[SurvivalStatType.Hunger].AddPenaltyValue(itemUseEffectInfoList[i].value);
                    break;
                case ItemUseEffectType.Thirst:
                    survivalStats.PenaltyTable[SurvivalStatType.Thirst].AddPenaltyValue(itemUseEffectInfoList[i].value);
                    break;
                default:
                    break;
            }
        }
    }

    public void OnChangeLifeStat(LifeSkillType type)
    {
        switch (type)
        {
            case LifeSkillType.Damage:
                currentDamage = currentStatTable[StatType.BasicAttackPower].Value + lifeStat.Damage;
                break;
            case LifeSkillType.MoveSpeed:
                currentSpeed = currentStatTable[StatType.MovementSpeed].Value + lifeStat.MoveSpeed;
                break;
            case LifeSkillType.AttackSpeed:
                currentAttackSpeed = currentStatTable[StatType.AttackSpeed].Value + lifeStat.AttackSpeed;
                break;
            case LifeSkillType.Hungur:
                GetComponent<HungerStat>().OnSetHungerSkillValue(lifeStat.Hungur);
                break;
            case LifeSkillType.Thirst:
                GetComponent<ThirstStat>().OnSetThirstSkillValue(lifeStat.Thirst);
                break;
            case LifeSkillType.Defence:
                currentDefence = currentStatTable[StatType.Defense].Value + lifeStat.Defence;
                break;
            case LifeSkillType.HP:
                currentStatTable[StatType.HP].AddMaxValue(5f);
                break;
            case LifeSkillType.Fatigue:
                GetComponent<FatigueStat>().OnSetFatigueSkillValue(lifeStat.Fatigue);
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

    public void OnChangeDamageValue(float value)
    {
        currentDamage = value + lifeStat.Damage;
    }

    public void OnChangeDefenceValue(float value)
    {
        currentDefence = value + lifeStat.Defence;
    }


    public void Save()
    {
        lifeStat.Save();
        survivalStats.Save();
    }

    public void Load()
    {
        // lifeStat.Load();

        if(SaveLoadManager.Data.levelStatInfo.skillLevelList.Count >(int)LifeSkillType.HP)
        {
            currentStatTable[StatType.HP].AddMaxValue(SaveLoadManager.Data.levelStatInfo.skillLevelList[(int)LifeSkillType.HP] * 5f);
        }

        currentStatTable[StatType.HP].SetValue(SaveLoadManager.Data.playerSaveInfo.hp);

        var equipmentItemIDList = SaveLoadManager.Data.equipmentItemIDList;

        for (int i = 0; i < equipmentItemIDList.Count; ++i)
        {
            if (equipmentItemIDList[i] == -1)
            {
                continue;
            }

            // 해당 부분 SaveLoad에 따라 ItemData 값 다르게 세팅
            var itemData = DataTableManager.ItemTable.Get(equipmentItemIDList[i]);
            OnEquipmentItem(itemData);
        }

    }
}
