
public class ItemInfo
{
    public ItemData itemData;
    public int index;
    public int Amount { get; set; }

    public void Empty()
    {
        itemData = null;
        Amount = 0;
    }

    public void OnSwapItemInfo(ItemInfo itemInfo)
    {
        (this.itemData, itemInfo.itemData) = (itemInfo.itemData, this.itemData);
        (this.Amount, itemInfo.Amount) = (itemInfo.Amount, this.Amount);
    }
}

public struct ItemInfoSaveData
{
    public int itemID;
    public int amount;
    public int index;
}