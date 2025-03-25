using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerStat : SurvivalStatBehaviour
{
    protected override void Awake()
    {
        survivalStatType = SurvivalStatType.Hunger;
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
