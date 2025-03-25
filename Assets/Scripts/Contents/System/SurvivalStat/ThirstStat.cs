using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstStat : SurvivalStatBehaviour
{
    protected override void Awake()
    {
        survivalStatType = SurvivalStatType.Thirst;
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
