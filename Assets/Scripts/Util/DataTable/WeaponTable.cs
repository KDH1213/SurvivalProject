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
    public string Prefab { get; set; }
    [field: SerializeField]
    public float AttackPower { get; set; }

    [field: SerializeField]
    public float AttackSpeed { get; set; }

    [field: SerializeField]
    public float AttackRadius { get; set; }
    [field: SerializeField]
    public float Durability { get; set; }
    [field: SerializeField]
    public float GatherType { get; set; }
    [field: SerializeField]
    public int AttackEffect { get; set; }
    [field: SerializeField]
    public int AttackSFX { get; set; }


    public Sprite ItemImage;
    public string ItemName { get { return ItemID.ToString(); } }

    public class WeaponTable : DataTable
    {
        private Dictionary<int, WeaponData> weaponDataTable = new Dictionary<int, WeaponData>();
        private readonly string assetIconPath = "UI/Icon/{0}";
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
                    // item.ItemImage = (Sprite)(Resources.Load(string.Format(assetIconPath, item.itemIconSpriteID), typeof(Sprite)));
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
}
