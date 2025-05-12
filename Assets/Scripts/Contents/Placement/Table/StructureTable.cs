using System.Collections.Generic;
using System.Linq;
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
        public int PlaceBuildingID { get; set; }
        public int BuildingSubType { get; set; }
        public int ReturnedItem1 { get; set; }
        public int ReturnedItemCount1 { get; set; }
        public int ReturnedItem2 { get; set; }
        public int ReturnedItemCount2 { get; set; }
        public int ReturnedItem3 { get; set; }
        public int ReturnedItemCount3 { get; set; }
        public int ReturnedItem4 { get; set; }
        public int ReturnedItemCount4 { get; set; }
        public int NextUpgradeID { get; set; }
        public string StageSpecifier { get; set; } 
        public int FatigueReductionPerMinute { get; set; }

        public Dictionary<int, int> ReturnItemList { get; set; } = new Dictionary<int, int>();

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
                var stages = item.StageSpecifier.Split("_");
                if (!stages.Contains(SaveLoadManager.Data.sceneStage.ToString()))
                {
                    continue;
                }

                if (item.ReturnedItem1 != 0)
                {
                    item.ReturnItemList.Add(item.ReturnedItem1, item.ReturnedItemCount1);
                }
                if (item.ReturnedItem2 != 0)
                {
                    item.ReturnItemList.Add(item.ReturnedItem2, item.ReturnedItemCount2);
                }
                if (item.ReturnedItem3 != 0)
                {
                    item.ReturnItemList.Add(item.ReturnedItem3, item.ReturnedItemCount3);
                }
                if (item.ReturnedItem4 != 0)
                {
                    item.ReturnItemList.Add(item.ReturnedItem4, item.ReturnedItemCount4);
                }

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
