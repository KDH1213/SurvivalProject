using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StructureStats : CharactorStats, ISaveLoadData
{
    public float HP { get { return currentStatTable[StatType.HP].Value; } }

    [SerializeField]
    private Slider HpBarSlider;
    protected override void Awake()
    {
    }

    public void OnChangeHp() //*HP °»½Å
    {
        HpBarSlider.value = HP / currentStatTable[StatType.HP].MaxValue;

        if (HP <= 0f)
        {
            IsDead = true;
            HpBarSlider.gameObject.SetActive(false);
        }
    }

    public void OnDestoryStructure()
    {
        IsDead = true;
        deathEvent?.Invoke();
    }

    public void OnRepair(float repair)
    {
        IsDead = false;
        currentStatTable[StatType.HP].SetValue(repair);
    }

    private void OnEnable()
    {
        if(!IsDead)
        {
            return;
        }
        currentStatTable[StatType.HP].SetValue(currentStatTable[StatType.HP].MaxValue);
        IsDead = false;

        if(HpBarSlider != null)
        {
            HpBarSlider.gameObject.SetActive(true);
            OnChangeHp();
        }
    }

    public void Load()
    {
    }

    public void Save()
    {
    }
}
