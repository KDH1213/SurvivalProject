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
    public ItemType ItemType { get; set; }
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
    public string ItemName { get { return NameID.ToString(); } }

    public List<StatInfo> GetItemInfoList()
    {
        var list = new List<StatInfo>();

        if(ItemType == ItemType.Weapon)
        {
            var weaponData = DataTableManager.WeaponTable.Get(ID);

            list.Add(new StatInfo(StatType.BasicAttackPower, weaponData.attack));
            list.Add(new StatInfo(StatType.AttackSpeed, weaponData.attackSpeed));
        }
        else if (ItemType == ItemType.Armor)
        {
            var armorData = DataTableManager.ArmorTable.Get(ID);

            list.Add(new StatInfo(StatType.Defense, armorData.defance));
            list.Add(new StatInfo(StatType.MovementSpeed, armorData.moveSpeed));
        }
        return list;
    }

    public static EquipmentType ConvertEquipmentType(ItemType itemType)
    {


        return EquipmentType.None;
    }
}

public class ItemTable : DataTable
{
    private Dictionary<int, ItemData> itemDataTable = new Dictionary<int, ItemData>();
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
                item.ItemImage = (Sprite)(Resources.Load(string.Format(assetIconPath, item.IconName), typeof(Sprite)));
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