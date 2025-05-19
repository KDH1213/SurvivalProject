
public enum ItemUseEffectType
{
    None,
    Hp,
    Hunger,
    Thirst,
    Fatigue,
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