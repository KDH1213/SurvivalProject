using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreateTable : DataTable
{
    public class Data
    {
        public int RecipeID { get; set; }
        public int NameID { get; set; }
        public int DescriptID { get; set; }
        public string IconName { get; set; }
        public int Rank { get; set; }
        public int RecipeCategory { get; set; }
        public int ItemCostID1 { get; set; }
        public int ItemCostValue1 { get; set; }
        public int ItemCostID2 { get; set; }
        public int ItemCostValue2 { get; set; }
        public int ItemCostID3 { get; set; }
        public int ItemCostValue3 { get; set; }
        public int ItemCostID4 { get; set; }
        public int ItemCostValue4 { get; set; }
        public int ResultID { get; set; }
        public int ResultValue { get; set; }
        public int DropExp { get; set; }
        public int PlusFatigue { get; set; }

        public Dictionary<int, int> NeedItemList { get; set; } = new Dictionary<int, int>();
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
            if (!dictionoary.ContainsKey(item.ResultID))
            {
                if(item.ItemCostID1 != 0)
                {
                    item.NeedItemList.Add(item.ItemCostID1, item.ItemCostValue1);
                }
                if (item.ItemCostID2 != 0)
                {
                    item.NeedItemList.Add(item.ItemCostID2, item.ItemCostValue2);
                }
                if (item.ItemCostID3 != 0)
                {
                    item.NeedItemList.Add(item.ItemCostID3, item.ItemCostValue3);
                }
                if (item.ItemCostID4 != 0)
                {
                    item.NeedItemList.Add(item.ItemCostID4, item.ItemCostValue4);
                }
                dictionoary.Add(item.ResultID, item);
                
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.ResultID}");
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
