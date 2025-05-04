using AYellowpaper.SerializedCollections;
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
    public bool CanHit { get; protected set; } = true;

    public UnityEvent hitEvent;
    public UnityEvent deathEvent;
    public UnityEvent damegedEvent;

    protected virtual void Awake()
    {
        originalData.CopyStat(ref currentStatTable);
    }

    public float GetStatValue(StatType statType)
    {
        if(currentStatTable.TryGetValue(statType, out var statValue))
        {
            return statValue.Value;
        }
        return 0f;
    }

    public StatValue GetStat(StatType statType)
    {
        if (currentStatTable.TryGetValue(statType, out var statValue))
        {
            return statValue;
        }
        return null;
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

    public void OnDeath()
    {
        IsDead = true;
    }
}
