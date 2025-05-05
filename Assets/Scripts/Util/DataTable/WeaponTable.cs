using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    [field: SerializeField]
    public int ItemID { get; set; }
    [field: SerializeField]
    public int WeaponType { get; set; }
    [field: SerializeField]
    public string PrefabName { get; set; }
    [field: SerializeField]
    public float AttackPower { get; set; }

    [field: SerializeField]
    public float AttackSpeed { get; set; }

    [field: SerializeField]
    public float AttackRadius { get; set; }
    [field: SerializeField]
    public int Durability { get; set; }
    [field: SerializeField]
    public float GatherType { get; set; }
    [field: SerializeField]
    public int AttackEffect { get; set; }
    [field: SerializeField]
    public int AttackSFX { get; set; }

    public GameObject WeaponPrefab;
}
public class WeaponTable : DataTable
{
    private Dictionary<int, WeaponData> weaponDataTable = new Dictionary<int, WeaponData>();
    private readonly string assetWeaponPath = "Prefabs/Weapons/{0}";
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var itemDataList = LoadCSV<WeaponData>(textAsset.text);

        weaponDataTable.Clear();

        foreach (var item in itemDataList)
        {
            if (!weaponDataTable.ContainsKey(item.ItemID))
            {
                weaponDataTable.Add(item.ItemID, item);
                item.WeaponPrefab = (GameObject)(Resources.Load(string.Format(assetWeaponPath, item.PrefabName), typeof(GameObject)));
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.ItemID}");
            }
        }
    }
    public WeaponData Get(int key)
    {
        if (!weaponDataTable.ContainsKey(key))
        {
            return default;
        }

        return weaponDataTable[key];
    }
}

