using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatInfo
{
    public StatType statType;
    public float value;

    public StatInfo(StatType statType, float value)
    {
        this.statType = statType;
        this.value = value;
    }
}

[System.Serializable]
public class ItemData
{
    [field: SerializeField]
    public int ID {  get; set; }
    [field: SerializeField]
    public ItemType ItemType { get; set; }
    [field: SerializeField]
    public string ItemName { get; set; }
    [field: SerializeField]
    public string ItemInfomation { get; set; }
    [field: SerializeField]
    public Sprite ItemImage { get; set; }
    [field: SerializeField]
    public float MoveSpeed { get; set; }
    [field: SerializeField]
    public float Attack { get; set; }
    [field: SerializeField]
    public float Defense { get; set; }
    [field: SerializeField]
    public float AttackSpeed { get; set; }
    [field: SerializeField]
    public int MaxAmount { get; set; }
    [field: SerializeField]
    public bool IsMax { get; set; }

    public ItemData() { }

    public ItemData(ItemData data)
    {
        ItemType=data.ItemType;
        ItemName=data.ItemName;
        ItemInfomation=data.ItemInfomation;
        ItemImage=data.ItemImage;
        MoveSpeed=data.MoveSpeed;
        Attack=data.Attack;
        Defense=data.Defense;
        AttackSpeed=data.AttackSpeed;
        MaxAmount=data.MaxAmount;
        IsMax=data.IsMax;
    }

    public List<StatInfo> GetItemInfoList()
    {
        var list = new List<StatInfo>();

        if(Attack != 0f)
        {
            list.Add(new StatInfo(StatType.BasicAttackPower, Attack));
        }
        if (Defense != 0f)
        {
            list.Add(new StatInfo(StatType.Defense, Defense));
        }
        if (AttackSpeed != 0f)
        {
            list.Add(new StatInfo(StatType.AttackSpeed, AttackSpeed));
        }
        if (MoveSpeed != 0f)
        {
            list.Add(new StatInfo(StatType.MovementSpeed, MoveSpeed));
        }

        return list;
    }
}
