using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemNameText;
    [SerializeField]
    private TextMeshProUGUI itemInfoText;
    [SerializeField]
    private TextMeshProUGUI itemStatsText;

    // private void OnSetItemInfo()

    private readonly string armorFormat = "방어력 : {0}\n이동 속도 : {1}\n";
    private readonly string weaponFormat = "공격력 : {0}\n공격 속도 : {1}\n";
    private readonly string consumableFormat = "체력 : {0}\n포만감 : {1}\n수분 : {2}\n피로도 : {3}\n";

    private void OnDisable()
    {
        Empty();
    }

    public void OnSetItemInfo(ItemData itemData)
    {
        if (itemData == null)
        {
            Empty();
            return;
        }

        itemNameText.text = itemData.ItemName;
        itemInfoText.text = itemData.ItemInfomation;

        switch (itemData.ItemType)
        {
            case ItemType.Helmet:
            case ItemType.Armor:
            case ItemType.Pants:
            case ItemType.Shoes:
                SetArmorInfo(itemData);
                break;
            case ItemType.Weapon:
                SetWeaponInfo(itemData);
                break;
            case ItemType.Consumable:
                SetConsumableInfo(itemData);
                break;
            case ItemType.Material:
            default:
                break;
        }
    }

    private void Empty()
    {
        itemNameText.text = string.Empty;
        itemInfoText.text = string.Empty;
        itemStatsText.text = string.Empty;
    }

    private void SetArmorInfo(ItemData itemData)
    {
        itemStatsText.text = string.Format(armorFormat, itemData.Defense, itemData.MoveSpeed);
    }

    private void SetWeaponInfo(ItemData itemData)
    {
        itemStatsText.text = string.Format(weaponFormat, itemData.Attack, itemData.AttackSpeed);
    }

    private void SetConsumableInfo(ItemData itemData)
    {
        itemStatsText.text = string.Format(consumableFormat, "0", "0", "0", "0");
    }
}
