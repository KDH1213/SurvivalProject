using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SurvivalStatData : ScriptableObject
{
    [SerializedDictionary, SerializeField]
    private SerializedDictionary<SurvivalStatType, int> survivalStatTable = new SerializedDictionary<SurvivalStatType, int>();

    [SerializeField]
    private IntMinMax statMinMaxValue;

    public void Initialize(ref SerializedDictionary<SurvivalStatType, int> table)
    {
        table.Clear();

        for (int i = 0; i < (int)SurvivalStatType.End; ++i)
        {
            table.Add((SurvivalStatType)i, statMinMaxValue.max);
        }
    }
}
