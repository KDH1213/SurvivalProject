using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FatigueStat : SurvivalStatBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float startPenaltyPersent = 0.7f;

    [SerializeField]
    private float extraValue = 1.5f;
    private bool isExtraValue = false;

    [SerializeField]
    private Slider fatigueSlider;
    protected override void Awake()
    {
        survivalStatType = SurvivalStatType.Fatigue;
        Load();
        OnChangeValue();
    }
    public override void AddPenaltyValue(float value)
    {
        if(isExtraValue)
        {
            this.value += value * extraValue;
        }
        else
        {
            this.value += value;
        }


        this.value = Mathf.Clamp(this.value, 0f, maxValue);
        OnChangeValue();

        if (!isOnDebuff && IsActivationCheckPenalty())
        {
            OnStartPenalty();
        }
    }

    public void SubPenaltyValue(float value)
    {
        this.value -= value;
        this.value = Mathf.Clamp(this.value, 0f, maxValue);
        OnChangeValue();

        if (isOnDebuff && !IsActivationCheckPenalty())
        {
            OnStopPenalty();
        }
    }

    protected override bool IsActivationCheckPenalty()
    {
        return value > maxValue * startPenaltyPersent;
    }

    public override void OnStartPenalty()
    {
        base.OnStartPenalty();
    }

    public override void OnStopPenalty()
    {
        base.OnStopPenalty();
    }

    public void OnStartThirstPenalty()
    {
        isExtraValue = true;
    }
    public void OnEndThirstPenalty()
    {
        isExtraValue = false;
    }

    public void OnChangeValue()
    {
        fatigueSlider.value = value / maxValue;
    }
}
