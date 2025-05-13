using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageTemperatureData", menuName = "System/StageTemperature/StageTemperatureData")]
public class StageTemperatureData : ScriptableObject
{
    [field: SerializeField]
    public int StageDefalutTemperature { get; private set; }

    [field: SerializeField]
    public IntMinMax StageMinMaxTemperature { get; private set; }

    [field: SerializeField]
    public List<IntMinMax> DailyTemperatureRangeList { get; private set; } = new List<IntMinMax>();

    [ContextMenu("Init")]
    private void Initialize()
    {
        DailyTemperatureRangeList.Clear();
        for (int i = 0; i < 24; ++i)
        {
            DailyTemperatureRangeList.Add(new IntMinMax(-2, 2));
        }
    }

    public int GetRandomTemperature(int hour)
    {
        if (hour < 0 || DailyTemperatureRangeList.Count <= hour)
        {
            return 0;
        }

        return DailyTemperatureRangeList[hour].GetRendomValue();
    }

    public int ClampTemperature(int temperature)
    {
        return Mathf.Clamp(temperature, StageMinMaxTemperature.min, StageMinMaxTemperature.max);
    }
}
