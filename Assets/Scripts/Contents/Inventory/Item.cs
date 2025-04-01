using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item.asset", menuName = "Util/BaseItem")]
public class Item : ScriptableObject
{
    public enum itemType
    {
        Equipment,  // ���
        Consumable, // �Ҹ�ǰ
        Material    // ���
    }

    public itemType type;

    public string itemName;
    public string itemInfomation;

    public Sprite itemImage;
    public int amount;
}
