using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstStat : SurvivalStatBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float startPenaltyPersent = 0.5f;

    [SerializeField]
    private float valueDownTime = 0.5f;

    private float currentTime = 0f;

    protected override void Awake()
    {
        survivalStatType = SurvivalStatType.Thirst;
        value = maxValue;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= valueDownTime)
        {
            currentTime -= valueDownTime;
            value -= maxValue * 0.01f;

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

        if (!isOnDebuff && IsActivationCheckPenalty())
        {
            OnStartPenalty();
        }
    }

    public override void OnStartPenalty()
    {
        base.OnStartPenalty();
    }

    public override void OnStopPenalty()
    {
        base.OnStopPenalty();
    }
}
