using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;

public class StructureTable : DataTable
{
    public class Data
    {
        public int BuildingID { get; set; }
        public int NameID { get; set; }
        public int DescriptID { get; set; }
        public string PrefebName { get; set; }
        public string IconName { get; set; }
        public int BuildingCategory { get; set; }
        public int Rank { get; set; }
        public int BuildingSizeX { get; set; }
        public int BuildingSizeY { get; set; }
        public int BuildingHealth { get; set; }
        public int ItemToProduce { get; set; }
        public int ProductionCycle { get; set; }
        public int AmountPerProduction { get; set; }
        public int MaxStorageCapacity { get; set; }
        public int BoxInventorySlot { get; set; }
        public int UpgradeID { get; set; }
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
            if (!dictionoary.ContainsKey(item.BuildingID))
            {
                dictionoary.Add(item.BuildingID, item);
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.BuildingID}");
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
