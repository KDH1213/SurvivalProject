using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float MoveSpeed => moveSpeed;
    public float Attack => attack;
    public float Defence => defence;
    public float AttackSpeed => attackSpeed;

    public int Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    public int MaxAmount => maxAmount;

    public bool IsMax = false;

    [SerializeField] 
    private string itemName;    // 아이템 이름
    [SerializeField] 
    private string itemInfomation; // 아이템 설명
    [SerializeField]
    private Sprite itemImage;
    [SerializeField]
    private int amount;
    [SerializeField] 
    private int maxAmount;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float attack;
    [SerializeField]
    private float defence;
    [SerializeField]
    private float attackSpeed;
}
