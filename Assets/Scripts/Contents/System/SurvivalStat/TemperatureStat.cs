using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct TemperaturePenaltyValueInfo
{
    public int step1PenaltyValue;
    public int step2PenaltyValue;
}

public class TemperatureStat : MonoBehaviour
{
    [SerializeField]
    private TemperaturePenaltyValueInfo coldPenaltyValueInfo;
    [SerializeField]
    private TemperaturePenaltyValueInfo heatPenaltyValueInfo;

    private int currentHeatResistance = 0;
    private int currentColdResistance = 0;

    private int stageTemperature;

    public UnityEvent<int> onColdPenaltyEvenet;
    public UnityEvent<int> onHeatPenaltyEvenet;

    private void Start()
    {
        var playerStats = GetComponent<PlayerStats>();

        var coldResistanceValue = playerStats.CurrentStatTable[StatType.ColdResistance];
        coldResistanceValue.OnChangeValue += OnChangeColdResistanceValue;

        var heatResistanceValue = playerStats.CurrentStatTable[StatType.HeatResistance];
        heatResistanceValue.OnChangeValue += OnChangeHeatResistanceValue;

        OnChangeColdResistanceValue(coldResistanceValue.Value);
        OnChangeHeatResistanceValue(heatResistanceValue.Value);
    }

    public void OnChangeColdResistanceValue(float value)
    {
        currentColdResistance = (int)value;

        if (stageTemperature >= 0)
        {
            onColdPenaltyEvenet?.Invoke(0);
            return;
        }

        var currentTemprature = currentColdResistance + stageTemperature;

        if (coldPenaltyValueInfo.step2PenaltyValue > currentTemprature)
        {
            onColdPenaltyEvenet?.Invoke(2);
        }
        else if (coldPenaltyValueInfo.step1PenaltyValue > currentTemprature)
        {
            onColdPenaltyEvenet?.Invoke(1);
        }
        else
        {
            onColdPenaltyEvenet?.Invoke(0);
        }
    }

    public void OnChangeHeatResistanceValue(float value)
    {
        currentHeatResistance = (int)value;

        if (stageTemperature <= 0)
        {
            onHeatPenaltyEvenet?.Invoke(0);
            return;
        }

        currentColdResistance = (int)value;
        var currentTemprature = currentColdResistance - stageTemperature;

        if (heatPenaltyValueInfo.step2PenaltyValue > currentTemprature)
        {
            onHeatPenaltyEvenet?.Invoke(2);
        }
        else if (heatPenaltyValueInfo.step1PenaltyValue > currentTemprature)
        {
            onHeatPenaltyEvenet?.Invoke(1);
        }
        else
        {
            onHeatPenaltyEvenet?.Invoke(0);
        }
    }

    public void OnChangeStageTemperature(int temperature)
    {
        stageTemperature = temperature;

        OnChangeColdResistanceValue(currentColdResistance);
        OnChangeHeatResistanceValue(currentHeatResistance);
    }

}
