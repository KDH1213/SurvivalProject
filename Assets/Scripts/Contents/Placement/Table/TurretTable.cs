using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretTable : DataTable
{
    public class Data
    {
        public int buildingID { get; set; }
        public int buildingNameID { get; set; }
        public string buildingIcon { get; set; }
        public int buildingPrefebID { get; set; }
        public int category { get; set; }
        public int buildingRecipeID { get; set; }
        public int buildingLevel { get; set; }
        public int turretAtkPower { get; set; }
        public float turretAtkSpeed { get; set; }
        public float turretAtkRange { get; set; }
        public int seOnDestroy { get; set; }
        public int geOnDestroy { get; set; }
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
                Debug.Log(item.ToString());
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
