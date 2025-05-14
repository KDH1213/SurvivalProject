using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeStatData", menuName = "System/Stat/LifeStatData", order = 1)]
public class LifeStatData : ScriptableObject
{

    [field: SerializeField]
    public SerializedDictionary<NormalSkillType, List<float>> NormalSkillStatTable { get; private set; } = new SerializedDictionary<NormalSkillType, List<float>>();

    [field: SerializeField]
    public SerializedDictionary<NormalSkillType, Sprite> NormalSkillSpriteTable { get; private set; } = new SerializedDictionary<NormalSkillType, Sprite>();

    [field: SerializeField]
    public SerializedDictionary<LifeSkillType, List<float>> LifeSkillStatTable { get; private set; } = new SerializedDictionary<LifeSkillType, List<float>>();

    [field: SerializeField]
    public SerializedDictionary<LifeSkillType, Sprite> SkillSpriteTable { get; private set; } = new SerializedDictionary<LifeSkillType, Sprite>();

    [field: SerializeField]
    public SerializedDictionary<CraftingSkillType, List<float>> CraftingSkillStatTable { get; private set; } = new SerializedDictionary<CraftingSkillType, List<float>>();

    [field: SerializeField]
    public SerializedDictionary<CraftingSkillType, Sprite> CraftingSkillSpriteTable { get; private set; } = new SerializedDictionary<CraftingSkillType, Sprite>();


    [ContextMenu("Initialize")]
    public void OnCreate()
    {
        for (int i = 0; i < (int)NormalSkillType.End; ++i)
        {
            List<float> list = new List<float>();

            for (int j = 0; j < 10; ++j)
            {
                list.Add(5f);
            }

            NormalSkillStatTable.Add((NormalSkillType)i, list);
        }

        for (int i = 0; i < (int)LifeSkillType.End; ++i)
        {
            List<float> list = new List<float>();

            for (int j = 0; j < 10; ++j)
            {
                list.Add(5f);
            }

            LifeSkillStatTable.Add((LifeSkillType)i, list);
        }

        for (int i = 0; i < (int)CraftingSkillType.End; ++i)
        {
            List<float> list = new List<float>();

            for (int j = 0; j < 10; ++j)
            {
                list.Add(5f);
            }

            CraftingSkillStatTable.Add((CraftingSkillType)i, list);
        }
    }
}
