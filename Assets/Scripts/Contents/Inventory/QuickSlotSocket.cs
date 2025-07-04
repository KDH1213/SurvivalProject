using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuickSlotSocket : EquipmentSocket
{
    [SerializeField]
    private QuickSlotButtonView quickSlotButtonView;

    public UnityEvent<int> onAmountEvent;
    public UnityEvent<ItemData> onUseItemEvent;

    public override void InitializeSocket(EquipmentType equipmentType, ItemData itemData, int amount, int durability)
    {
        base.InitializeSocket(equipmentType, itemData, amount, durability);

        var player = GameObject.FindWithTag(Tags.Player);
        onUseItemEvent.AddListener(player.GetComponent<PlayerStats>().OnUseItem);
        onAmountEvent.AddListener(quickSlotButtonView. OnSetAmount);

        player.GetComponent<PlayerFSM>().PlayerInventory.addQuickSlotItemFunc += IsAddItem;
    }

    public override void OnEquipment(EquipmentType equipmentType, ItemData itemData, int amount, int durability)
    {
        base.OnEquipment(equipmentType, itemData, amount, durability);

        quickSlotButtonView.OnSetItemInfo();
        onAmountEvent?.Invoke(amount);
    }

    public override void OnUnEquipment()
    {
        OnEmpty();
        quickSlotButtonView.OnSetItemInfo();
    }

    public void OnUseItem()
    {
        if(ItemData == null)
        {
            return;
        }

        --Amount;
        amountText.text = Amount.ToString();
        onUseItemEvent?.Invoke(ItemData);
        onAmountEvent?.Invoke(Amount);

        if(Amount <= 0)
        {
            OnEmpty();
            quickSlotButtonView.OnSetItemInfo();
        }
    }

    public int IsAddItem(ItemData addItemData, int addAmount)
    {
        if (ItemInfo != null && ItemInfo.itemData != null 
            && ItemInfo.itemData.ID == addItemData.ID)
        {
            int maxStack = ItemInfo.itemData.MaxStack;

            ItemInfo.Amount += addAmount;
            int amountRemaining = ItemInfo.Amount - maxStack;

            if(amountRemaining > 0)
            {
                ItemInfo.Amount = maxStack;
                onAmountEvent?.Invoke(ItemInfo.Amount);
                amountText.text = Amount.ToString();
                return amountRemaining;
            }
            else
            {
                onAmountEvent?.Invoke(ItemInfo.Amount);
                amountText.text = Amount.ToString();
                return 0;
            }
        }

        return addAmount;
    }

}
