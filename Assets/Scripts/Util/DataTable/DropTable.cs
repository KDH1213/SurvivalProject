using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DropItemInfo
{
    public int id;
    public int amount;
    public int durability;

    public ItemData itemData;

    public DropItemInfo(ItemData itemData, int id, int amount, int durability)
    {
        this.itemData = itemData;
        this.id = id;
        this.amount = amount;
        this.durability = durability;
    }

    public DropItemInfo(ItemInfo itemInfo)
    {
        this.itemData = itemInfo.itemData;
        this.id = itemInfo.itemData == null ? 0 : itemInfo.itemData.ID;
        this.amount = itemInfo.Amount;
        this.durability = itemInfo.Durability;
    }
}

public struct DropInfo
{
    public int itemID;
    public int itemDropRate;
    public int itemMinCount;
    public int itemMaxCount;

    public DropInfo(int itemID, int itemDropRate, int itemMinCount, int itemMaxCount)
    {
        this.itemID = itemID;
        this.itemDropRate = itemDropRate;
        this.itemMinCount = itemMinCount;
        this.itemMaxCount = itemMaxCount;
    }
}

[System.Serializable]
public class DropData
{
    [field: SerializeField]
    public int DropID { get; set; }
    [field: SerializeField]
    public int Item1ID { get; set; }
    [field: SerializeField]
    public int Item1DropRate { get; set; }
    [field: SerializeField]
    public int Item1Minimum { get; set; }
    [field: SerializeField]
    public int Item1Maximum { get; set; }

    [field: SerializeField]
    public int Item2ID { get; set; }
    [field: SerializeField]
    public int Item2DropRate { get; set; }
    [field: SerializeField]
    public int Item2Minimum { get; set; }
    [field: SerializeField]
    public int Item2Maximum { get; set; }

    [field: SerializeField]
    public int Item3ID { get; set; }
    [field: SerializeField]
    public int Item3DropRate { get; set; }
    [field: SerializeField]
    public int Item3Minimum { get; set; }
    [field: SerializeField]
    public int Item3Maximum { get; set; }

    [field: SerializeField]
    public int Item4ID { get; set; }
    [field: SerializeField]
    public int Item4DropRate { get; set; }
    [field: SerializeField]
    public int Item4Minimum { get; set; }
    [field: SerializeField]
    public int Item4Maximum { get; set; }

    public List<DropItemInfo> DropItemInfoList = new List<DropItemInfo>();
    private List<DropInfo> dropInfoList = new List<DropInfo>();

    public void Initialize()
    {
        dropInfoList.Clear();

        if(Item1ID == 0)
        {
            return;
        }

        dropInfoList.Add(new DropInfo(Item1ID, Item1DropRate, Item1Minimum, Item1Maximum));

        if (Item2ID == 0)
        {
            return;
        }

        dropInfoList.Add(new DropInfo(Item2ID, Item2DropRate, Item2Minimum, Item2Maximum));

        if (Item3ID == 0)
        {
            return;
        }

        dropInfoList.Add(new DropInfo(Item3ID, Item3DropRate, Item3Minimum, Item3Maximum));
        if (Item4ID == 0)
        {
            return;
        }

        dropInfoList.Add(new DropInfo(Item4ID, Item4DropRate, Item4Minimum, Item4Maximum));

    }

    public List<DropItemInfo> GetDropItemList()
    {
        var dropItemList = new List<DropItemInfo>();

        for (int i = 0; i < dropInfoList.Count; ++i)
        {
            bool isGetItem = false;
            if(dropInfoList[i].itemDropRate == 100)
            {
                isGetItem = true;
            }
            else
            {
                var randomValue = Random.value * 100f;
                if(randomValue < dropInfoList[i].itemDropRate)
                {
                    isGetItem = true;
                }
            }

            if(!isGetItem)
            {
                continue;
            }

            var getItemCount = Random.Range(dropInfoList[i].itemMinCount, dropInfoList[i].itemMaxCount);

            if(getItemCount != 0)
            {
                var itemData = DataTableManager.ItemTable.Get(dropInfoList[i].itemID);
                dropItemList.Add(new DropItemInfo(itemData, dropInfoList[i].itemID, getItemCount, itemData.Durability));
            }
        }

        return dropItemList;
    }
}

public class DropTable : DataTable
{
    private Dictionary<int, DropData> dropDataTable = new Dictionary<int, DropData>();
    public Dictionary<int, DropData> DropDataTable { get { return dropDataTable; } }

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var dropDataList = LoadCSV<DropData>(textAsset.text);

        dropDataTable.Clear();

        foreach (var dropData in dropDataList)
        {
            if (!dropDataTable.ContainsKey(dropData.DropID))
            {
                dropDataTable.Add(dropData.DropID, dropData);
                dropData.Initialize();
            }
            else
            {
                Debug.LogError($"Key Duplicated {dropData.DropID}");
            }
        }
    }
    public DropData Get(int key)
    {
        if (!dropDataTable.TryGetValue(key, out var dropData))
        {
            return default;
        }

        return dropData;
    }
}