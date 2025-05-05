using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureStats : CharactorStats, ISaveLoadData
{
    public float HP { get { return currentStatTable[StatType.HP].Value; } }

    [SerializeField]
    private Slider HpBarSlider;
    protected override void Awake()
    {
    }

    private void Start()
    {
        //if (ObjectPoolManager.Instance.ObjectPoolTable.TryGetValue(ObjectPoolType.HpBar, out var component))
        //{
        //    var hpBarObjectPool = component.GetComponent<UIHpBarObjectPool>();
        //    var hpBar = hpBarObjectPool.GetHpBar();
        //    hpBar.GetComponent<UITargetFollower>().SetTarget(transform, Vector3.down * 2f);
        //    hpBar.SetTarget(this);

        //    deathEvent.AddListener(() => { hpBar.gameObject.SetActive(false); });
        //}

        OnChangeHp();
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
    }

    public void OnRepair(float repair)
    {
        IsDead = false;
        currentStatTable[StatType.HP].SetValue(repair);
    }

    private void OnEnable()
    {
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
