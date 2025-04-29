using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DropItemInfo
{
    public int id;
    public int amount;

    public ItemData itemData;
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

    public List<DropItemInfo> GetDropItem()
    {
        var dropItemList = new List<DropItemInfo>();
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