using System.Collections.Generic;
using UnityEngine;

public class StructureTable : DataTable
{
    private Dictionary<string, PlacementObjectInfo> items = new();
    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<PlacementObjectInfo>(textAsset.text);
        foreach ( var item in list )
        {
            Debug.Log(item);
        }
        items.Clear();
        /*list.ForEach(x =>
        {
            if (!items.ContainsKey((x.Kind, x.Level)))
            {
                items.Add((x.Kind, x.Level), x);
            }
            else Debug.Log("");
        }
        );
        itemCount = items.Count;*/

        
    }

}
