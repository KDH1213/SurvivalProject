using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeStatData", menuName = "System/Stat/LifeStatData", order = 1)]
public class LifeStatData : ScriptableObject
{
    [field: SerializeField]
    public SerializedDictionary<SkillType, List<float>> SkillStatTable { get; private set; } = new SerializedDictionary<SkillType, List<float>>();


    [field: SerializeField]
    public List<SkillType> NormalSkillTypeList { get; private set; } = new List<SkillType> { };

    [field: SerializeField]
    public List<SkillType> LifeSkillTypeList { get; private set; } = new List<SkillType> { };

    [field: SerializeField]
    public List<SkillType> CraftingSkillTypeList { get; private set; } = new List<SkillType> { };

    [field: SerializeField]
    public SerializedDictionary<SkillType, Sprite> SkillSpriteTable { get; private set; } = new SerializedDictionary<SkillType, Sprite>();


    [ContextMenu("Initialize")]
    public void OnCreate()
    {
        for (int i = 0; i < (int)SkillType.End; ++i)
        {
            var list = new List<float>();

            for(int j = 0; j < 10; ++j)
            {
                list.Add(5f);
            }

            SkillStatTable.Add((SkillType)i, list);
        }
    }
}
