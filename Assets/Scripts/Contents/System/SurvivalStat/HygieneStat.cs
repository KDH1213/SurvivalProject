using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HygieneStat : SurvivalStatBehaviour
{
    protected override void Awake()
    {
        survivalStatType = SurvivalStatType.Hygiene;
    }
    public override void AddPenaltyValue(float value)
    {
        this.value -= value;
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
