using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StructureKind
{
    Farm = 1,
    Turret,
    Other

}

public class PlacementObjectList
{
    private readonly string prefabPathFormat = "Prefabs/Placement/Structure/{0}/{1}";
    private readonly string spritePathFormat = "Sprites/Placement/{0}/{1}";

    public List<PlacementObjectInfo> objects = new List<PlacementObjectInfo>();

    public void SetObjects()
    {
        SetFarmObject();
        SetTurretObject();
        SetOtherObject();
    }

    private void SetFarmObject()
    {
        var allData = DataTableManager.FarmTable.GetAll();
        foreach (var data in allData)
        {
            PlacementObjectInfo objInfo = new PlacementObjectInfo();
            objInfo.ID = data.Value.buildingID;
            var construction = DataTableManager.ConstructionTable.Get(objInfo.ID);

            objInfo.MaxBuildCount = Random.Range(1, 11);
            string[] xy = construction.lengthXY.Split('_');
            if (int.TryParse(xy[0], out int x) && int.TryParse(xy[1], out int y))
            {
                objInfo.Size = new Vector2Int(x, y);
            }
            else
            {
                Debug.LogError("肋给等 单捞磐!");
            }

            objInfo.Kind = (StructureKind)construction.category;
            objInfo.Name = data.Value.buildingNameID.ToString();
            objInfo.DefaultHp = construction.buildingHP;
            string prefab = "";

            if (data.Value.category == 1)
            {
                prefab = "PlacementTest";
            }
            else
            {
                prefab = "PlacementTest3";
            }



            objInfo.Prefeb = Resources.Load<GameObject>
                (string.Format(prefabPathFormat, objInfo.Kind.ToString(), prefab));

            string[] needItemKeys = construction.buildCostID.Split('_');
            string[] needItemValues = construction.buildCostValue.Split('_');
            for (int i = 0; i < needItemKeys.Length; i++)
            {
                objInfo.NeedItems.Add(needItemKeys[i], int.Parse(needItemValues[i]));
            }


            objInfo.Icon = Resources.Load<Sprite>
                (string.Format(spritePathFormat, objInfo.Kind.ToString(), data.Value.buildingIcon));
            //objInfo.Feature = 

            objects.Add(objInfo);

        }
    }

    private void SetTurretObject()
    {
        var allData = DataTableManager.TurretTable.GetAll();
        foreach (var data in allData)
        {
            PlacementObjectInfo objInfo = new PlacementObjectInfo();
            objInfo.ID = data.Value.buildingID;
            var construction = DataTableManager.ConstructionTable.Get(objInfo.ID);

            objInfo.MaxBuildCount = Random.Range(1, 11);
            string[] xy = construction.lengthXY.Split('_');
            if (int.TryParse(xy[0], out int x) && int.TryParse(xy[1], out int y))
            {
                objInfo.Size = new Vector2Int(x, y);
            }
            else
            {
                Debug.LogError("肋给等 单捞磐!");
            }

            objInfo.Kind = (StructureKind)construction.category;
            objInfo.Name = data.Value.buildingNameID.ToString();
            objInfo.DefaultHp = construction.buildingHP;
            string prefab = "";

            if (data.Value.category == 1)
            {
                prefab = "PlacementTest 1";
            }
            else
            {
                prefab = "PlacementTest Lv2";
            }



            objInfo.Prefeb = Resources.Load<GameObject>
                (string.Format(prefabPathFormat, objInfo.Kind.ToString(), prefab));

            string[] needItemKeys = construction.buildCostID.Split('_');
            string[] needItemValues = construction.buildCostValue.Split('_');
            for (int i = 0; i < needItemKeys.Length; i++)
            {
                objInfo.NeedItems.Add(needItemKeys[i], int.Parse(needItemValues[i]));
            }


            objInfo.Icon = Resources.Load<Sprite>
                (string.Format(spritePathFormat, objInfo.Kind.ToString(), data.Value.buildingIcon));
            //objInfo.Feature = 

            objects.Add(objInfo);

        }
    }

    private void SetOtherObject()
    {
        var allData = DataTableManager.OtherTable.GetAll();
        foreach (var data in allData)
        {
            PlacementObjectInfo objInfo = new PlacementObjectInfo();
            objInfo.ID = data.Value.buildingID;
            var construction = DataTableManager.ConstructionTable.Get(objInfo.ID);

            objInfo.MaxBuildCount = Random.Range(1, 11);
            string[] xy = construction.lengthXY.Split('_');
            if (int.TryParse(xy[0], out int x) && int.TryParse(xy[1], out int y))
            {
                objInfo.Size = new Vector2Int(x, y);
            }
            else
            {
                Debug.LogError("肋给等 单捞磐!");
            }

            objInfo.Kind = (StructureKind)construction.category;
            objInfo.Name = data.Value.buildingNameID.ToString();
            objInfo.DefaultHp = construction.buildingHP;
            string prefab = "";

            if (data.Value.category == 1)
            {
                prefab = "FencePrefab1";
            }
            else
            {
                prefab = "FencePrefab2";
            }



            objInfo.Prefeb = Resources.Load<GameObject>
                (string.Format(prefabPathFormat, objInfo.Kind.ToString(), prefab));

            string[] needItemKeys = construction.buildCostID.Split('_');
            string[] needItemValues = construction.buildCostValue.Split('_');
            for (int i = 0; i < needItemKeys.Length; i++)
            {
                objInfo.NeedItems.Add(needItemKeys[i], int.Parse(needItemValues[i]));
            }


            objInfo.Icon = Resources.Load<Sprite>
                (string.Format(spritePathFormat, objInfo.Kind.ToString(), data.Value.buildingIcon));
            //objInfo.Feature = 

            objects.Add(objInfo);

        }
    }
}
