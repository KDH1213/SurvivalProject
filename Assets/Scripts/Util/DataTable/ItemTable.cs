using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemData
{
    [field: SerializeField]
    public int ID { get; set; }
    [field: SerializeField]
    public int itemNameID { get; set; }
    [field: SerializeField]
    public int itemDescID { get; set; }
    [field: SerializeField]
    public string itemIconSpriteID { get; set; }
    [field: SerializeField]
    public ItemType ItemType { get; set; }
    [field: SerializeField]
    public int MaxAmount { get; set; }
    [field: SerializeField]
    public int isDisposible { get; set; }
    [field: SerializeField]
    public int itemCooltime { get; set; }
    [field: SerializeField]
    public string targetStat { get; set; }
    [field: SerializeField]
    public string value { get; set; }
    [field: SerializeField]
    public int seOnUse { get; set; }
    [field: SerializeField]
    public float lifeExpGain { get; set; }



    public Sprite ItemImage;
    public string ItemName { get { return itemNameID.ToString(); } }

    public List<StatInfo> GetItemInfoList()
    {
        var list = new List<StatInfo>();

        if(ItemType == ItemType.Weapon)
        {
            var weaponData = DataTableManager.WeaponTable.Get(ID);

            list.Add(new StatInfo(StatType.BasicAttackPower, weaponData.attack));
            list.Add(new StatInfo(StatType.AttackSpeed, weaponData.attackSpeed));
        }
        else if (ItemType >= ItemType.Helmet && ItemType <= ItemType.Shoes)
        {
            var armorData = DataTableManager.ArmorTable.Get(ID);

            list.Add(new StatInfo(StatType.Defense, armorData.defance));
            list.Add(new StatInfo(StatType.MovementSpeed, armorData.moveSpeed));
        }
        return list;
    }
}

public class ItemTable : DataTable
{
    private Dictionary<int, ItemData> itemDataTable = new Dictionary<int, ItemData>();
    private readonly string assetIconPath = "UI/Icon/{0}";
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
                item.ItemImage = (Sprite)(Resources.Load(string.Format(assetIconPath, item.itemIconSpriteID), typeof(Sprite)));
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