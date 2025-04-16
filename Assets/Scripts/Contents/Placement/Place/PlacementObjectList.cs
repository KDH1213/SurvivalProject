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
            objInfo.NextStructureID = data.Value.UpgradeID;
            var construction = DataTableManager.ConstructionTable.Get(objInfo.ID);

            objInfo.MaxBuildCount = Random.Range(1, 11);
            objInfo.Size = new Vector2Int(data.Value.BuildingSizeX, data.Value.BuildingSizeY);

            objInfo.Kind = (StructureKind)data.Value.BuildingCategory;
            objInfo.Name = data.Value.NameID.ToString();
            objInfo.DefaultHp = construction.buildingHP;
            string prefab = data.Value.PrefebName;

            objInfo.Prefeb = Resources.Load<GameObject>
                (string.Format(prefabPathFormat, objInfo.Kind.ToString(), prefab));

            var table = objInfo.Prefeb.transform.GetChild(0).GetComponent<StructureStats>().CurrentStatTable;
            table.Clear();
            table.Add(StatType.HP, new StatValue(StatType.HP, objInfo.DefaultHp));

            string[] needItemKeys = construction.buildCostID.Split('_');
            string[] needItemValues = construction.buildCostValue.Split('_');
            for (int i = 0; i < needItemKeys.Length; i++)
            {
                objInfo.NeedItems.Add(int.Parse(needItemKeys[i]), int.Parse(needItemValues[i]));
            }


            objInfo.Icon = Resources.Load<Sprite>
                (string.Format(spritePathFormat, objInfo.Kind.ToString(), data.Value.IconName));
            objInfo.Feature = data.Value.DescriptID.ToString();

            objects.Add(objInfo);
        }
    }
    
}
