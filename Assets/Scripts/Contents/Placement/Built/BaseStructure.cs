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
        // SetData();
        Load();
    }

    private void Start()
    {
        if (returnCount >= maxRelics)
        {
            onMaxCollectRelicsEvent?.Invoke();
        }
    }

    public override void SetData()
    {
        var table = GetComponent<StructureStats>().CurrentStatTable;
        table.Clear();

        var hpStat = new StatValue(StatType.HP, Hp, maxHp);
        table.Add(StatType.HP, hpStat);

        hpStat.OnChangeValue += (hp) => Hp = hp;
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

        
        saveInfo.hp = GetComponent<StructureStats>().HP;
        saveInfo.id = ID;
        saveInfo.returnCount = returnCount;
        SaveLoadManager.Data.basePointerSaveInfo = saveInfo;
    }

    public override void Load()
    {
        var data = SaveLoadManager.Data.basePointerSaveInfo;

        var table = GetComponent<StructureStats>().CurrentStatTable;
        var hpStat = table[StatType.HP];
        hpStat.OnChangeValue += (hp) => Hp = hp;

        if (data != null)
        {
            Hp = data.hp;
            returnCount = data.returnCount;
            
            hpStat.SetValue(Hp);
        }

    }

    public override void Interact(GameObject interactor)
    {
        baseUIController.OnOpenBasePointUI(interactor, this);
    }
    public override void OnAttack(GameObject attacker, DamageInfo damageInfo)
    {
        return;
    }

}
