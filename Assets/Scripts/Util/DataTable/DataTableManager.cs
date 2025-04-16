using System.Collections.Generic;
using UnityEngine;
using static ArmorData;
using static WeaponData;

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

        foreach (var id in ItemTableIds.String)
        {
            var table = new ItemTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach (var id in WeaponTableIds.String)
        {
            var table = new WeaponTable();
            table.Load(id);
            tables.Add(id, table);
        }

        foreach (var id in ArmorTableIds.String)
        {
            var table = new ArmorTable();
            table.Load(id);
            tables.Add(id, table);
        }


        var tableConstruction = new ConstructionTable();
        string idConstruction = PlacementTableIds.ConstructionTable;
        tableConstruction.Load(idConstruction);
        tables.Add(idConstruction, tableConstruction);

        var tableStructure = new StructureTable();
        string idStructure = PlacementTableIds.StructureTable;
        tableStructure.Load(idStructure);
        tables.Add(idStructure, tableStructure);

        var tableItemCreate = new ItemCreateTable();
        string idItemCreate = PlacementTableIds.ItemCreateTable;
        tableItemCreate.Load(idItemCreate);
        tables.Add(idItemCreate, tableItemCreate);
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

    public static ItemTable ItemTable
    {
        get
        {
            return Get<ItemTable>(ItemTableIds.String[0]);
        }
    }

    public static WeaponTable WeaponTable
    {
        get
        {
            return Get<WeaponTable>(WeaponTableIds.String[0]);
        }
    }

    public static ArmorTable ArmorTable
    {
        get
        {
            return Get<ArmorTable>(ArmorTableIds.String[0]);
        }
    }


    public static ConstructionTable ConstructionTable
    {
        get { return Get<ConstructionTable>(PlacementTableIds.ConstructionTable); }
    }

    public static StructureTable StructureTable
    {
        get { return Get<StructureTable>(PlacementTableIds.StructureTable); }
    }
    public static ItemCreateTable ItemCreateTable
    {
        get { return Get<ItemCreateTable>(PlacementTableIds.ItemCreateTable); }
    }
}
