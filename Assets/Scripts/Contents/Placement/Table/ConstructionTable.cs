using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTable : DataTable
{
    
    public class Data
    {
        public int UpgradeID { get; set; }
        public int ResultBuildingID { get; set; }
        public int BuildCostItem1 { get; set; }
        public int CostItemValue1 { get; set; }
        public int BuildCostItem2 { get; set; }
        public int CostItemValue2 { get; set; }
        public int BuildCostItem3 { get; set; }
        public int CostItemValue3 { get; set; }
        public int BuildCostItem4 { get; set; }
        public int CostItemValue4 { get; set; }
        public int MinusSatiation { get; set; }
        public int MinusHydration { get; set; }
        public int PlusFatigue { get; set; }
        public float DropExp { get; set; }

        public Dictionary<int, int> NeedItems { get; set; } = new Dictionary<int, int>();
        public Dictionary<SurvivalStatType, int> NeedPenalties { get; set; } = new Dictionary<SurvivalStatType, int>();
    }

    private Dictionary<int, Data> dictionoary = new Dictionary<int, Data>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<Data>(textAsset.text);

        dictionoary.Clear();

        foreach (var item in list)
        {
            if (!dictionoary.ContainsKey(item.UpgradeID))
            {
                if (item.BuildCostItem1 != 0)
                {
                    item.NeedItems.Add(item.BuildCostItem1, item.CostItemValue1);
                }
                if (item.BuildCostItem2 != 0)
                {
                    item.NeedItems.Add(item.BuildCostItem2, item.CostItemValue2);
                }
                if (item.BuildCostItem3 != 0)
                {
                    item.NeedItems.Add(item.BuildCostItem3, item.CostItemValue3);
                }
                if (item.BuildCostItem4 != 0)
                {
                    item.NeedItems.Add(item.BuildCostItem4, item.CostItemValue4);
                }
                item.NeedPenalties.Add(SurvivalStatType.Hunger, item.MinusSatiation);
                item.NeedPenalties.Add(SurvivalStatType.Thirst, item.MinusHydration);
                item.NeedPenalties.Add(SurvivalStatType.Fatigue, item.PlusFatigue);
                dictionoary.Add(item.UpgradeID, item);
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.UpgradeID}");
            }
        }
        

    }

    public Data Get(int key)
    {
        if (!dictionoary.ContainsKey(key))
        {
            Debug.LogError($"{key} None");
            return null;
        }

        return dictionoary[key];
    }

    public Dictionary<int, Data> GetAll()
    {
        return dictionoary;
    }
}
