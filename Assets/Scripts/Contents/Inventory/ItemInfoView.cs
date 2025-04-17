using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI itemNameText;
    [SerializeField]
    private TextMeshProUGUI itemInfoText;
    [SerializeField]
    private TextMeshProUGUI itemStatsText;

    [SerializeField]
    private Button useButton;
    [SerializeField]
    private Button equipButton;
    [SerializeField]
    private Button eraseButton;
    [SerializeField]
    private Button unEquiptButton;

    // private void OnSetItemInfo()

    private readonly string armorFormat = "방어력 : {0}\n이동 속도 : {1}\n";
    private readonly string weaponFormat = "공격력 : {0}\n공격 속도 : {1}\n";
    private readonly string consumableFormat = "체력 : {0}\n포만감 : {1}\n수분 : {2}\n피로도 : {3}\n";

    private void OnDisable()
    {
        Empty();
    }

    private void OnSetItemInfo(ItemData itemData, bool isItemSlot)
    {
        if (itemData == null)
        {
            Empty();
            return;
        }

        itemNameText.text = itemData.NameID.ToString();
        itemInfoText.text = itemData.DescriptID.ToString();

        if(!isItemSlot)
        {
            equipButton.interactable = false;
        }
        else
        {
            unEquiptButton.interactable = false;
        }

        if (itemData.IsDisposible == 1)
        {
            eraseButton.interactable = true;
        }

        switch (itemData.ItemType)
        {
            case ItemType.Material:
                itemStatsText.text = string.Empty;
                useButton.interactable = false;
                equipButton.interactable = false;
                break;
            case ItemType.Consumable:
                SetConsumableInfo(itemData, isItemSlot);
                break;
            case ItemType.Relics:
                break;
            case ItemType.Armor:
                SetArmorInfo(itemData, isItemSlot);
                break;
            case ItemType.Weapon:
                SetWeaponInfo(itemData, isItemSlot);
                break;
            default:
                break;
        }
    }

    public void OnSetItemSlotInfo(ItemData itemData)
    {
        OnSetItemInfo(itemData, true);
    }

    public void OnSetEquipmentItemInfo(ItemData itemData)
    {
        OnSetItemInfo(itemData, false);
    }

    private void Empty()
    {
        itemNameText.text = string.Empty;
        itemInfoText.text = string.Empty;
        itemStatsText.text = string.Empty;

        useButton.interactable = false;
        equipButton.interactable = false;
        eraseButton.interactable = false;
        unEquiptButton.interactable = false;
    }

    private void SetArmorInfo(ItemData itemData, bool isItemSlot)
    {
        var armorData = DataTableManager.ArmorTable.Get(itemData.ID);
        itemStatsText.text = string.Format(armorFormat, armorData.defance.ToString(), armorData.moveSpeed.ToString());

        useButton.interactable = false;

        if(isItemSlot)
        {
            equipButton.interactable = true;

        }
        else
        {
            unEquiptButton.interactable = true;
        }
       
    }

    private void SetWeaponInfo(ItemData itemData, bool isItemSlot)
    {
        var weaponData = DataTableManager.WeaponTable.Get(itemData.ID);
        itemStatsText.text = string.Format(weaponFormat, weaponData.AttackPower.ToString(), weaponData.AttackSpeed.ToString());

        useButton.interactable = false;

        if (isItemSlot)
        {
            equipButton.interactable = true;
        }
        else
        { 
            unEquiptButton.interactable = true;
        }
    }

    private void SetConsumableInfo(ItemData itemData, bool isItemSlot)
    {
        // "체력 : {0}\n포만감 : {1}\n수분 : {2}\n피로도 : {3}\n";
        itemStatsText.text = string.Format(consumableFormat, itemData.Value1.ToString(), itemData.Value2.ToString(), itemData.Value3.ToString(), itemData.Value4.ToString());

        useButton.interactable = true;

        if (isItemSlot)
        {
            equipButton.interactable = true;
        }
        else
        {
            unEquiptButton.interactable = true;
        }
    }
}
