using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableManager()
    {
        //foreach (var id in DataTableIds.String)
        //{
        //    var table = new StringTable();
        //    table.Load(id);
        //    tables.Add(id, table);
        //}

        var table = new ConstructionTable();
        string id = PlacementTableIds.ConstructionTable;
        table.Load(id);
        tables.Add(id, table);

        var tableFarm = new FarmTable();
        string idFarm = PlacementTableIds.FarmTable;
        tableFarm.Load(idFarm);
        tables.Add(idFarm, tableFarm);

        var tableTurret = new TurretTable();
        string idTurret = PlacementTableIds.TurretTable;
        tableTurret.Load(idTurret);
        tables.Add(idTurret, tableTurret);
    }


    public static ItemTable ItemTable
    {
        get
        {
            return Get<ItemTable>(ItemTableIds.String[0]);
        }
    }


    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError("���̺� ����");
            return default(T);
        }

        return tables[id] as T;
    }

    public static ConstructionTable ConstructionTable
    {
        get { return Get<ConstructionTable>(PlacementTableIds.ConstructionTable); }
    }

    public static FarmTable FarmTable
    {
        get { return Get<FarmTable>(PlacementTableIds.FarmTable); }
    }

    public static TurretTable TurretTable
    {
        get { return Get<TurretTable>(PlacementTableIds.TurretTable); }
    }
}
