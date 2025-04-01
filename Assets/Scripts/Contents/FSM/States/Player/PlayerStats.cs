using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharactorStats
{
    [SerializeField]
    private PenaltyController survivalStats;

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
            return currentStatTable[StatType.MovementSpeed].Value * survivalStats.CalculatePenaltySpeed();
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
            return currentStatTable[StatType.AttackSpeed].Value * survivalStats.CalculatePenaltyAttackSpeed();
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
    public void OnChangeHp() //*HP °»½Å
    {
        HpBarSlider.value = Hp / currentStatTable[StatType.HP].MaxValue;
    }
}
