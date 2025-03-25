using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharactorStats : MonoBehaviour
{
    [SerializeField]
    private StatData originalData;

    [SerializedDictionary, SerializeField]
    private SerializedDictionary<StatType, StatValue> currentStatTable = new SerializedDictionary<StatType, StatValue>();
    public SerializedDictionary<StatType, StatValue> CurrentStatTable { get { return currentStatTable; } }

    public bool IsDead { get; private set; } = false;

    public UnityEvent hitEvent;
    public UnityEvent deathEvent;
    public UnityEvent damegedEvent;

    private void Awake()
    {
        originalData.CopyStat(ref currentStatTable);
    }

    public float GetStatValue(StatType statType)
    {
        return currentStatTable[statType].Value;
    }

    public StatValue GetStat(StatType statType)
    {
        return currentStatTable[statType];
    }
}
