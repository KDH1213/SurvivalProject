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
    public UnityEvent<int> onChangeReturnRelicsCountEvent;


    public bool IsMaxCollectRelics => returnCount >= maxRelics;

    private void OnEnable()
    {
        SetData();
        Load();
    }

    private void Start()
    {
        var table = GetComponent<StructureStats>().CurrentStatTable;
        if (returnCount >= maxRelics)
        {
            onMaxCollectRelicsEvent?.Invoke();
        }

        if (ObjectPoolManager.Instance.ObjectPoolTable.TryGetValue(ObjectPoolType.HpBar, out var component))
        {
            var stats = GetComponent<StructureStats>();
            var hpBarObjectPool = component.GetComponent<UIHpBarObjectPool>();
            var hpBar = hpBarObjectPool.GetHpBar();
            hpBar.GetComponent<UITargetFollower>().SetTarget(transform, Vector3.zero);
            hpBar.SetTarget(stats);

            stats.deathEvent.AddListener(() => { hpBar.gameObject.SetActive(false); });
        }
    }

    public override void SetData()
    {
        var table = GetComponent<StructureStats>().CurrentStatTable;
        Position = Vector3Int.zero;
        Rotation = Quaternion.identity;
        IsPlaced = true;
        Rank = -1;
        ID = -1;
        Hp = table[StatType.HP].MaxValue;
        table[StatType.HP].SetValue(Hp);
    }

    public void OnReturnRelicsCount(int count)
    {
        returnCount += count;
        onChangeReturnRelicsCountEvent?.Invoke(count);

        if (returnCount >= maxRelics)
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

        if (data != null && data.id == -1)
        {
            Hp = data.hp;
            returnCount = data.returnCount;
            
            hpStat.SetValue(Hp);
        }
        GetComponent<StructureStats>().damegedEvent.AddListener(
            () => SoundManager.Instance.OnSFXPlay(transform, (int)SoundType.BulidingHit));
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
