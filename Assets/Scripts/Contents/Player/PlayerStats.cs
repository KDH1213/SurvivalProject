using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharactorStats
{
    [SerializeField]
    private SurvivalStats survivalStats;

    protected override void Awake()
    {
        originalData.CopyStat(ref currentStatTable);

        if (survivalStats == null)
        {
            survivalStats = GetComponent<SurvivalStats>();
        }
    }

    public float Speed 
    { 
        get
        {
            return currentStatTable[StatType.MovementSpeed].Value * survivalStats.CalculateSpeedDebuff();
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
            return currentStatTable[StatType.AttackSpeed].Value * survivalStats.CalculateAttackSpeedDebuff();
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
        if(currentStatTable.TryGetValue(type, out var statValue))
        {
            statValue.AddValue(addValue);
        }
    }
}
