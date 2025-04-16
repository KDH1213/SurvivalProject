
public enum ItemUseEffectType
{
    None,
    Hp,
    Fatigue,
    Hunger,
    Thirst,
}

public struct ItemUseEffectInfo
{
    public ItemUseEffectType itemUseEffectType;
    public int value;

    public ItemUseEffectInfo(ItemUseEffectType itemUseEffectType, int value)
    {
        this.itemUseEffectType = itemUseEffectType;
        this.value = value; 
    }
}