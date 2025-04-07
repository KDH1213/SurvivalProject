using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    [field: SerializeField]
    public itemType ItemType { get; set; }
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
    public float Defence { get; set; }
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
        Defence=data.Defence;
        AttackSpeed=data.AttackSpeed;
        MaxAmount=data.MaxAmount;
        IsMax=data.IsMax;
    }
}
