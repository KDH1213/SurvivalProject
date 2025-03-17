using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableManager()
    {
        //foreach(var id in DataTableIds.String)
        //{ 
        //    var table = new StringTable();
        //    table.Load(id);
        //    tables.Add(id, table);
        //}

    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError("테이블 없음");
            return default(T);
        }

        return tables[id] as T;
    }
}
