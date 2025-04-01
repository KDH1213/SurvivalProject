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
        Equipment,  // 장비
        Consumable, // 소모품
        Material    // 재료
    }

    public itemType type;

    public string itemName;
    public string itemInfomation;

    public Sprite itemImage;
    public int amount;
}
