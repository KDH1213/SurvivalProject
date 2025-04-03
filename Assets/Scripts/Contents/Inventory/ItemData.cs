using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item.asset", menuName = "Util/BaseItem")]
public class ItemData : ScriptableObject
{
    public itemType type;

    public string ItemName => itemName;
    public string ItemInfomation => itemInfomation;
    public Sprite ItemImage => itemImage;
    public int Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    public int MaxAmount => maxAmount;

    public bool IsMax = false;

    [SerializeField] private string itemName;    // ������ �̸�
    [Multiline]
    [SerializeField] private string itemInfomation; // ������ ����
    [SerializeField] private Sprite itemImage;
    [SerializeField] private int amount;
    [SerializeField] private int maxAmount;
}
