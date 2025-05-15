using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureStats : CharactorStats, ISaveLoadData
{
    public float HP { get { return currentStatTable[StatType.HP].Value; } }

    protected override void Awake()
    {
    }

    private void Start()
    {
        //if (ObjectPoolManager.Instance.ObjectPoolTable.TryGetValue(ObjectPoolType.HpBar, out var component))
        //{
        //    var hpBarObjectPool = component.GetComponent<UIHpBarObjectPool>();
        //    var hpBar = hpBarObjectPool.GetHpBar();
        //    hpBar.GetComponent<UITargetFollower>().SetTarget(transform, Vector3.zero);
        //    hpBar.SetTarget(this);

        //    deathEvent.AddListener(() => { hpBar.gameObject.SetActive(false); });
        //}

        OnChangeHp();
    }

    public void OnChangeHp() //*HP °»½Å
    {
        // onChangeHpEvnet?.Invoke(currentStatTable[StatType.HP].PersentValue);

        //if (HP <= 0f)
        //{
        //    IsDead = true;
        //    HpBarSlider.gameObject.SetActive(false);
        //}
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

        //if(HpBarSlider != null)
        //{
        //    HpBarSlider.gameObject.SetActive(true);
        //    OnChangeHp();
        //}
    }

    public void Load()
    {
    }

    public void Save()
    {
    }
}
