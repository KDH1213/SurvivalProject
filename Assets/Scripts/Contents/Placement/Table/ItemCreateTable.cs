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
        public int ResultID { get; set; }
        public int ResultValue { get; set; }
        public int DropExp { get; set; }
        public int PlusFatigue { get; set; }


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
                dictionoary.Add(item.ResultID, item);
                Debug.Log(item.ToString());
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
