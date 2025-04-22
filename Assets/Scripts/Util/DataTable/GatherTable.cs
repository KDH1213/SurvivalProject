using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;


[System.Serializable]
public class GatherData
{
    [field: SerializeField]
    public int ID { get; set; }

    [field: SerializeField]
    public string PrefabName { get; set; }

    [field: SerializeField]
    public int ResourceType { get; set; }

    [field: SerializeField]
    public int GatheringTime { get; set; }

   

    [field: SerializeField]
    public int MinusSatiation { get; set; }

    [field: SerializeField]
    public int MinusHydration { get; set; }

    [field: SerializeField]
    public int PlusFatigue { get; set; }

    [field: SerializeField]
    public int DropID { get; set; }

    [field: SerializeField]
    public float RespawnTime { get; set; }

    public GatherType GatherType;
    public GameObject Prefab;
    public List<ActInfo> actInfoList = new List<ActInfo>();
    public float InteractTime;

    public void Initialize()
    {
        CreateActInfo();

        GatherType = (GatherType)ResourceType;
        InteractTime = GatheringTime * 0.01f;
    }

    private void CreateActInfo()
    {
        actInfoList.Add(new ActInfo(SurvivalStatType.Fatigue, PlusFatigue));
        actInfoList.Add(new ActInfo(SurvivalStatType.Hunger, MinusSatiation));
        actInfoList.Add(new ActInfo(SurvivalStatType.Thirst, MinusHydration));
    }
}

public class GatherTable : DataTable
{
    private Dictionary<int, GatherData> gatherDataTable = new Dictionary<int, GatherData>();
    private readonly string assetPath = "Prefabs/Nature/{0}/{1}";

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var itemDataList = LoadCSV<GatherData>(textAsset.text);

        gatherDataTable.Clear();

        foreach (var item in itemDataList)
        {
            if (!gatherDataTable.ContainsKey(item.ID))
            {
                gatherDataTable.Add(item.ID, item);
                item.Initialize();

                item.Prefab = (GameObject)(Resources.Load(string.Format(assetPath, item.GatherType.ToString(), item.PrefabName), typeof(GameObject)));

                if (item.Prefab is not null)
                {
                    item.Prefab.GetComponent<Gather>().SetGatherData(item);
                }
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.ID}");
            }
        }
    }
    public GatherData Get(int key)
    {
        if (!gatherDataTable.ContainsKey(key))
        {
            return default;
        }

        return gatherDataTable[key];
    }
}
