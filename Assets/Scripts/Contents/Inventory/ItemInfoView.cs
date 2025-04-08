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

    private readonly string armorFormat = "���� : {0}\n�̵� �ӵ� : {1}\n";
    private readonly string weaponFormat = "���ݷ� : {0}\n���� �ӵ� : {1}\n";
    private readonly string consumableFormat = "ü�� : {0}\n������ : {1}\n���� : {2}\n�Ƿε� : {3}\n";

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
