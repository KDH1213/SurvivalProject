using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ArmorData
{

    [field: SerializeField]
    public int ID { get; set; }
    [field: SerializeField]
    public int itemNameID { get; set; }
    [field: SerializeField]
    public int itemDescriptionID { get; set; }
    [field: SerializeField]
    public int prefabID { get; set; }
    [field: SerializeField]
    public string itemIconSpriteID { get; set; }

    [field: SerializeField]
    public ArmorType ArmorType { get; set; }

    [field: SerializeField]
    public int isDisposible { get; set; }

    [field: SerializeField]
    public float defance { get; set; }

    [field: SerializeField]
    public float moveSpeed { get; set; }

    [field: SerializeField]
    public int ColdResistance { get; set; }

    [field: SerializeField]
    public int HeatResistance { get; set; }

    public Sprite ItemImage;
    public string ItemName { get { return DataTableManager.StringTable.Get(itemNameID); } }
}
public class ArmorTable : DataTable
{
    private Dictionary<int, ArmorData> armorDataTable = new Dictionary<int, ArmorData>();
    private readonly string assetIconPath = "UI/Icon/{0}";
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var itemDataList = LoadCSV<ArmorData>(textAsset.text);

        armorDataTable.Clear();

        foreach (var item in itemDataList)
        {
            if (!armorDataTable.ContainsKey(item.ID))
            {
                armorDataTable.Add(item.ID, item);
                item.ItemImage = (Sprite)(Resources.Load(string.Format(assetIconPath, item.itemIconSpriteID), typeof(Sprite)));
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.ID}");
            }
        }
    }
    public ArmorData Get(int key)
    {
        if (!armorDataTable.ContainsKey(key))
        {
            return default;
        }

        return armorDataTable[key];
    }
}
