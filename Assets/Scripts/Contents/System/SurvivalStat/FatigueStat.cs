using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatigueStat : SurvivalStatBehaviour
{
    protected override void Awake()
    {
        survivalStatType = SurvivalStatType.Fatigue;
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
