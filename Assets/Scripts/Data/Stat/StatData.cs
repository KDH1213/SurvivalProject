using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatData", menuName = "System/Stat/StatData", order = 2)]
public class StatData : ScriptableObject
{
    [SerializedDictionary, SerializeField]
    public SerializedDictionary<StatType, StatValue> StatTable = new SerializedDictionary<StatType, StatValue>();

    public void CopyStat(ref SerializedDictionary<StatType, StatValue> statTable)
    {
        statTable.Clear();

        foreach (var stat in StatTable)
        {
            statTable.Add(stat.Key, new StatValue(stat.Value));
        }
    }
}
