using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
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
    public int category { get; set; }

    [field: SerializeField]
    public int isDisposible { get; set; }

    [field: SerializeField]
    public float attack { get; set; }

    [field: SerializeField]
    public float attackSpeed { get; set; }

    [field: SerializeField]
    public float attackRange { get; set; }
    [field: SerializeField]
    public float attackBoxX { get; set; }
    [field: SerializeField]
    public float miningTime { get; set; }
    [field: SerializeField]
    public int effect { get; set; }
    [field: SerializeField]
    public int seOnUse { get; set; }
    [field: SerializeField]
    public int collectableObeject1 { get; set; }
    [field: SerializeField]
    public int collectableObeject2 { get; set; }



    public Sprite ItemImage;
    public string ItemName { get { return itemNameID.ToString(); } }

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
                if (!weaponDataTable.ContainsKey(item.ID))
                {
                    weaponDataTable.Add(item.ID, item);
                    item.ItemImage = (Sprite)(Resources.Load(string.Format(assetIconPath, item.itemIconSpriteID), typeof(Sprite)));
                }
                else
                {
                    Debug.LogError($"Key Duplicated {item.ID}");
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
