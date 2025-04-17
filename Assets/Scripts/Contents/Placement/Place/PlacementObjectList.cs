using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StructureKind
{
    // 1 = 기타 건물, 2 = 농장, 3 = 포탑, 4 = 제작, 5 = 보관
    Other = 1,
    Farm,
    Turret,
    Create,
    Store,
}

public class PlacementObjectList
{
    private readonly string prefabPathFormat = "Prefabs/Placement/Structure/{0}/{1}";
    private readonly string spritePathFormat = "Sprites/Placement/{0}/{1}";

    public List<PlacementObjectInfo> objects = new List<PlacementObjectInfo>();

    public void SetObjects()
    {
        SetStructureObject();
    }

    private void SetStructureObject()
    {
        var allData = DataTableManager.StructureTable.GetAll();
        foreach (var data in allData)
        {
            PlacementObjectInfo objInfo = new PlacementObjectInfo();
            objInfo.ID = data.Value.BuildingID;
            var construction = DataTableManager.ConstructionTable.Get(data.Value.UpgradeID);
            objInfo.NextStructureID = construction.ResultBuildingID;

            objInfo.Rank = data.Value.Rank;
            objInfo.SubType = data.Value.BuildingSubType;
            objInfo.Size = new Vector2Int(data.Value.BuildingSizeX, data.Value.BuildingSizeY);

            objInfo.Kind = (StructureKind)data.Value.BuildingCategory;
            objInfo.Name = data.Value.NameID.ToString();
            objInfo.DefaultHp = data.Value.BuildingHealth;
            string prefab = data.Value.PrefebName;

            objInfo.Prefeb = Resources.Load<GameObject>
                (string.Format(prefabPathFormat, objInfo.Kind.ToString(), prefab));

            var table = objInfo.Prefeb.transform.GetChild(0).GetComponent<StructureStats>().CurrentStatTable;
            table.Clear();
            table.Add(StatType.HP, new StatValue(StatType.HP, objInfo.DefaultHp));

            foreach (var needItem in construction.NeedItemList)
            {
                objInfo.NeedItems.Add(needItem.Key, needItem.Value);
            }


            objInfo.Icon = Resources.Load<Sprite>
                (string.Format(spritePathFormat, objInfo.Kind.ToString(), data.Value.IconName));
            objInfo.Feature = data.Value.DescriptID.ToString();

            objects.Add(objInfo);
        }
    }
    
}
