using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public itemType ItemType { get; set; }

    public string ItemName { get; set; }
    public string ItemInfomation { get; set; }
    public Sprite ItemImage { get; set; }
    public float MoveSpeed { get; set; }
    public float Attack { get; set; }
    public float Defence { get; set; }
    public float AttackSpeed { get; set; }
    public int MaxAmount { get; set; }

    public bool IsMax { get; set; }
}
