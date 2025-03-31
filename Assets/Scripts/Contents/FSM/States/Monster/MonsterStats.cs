using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStats : CharactorStats
{
    [SerializeField]
    private Slider HpBarSlider;

    protected override void Awake()
    {

        if(currentStatTable.Count == 0)
        {
            originalData.CopyStat(ref currentStatTable);
            OnChangeHp();
        }
        
    }

    private void OnEnable()
    {
        if (currentStatTable.Count == 0)
        {
            originalData.CopyStat(ref currentStatTable);
        }

        currentStatTable[StatType.HP].SetValue(currentStatTable[StatType.HP].MaxValue);
        OnChangeHp();
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

    public float AttackPower
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
        HpBarSlider.value = Hp / currentStatTable[StatType.HP].MaxValue;
    }

    public void LoadStats(float hp)
    {
        if (currentStatTable.Count == 0)
        {
            originalData.CopyStat(ref currentStatTable);
        }

        currentStatTable[StatType.HP].SetValue(hp);
        OnChangeHp();
    }
}
