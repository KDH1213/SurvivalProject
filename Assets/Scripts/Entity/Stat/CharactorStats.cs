using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharactorStats : MonoBehaviour
{
    [SerializeField]
    protected StatData originalData;

    [SerializedDictionary, SerializeField]
    protected SerializedDictionary<StatType, StatValue> currentStatTable = new SerializedDictionary<StatType, StatValue>();
    public SerializedDictionary<StatType, StatValue> CurrentStatTable { get { return currentStatTable; } }

    public bool IsDead { get; protected set; } = false;

    public UnityEvent hitEvent;
    public UnityEvent deathEvent;
    public UnityEvent damegedEvent;

    protected virtual void Awake()
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

    public virtual float AttackPower
    {
        get
        {
            if (currentStatTable.TryGetValue(StatType.BasicAttackPower, out var value))
            {
                return value.Value;
            }
            return 0f;
        }
    }
}
