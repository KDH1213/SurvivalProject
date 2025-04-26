using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemData
{
    [field: SerializeField]
    public int ID { get; set; }
    [field: SerializeField]
    public int NameID { get; set; }
    [field: SerializeField]
    public int DescriptID { get; set; }
    [field: SerializeField]
    public string IconName { get; set; }
    [field: SerializeField]
    public int ItemCategory { get; set; }
    [field: SerializeField]
    public int MaxStack { get; set; }
    [field: SerializeField]
    public int IsDisposible { get; set; }

    [field: SerializeField]
    public int TargetStat1 { get; set; }

    [field: SerializeField]
    public int Value1 { get; set; }

    [field: SerializeField]
    public int TargetStat2 { get; set; }

    [field: SerializeField]
    public int Value2 { get; set; }

    [field: SerializeField]
    public int TargetStat3 { get; set; }

    [field: SerializeField]
    public int Value3 { get; set; }

    [field: SerializeField]
    public int TargetStat4 { get; set; }

    [field: SerializeField]
    public int Value4 { get; set; }
    

    public Sprite ItemImage;
    public ItemType ItemType;
    public string ItemName { get { return DataTableManager.StringTable.Get(NameID); } }

    private List<ItemUseEffectInfo> itemUseEffectInfoList;
    public List<ItemUseEffectInfo> ItemUseEffectInfoList { get { return itemUseEffectInfoList; } }

    public List<StatInfo> GetItemInfoList()
    {
        var list = new List<StatInfo>();

        if(ItemType == ItemType.Weapon)
        {
            var weaponData = DataTableManager.WeaponTable.Get(ID);

            list.Add(new StatInfo(StatType.BasicAttackPower, weaponData.AttackPower));
            list.Add(new StatInfo(StatType.AttackSpeed, weaponData.AttackSpeed));
        }
        else if (ItemType == ItemType.Armor)
        {
            var armorData = DataTableManager.ArmorTable.Get(ID);

            list.Add(new StatInfo(StatType.Defense, armorData.defance));
            list.Add(new StatInfo(StatType.MovementSpeed, armorData.moveSpeed));
        }
        return list;
    }

    public static EquipmentType ConvertEquipmentType(int id, ItemType itemType)
    {
        if(itemType == ItemType.Armor)
        {
            return (EquipmentType)DataTableManager.ArmorTable.Get(id).ArmorType - 1;
        }
        else if(itemType == ItemType.Weapon)
        {
            return EquipmentType.Weapon;
        }
        else if (itemType == ItemType.Consumable)
        {
            return EquipmentType.Consumable;
        }    

        return EquipmentType.None;
    }

    public void CreateItemUseEffectInfo()
    {
        if(TargetStat1 == 0)
        {
            return;
        }
        itemUseEffectInfoList = new List<ItemUseEffectInfo>();
        itemUseEffectInfoList.Add(new ItemUseEffectInfo((ItemUseEffectType)TargetStat1, Value1));

        if (TargetStat2 == 0)
        {
            return;
        }

        itemUseEffectInfoList.Add(new ItemUseEffectInfo((ItemUseEffectType)TargetStat2, Value2));

        if (TargetStat3 == 0)
        {
            return;
        }

        itemUseEffectInfoList.Add(new ItemUseEffectInfo((ItemUseEffectType)TargetStat3, Value3));

        if (TargetStat4 == 0)
        {
            return;
        }

        itemUseEffectInfoList.Add(new ItemUseEffectInfo((ItemUseEffectType)TargetStat4, Value4));
    }
}

public class ItemTable : DataTable
{
    private Dictionary<int, ItemData> itemDataTable = new Dictionary<int, ItemData>();
    public Dictionary<int, ItemData> ItemDataTable { get { return itemDataTable; } }

    private readonly string assetIconPath = "Sprites/UI/Icons/{0}";
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var itemDataList = LoadCSV<ItemData>(textAsset.text);

        itemDataTable.Clear();

        foreach (var item in itemDataList)
        {
            if (!itemDataTable.ContainsKey(item.ID))
            {
                itemDataTable.Add(item.ID, item);
                item.ItemType = (ItemType)item.ItemCategory;
                item.ItemImage = (Sprite)(Resources.Load(string.Format(assetIconPath, item.IconName), typeof(Sprite)));
                item.CreateItemUseEffectInfo();
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.ID}");
            }
        }
    }
    public ItemData Get(int key)
    {
        if (!itemDataTable.ContainsKey(key))
        {
            return default;
        }

        return itemDataTable[key];
    }
}