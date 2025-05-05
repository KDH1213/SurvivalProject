
public class ItemSlotInfo
{
    public ItemData itemData;
    public int index;
    public int Amount { get; set; }
    public int Durability { get; set; }

    public System.Action<int> onUseDurabilityAction;

    public void Empty()
    {
        itemData = null;
        Amount = 0;
    }

    public void OnSwapItemInfo(ItemSlotInfo itemInfo)
    {
        (this.itemData, itemInfo.itemData) = (itemInfo.itemData, this.itemData);
        (this.Amount, itemInfo.Amount) = (itemInfo.Amount, this.Amount);
        (this.Durability, itemInfo.Durability) = (itemInfo.Durability, this.Durability);
    }

    public void OnUseDurability()
    {
        Durability -= 1;
        onUseDurabilityAction?.Invoke(index);
    }
}

[System.Serializable]
public class ItemInfo
{
    public ItemData itemData;
    public int Amount { get; set; }
    public int Durability { get; set; }

}

public struct ItemInfoSaveData
{
    public int itemID;
    public int amount;
    public int index;
    public int durability;
}

public struct EquipmentItemInfoSaveData
{
    public int id;
    public int durability;

    public EquipmentItemInfoSaveData(int id, int durability)
    {
        this.id = id;
        this.durability = durability;
    }
}