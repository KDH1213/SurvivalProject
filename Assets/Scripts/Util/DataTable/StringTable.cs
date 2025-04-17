using CsvHelper;
using System.Collections.Generic;
using UnityEngine;

public class StringTable : DataTable
{
    public class Data
    {

        public int ID { get; set; }
        public string String { get; set; }
    }

    private Dictionary<int, string> dictionoary = new Dictionary<int, string>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<Data>(textAsset.text);

        dictionoary.Clear();

        foreach (var item in list)
        {
            if (!dictionoary.ContainsKey(item.ID))
            {
                dictionoary.Add(item.ID, item.String);
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.ID}");
            }
        }
    }

    public string Get(int key)
    {
        if (!dictionoary.ContainsKey(key))
        {
            Debug.LogError($"{key} None");
            return "Å° ¾øÀ½";
        }

        return dictionoary[key];
    }
}
