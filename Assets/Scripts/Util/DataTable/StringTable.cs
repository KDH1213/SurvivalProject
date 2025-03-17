using CsvHelper;
using System.Collections.Generic;
using UnityEngine;

public class StringTable : DataTable
{
    public class Data
    {
        public string Id { get; set; }
        public string String { get; set; }
    }

    private Dictionary<string, string> dictionoary = new Dictionary<string, string>();

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        var list = LoadCSV<Data>(textAsset.text);

        dictionoary.Clear();

        foreach (var item in list)
        {
            if (!dictionoary.ContainsKey(item.Id))
            {
                dictionoary.Add(item.Id, item.String);
            }
            else
            {
                Debug.LogError($"Key Duplicated {item.Id}");
            }
        }
    }

    public string Get(string key)
    {
        if (!dictionoary.ContainsKey(key))
        {
            Debug.LogError($"{key} None");
            return "Å° ¾øÀ½";
        }

        return dictionoary[key];
    }
}
