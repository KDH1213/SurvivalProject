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


    [SerializeField]
    private Sprite coldDebuffIcon;

    [SerializeField]
    private Sprite heatDebuffIcon;

    private void Awake()
    {
        var stageManagerObject = GameObject.FindWithTag(Tags.StageManager);
        if(stageManagerObject != null)
        {
            stageManagerObject.GetComponent<StageManager>().onChangeTemperatureEvent.AddListener(OnChangeStageTemperature);
        }

        var uIDebuffIcon = GameObject.FindWithTag(Tags.UIDebuffIcon);
        if (uIDebuffIcon != null)
        {
            var uIDebuffIconView = uIDebuffIcon.GetComponent<UIDebuffIconView>();
            var debuff = uIDebuffIconView.CreateDebuffIcon(coldDebuffIcon, "느려짐", "이동속도 감소");
            onColdPenaltyEvenet.AddListener((level) => 
            {
                if (level == 0)
                {
                    debuff.gameObject.SetActive(false);
                }
                else
                {
                    debuff.gameObject.SetActive(true);
                }
            });
            debuff.gameObject.SetActive(false);

            var newdebuff = uIDebuffIconView.GetComponent<UIDebuffIconView>().CreateDebuffIcon(heatDebuffIcon, "체력 감소", "체력 떨어짐");
            onHeatPenaltyEvenet.AddListener((level) =>
            {
                if (level == 0)
                {
                    newdebuff.gameObject.SetActive(false);
                }
                else
                {
                    newdebuff.gameObject.SetActive(true);
                }
            });
            newdebuff.gameObject.SetActive(false);
        }
    }

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

        if (coldPenaltyValueInfo.step2PenaltyValue >= currentTemprature)
        {
            onColdPenaltyEvenet?.Invoke(2);
        }
        else if (coldPenaltyValueInfo.step1PenaltyValue >= currentTemprature)
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

        currentHeatResistance = (int)value;
        var currentTemprature = currentHeatResistance - stageTemperature;

        if (heatPenaltyValueInfo.step2PenaltyValue <= currentTemprature)
        {
            onHeatPenaltyEvenet?.Invoke(2);
        }
        else if (heatPenaltyValueInfo.step1PenaltyValue <= currentTemprature)
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

        if (temperature >= 0)
        {
            onColdPenaltyEvenet?.Invoke(0);
        }
        else if (temperature <= 0)
        {
            onHeatPenaltyEvenet?.Invoke(0);
        }

        if (temperature > 0)
        {
            OnChangeHeatResistanceValue(currentHeatResistance);
        }
        else if (temperature < 0)
        {
            OnChangeColdResistanceValue(currentColdResistance);
        }
    }

}
