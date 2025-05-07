using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseStructure : PlacementObject
{
    [SerializeField]
    private PlacementUIController baseUIController;

    public float maxHp;
    [SerializeField]
    private int returnCount;
    public int ReturnCount => returnCount;
    [SerializeField]
    private int maxRelics;
    public int MaxRelics => maxRelics;

    public UnityEvent onMaxCollectRelicsEvent;


    public bool IsMaxCollectRelics => returnCount >= maxRelics;

    private void OnEnable()
    {
        SetData();
        Load();
    }

    public override void SetData()
    {
        var table = GetComponent<StructureStats>().CurrentStatTable;
        table.Clear();
        table.Add(StatType.HP, new StatValue(StatType.HP, maxHp));
    }

    public void OnReturnRelicsCount(int count)
    {
        returnCount += count;

        if(returnCount >= maxRelics)
        {
            onMaxCollectRelicsEvent?.Invoke();
        }
    }

    public override void Save()
    {
        if (SaveLoadManager.Data == null)
        {
            return;
        }

        var saveInfo = new BasePointerSaveInfo();
        saveInfo.hp = Hp;
        saveInfo.id = ID;
        saveInfo.returnCount = returnCount;
        SaveLoadManager.Data.basePointerSaveInfo = saveInfo;
    }

    public override void Load()
    {
        var data = SaveLoadManager.Data.basePointerSaveInfo;
        Hp = data.hp;
        returnCount = data.returnCount;
        var table = GetComponent<StructureStats>().CurrentStatTable;
        table.Clear();
        table.Add(StatType.HP, new StatValue(StatType.HP, Hp));
    }

    public override void Interact(GameObject interactor)
    {
        baseUIController.OnOpenBasePointUI(interactor, this);
    }
    public override void OnAttack(GameObject attacker, DamageInfo damageInfo)
    {
        base.OnAttack(attacker, damageInfo);
        // 거점 파괴 이후 처리
    }
}
