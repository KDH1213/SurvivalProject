using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureStat : SurvivalStatBehaviour
{
    protected override void Awake()
    {
        survivalStatType = SurvivalStatType.Temperature;
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
