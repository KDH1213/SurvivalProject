
using CsvHelper;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelData
{

    public int Level { get; set; }
    public int RequiredExp { get; set; }
}

public class PlayerLevelTable : DataTable
{
    public List<PlayerLevelData> PlayerLevelDataList { get; private set; }

    public override void Load(string filename)
    {
        var path = string.Format(FormatPath, filename);

        var textAsset = Resources.Load<TextAsset>(path);
        PlayerLevelDataList = LoadCSV<PlayerLevelData>(textAsset.text);

    }

    public int Get(int key)
    {
        return PlayerLevelDataList[key].RequiredExp;
    }
}
