using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ArmorData
{

    [field: SerializeField]
    public int ItemID { get; set; }
    [field: SerializeField]
    public string PrefabName { get; set; }
    [field: SerializeField]
    public int EquipType { get; set; }

    [field: SerializeField]
    public float DefensePower { get; set; }

    [field: SerializeField]
    public float MovementSpeed { get; set; }

    [field: SerializeField]
    public int Durability { get; set; }

    [field: SerializeField]
    public int ColdResistance { get; set; }

    [field: SerializeField]
    public int HeatResistance { get; set; }


    public ArmorType ArmorType => (ArmorType)EquipType;
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
            if (!armorDataTable.ContainsKey(item.ItemID))
            {
                armorDataTable.Add(item.ItemID, item);
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.ItemID}");
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
