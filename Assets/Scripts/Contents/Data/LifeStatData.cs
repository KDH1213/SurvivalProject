using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeStatData", menuName = "System/Stat/LifeStatData", order = 1)]
public class LifeStatData : ScriptableObject
{
    [field: SerializeField]
    public List<float> LevelList { get; private set; } = new List<float>();

    [field: SerializeField, SerializedDictionary]
    public SerializedDictionary<LifeSkillType, float> LifeSkillStatTable { get; private set; } = new SerializedDictionary<LifeSkillType, float>();


    [ContextMenu("Initialize")]
    public void OnCreate()
    {
        for (int i = 0; i < 100; ++i)
        {
            LevelList.Add(1000f);
        }

        for (int i = 0; i < (int)LifeSkillType.End; ++i)
        {
            LifeSkillStatTable.Add((LifeSkillType)i, 5f);
        }
    }
}
