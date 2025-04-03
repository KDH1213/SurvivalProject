using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharactorStats
{
    [SerializeField]
    private PenaltyController survivalStats;

    [SerializeField]
    private LifeStat lifeStat;

    [SerializeField]
    private CraftingStat craftingStat;

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
            return currentStatTable[StatType.AttackSpeed].Value * survivalStats.CalculatePenaltyAttackSpeed();
        }
    }

    public float AttackPower
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
        if(currentStatTable.TryGetValue(type, out var statValue))
        {
            statValue.AddValue(addValue);
        }
    }
    public void OnChangeHp() //*HP °»½Å
    {
        HpBarSlider.value = Hp / currentStatTable[StatType.HP].MaxValue;
    }

    public void SetAttackPowerValue(float amount)
    {
        originalData.StatTable[StatType.BasicAttackPower].AddValue(amount);
    }

    public void SetDefenceValue(float amount)
    {
        originalData.StatTable[StatType.Defense].AddValue(amount);
    }

    public void SetMoveSpeedValue(float amount)
    {
        originalData.StatTable[StatType.MovementSpeed].AddValue(amount);
    }

    public void SetAttackSpeedValue(float amount)
    {
        originalData.StatTable[StatType.AttackSpeed].AddValue(amount);
    }
}
