using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirstStat : SurvivalStatBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float startPenaltyPersent = 0.5f;

    [SerializeField]
    private float valueDownTime = 0.5f;

    [SerializeField]
    private Slider thirstSlider;

    private float currentTime = 0f;

    private float thirstSkillValue = 0f;
    private float totalValueDownTime = 0f;

    protected override void Awake()
    {
        survivalStatType = SurvivalStatType.Thirst;
        value = maxValue;
        totalValueDownTime = valueDownTime;
        Load();
        OnChangeValue();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= totalValueDownTime)
        {
            currentTime -= totalValueDownTime;
            value -= maxValue * 0.01f;
            this.value = Mathf.Max(value, 0f);
            OnChangeValue();

            if (!isOnDebuff && IsActivationCheckPenalty())
            {
                OnStartPenalty();
            }
        }
    }

    protected override bool IsActivationCheckPenalty()
    {
        return value < startPenaltyPersent * maxValue;
    }

    public override void AddPenaltyValue(float value)
    {
        this.value += value;
        this.value = Mathf.Clamp(this.value, 0f, maxValue);
        OnChangeValue();

        if (!isOnDebuff && IsActivationCheckPenalty())
        {
            OnStartPenalty();
        }
    }
    public void OnSetThirstSkillValue(float value)
    {
        thirstSkillValue = value;
        totalValueDownTime = valueDownTime + thirstSkillValue;
    }
    public override void OnStartPenalty()
    {
        base.OnStartPenalty();
    }

    public override void OnStopPenalty()
    {
        base.OnStopPenalty();
    }

    public void OnChangeValue()
    {
        thirstSlider.value = value / maxValue;
    }
}
