using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalStats : MonoBehaviour
{
    [field: SerializeField]
    public SurvivalStatData SurvivalStatData { get; private set; }
    [SerializedDictionary, SerializeField]
    private SerializedDictionary<SurvivalStatType, int> survivalStatTable = new SerializedDictionary<SurvivalStatType, int>();

    private void Awake()
    {
        SurvivalStatData.Initialize(ref survivalStatTable);
    }

    public float CalculateSpeedDebuff()
    {
        // if (survivalStatTable[SurvivalStatType.Hunger] < )
        return 1f;
    }

    internal float CalculateAttackSpeedDebuff()
    {
        throw new NotImplementedException();
    }
}
