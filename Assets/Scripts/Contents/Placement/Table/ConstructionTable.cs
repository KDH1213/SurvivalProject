using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTable : DataTable
{
    
    public class Data
    {
        public int buildingRecipeID { get; set; }
        public int buildingRecipeNameID { get; set; }
        public string buildingRecipeIcon { get; set; }
        public int category { get; set; }
        public int buildingID { get; set; }
        public float buildingHP { get; set; }
        public string buildCostID { get; set; }
        public string buildCostValue { get; set; }
        public string lengthXY { get; set; }
        public string geOnBuild { get; set; }
        public string seOnBuild { get; set; }
        public string buildFeedback { get; set; }
        public int buildResultExp { get; set; }

        public override string ToString()
        {
            return $@"{buildingRecipeID} / {buildingRecipeNameID} / {buildingRecipeIcon} / {category} / 
                      {buildingID} / {buildingHP} / {buildCostID} / {buildCostValue} /
                    {lengthXY} /{geOnBuild} /{seOnBuild} /{buildFeedback} /{buildResultExp}";
        }
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
            if (!dictionoary.ContainsKey(item.buildingID))
            {
                dictionoary.Add(item.buildingID, item);
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.buildingID}");
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
