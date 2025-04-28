using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageTemperatureData", menuName = "System/StageTemperature/StageTemperatureData")]
public class StageTemperatureData : ScriptableObject
{
    [field: SerializeField]
    public int StageDefalutTemperature { get; private set; }

    [field: SerializeField]
    public List<IntMinMax> DailyTemperatureRangeList { get; private set; } = new List<IntMinMax>();

    [ContextMenu("Init")]
    private void Initialize()
    {
        DailyTemperatureRangeList.Clear();
        for (int i = 0; i < 24; ++i)
        {
            DailyTemperatureRangeList.Add(new IntMinMax());
        }
    }
}
