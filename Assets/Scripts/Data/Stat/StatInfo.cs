using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatInfo
{
    public StatType statType;
    public float value;

    public StatInfo(StatType statType, float value)
    {
        this.statType = statType;
        this.value = value;
    }
}