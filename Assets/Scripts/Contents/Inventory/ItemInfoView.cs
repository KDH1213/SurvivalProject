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

        itemNameText.text = itemData.itemNameID.ToString();
        itemInfoText.text = itemData.itemDescID.ToString();

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
                itemStatsText.text = string.Empty;
                break;
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
        var armorData = DataTableManager.ArmorTable.Get(itemData.ID);
        itemStatsText.text = string.Format(armorFormat, armorData.defance.ToString(), armorData.moveSpeed.ToString());
    }

    private void SetWeaponInfo(ItemData itemData)
    {
        var weaponData = DataTableManager.WeaponTable.Get(itemData.ID);
        itemStatsText.text = string.Format(weaponFormat, weaponData.attack.ToString(), weaponData.attackSpeed.ToString());
    }

    private void SetConsumableInfo(ItemData itemData)
    {
        // "체력 : {0}\n포만감 : {1}\n수분 : {2}\n피로도 : {3}\n";
        itemStatsText.text = string.Format(consumableFormat, "0", "0", "0", "0");
    }
}
