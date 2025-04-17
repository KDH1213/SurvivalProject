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
        if (currentStatTable.Count == 0)
        {
            originalData.CopyStat(ref currentStatTable);
        }
        deathEvent.AddListener(() => 
        { 
            if (HpBarSlider != null)
                HpBarSlider.gameObject.SetActive(false);
        });
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

            HpBarSlider.gameObject.SetActive(true);
        }
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
        HpBarSlider.value = Hp / currentStatTable[StatType.HP].MaxValue;

        if(Hp <= 0f)
        {
            IsDead = true;
        }
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
