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

    public override void InitializeSocket(EquipmentType equipmentType, ItemData itemData, int amount)
    {
        base.InitializeSocket(equipmentType, itemData, amount);

        var player = GameObject.FindWithTag(Tags.Player);
        onUseItemEvent.AddListener(player.GetComponent<PlayerStats>().OnUseItem);
        onAmountEvent.AddListener(quickSlotButtonView.OnSetAmount);
    }

    public override void OnEquipment(EquipmentType equipmentType, ItemData itemData, int amount)
    {
        base.OnEquipment(equipmentType, itemData, amount);

        quickSlotButtonView.OnSetItemInfo();
        onAmountEvent?.Invoke(amount);
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

        if(Amount == 0)
        {
            OnEmpty();
            quickSlotButtonView.OnSetItemInfo();
        }

    }
}
