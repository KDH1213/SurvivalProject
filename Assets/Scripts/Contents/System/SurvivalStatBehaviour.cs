using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SurvivalStatBehaviour : MonoBehaviour, IPenalty
{
    [SerializeField]
    protected SurvivalStatType survivalStatType;
    public SurvivalStatType PenaltyType { get { return survivalStatType; } }

    [SerializeField]
    protected float maxValue = 10000f;

    protected float value;
    protected bool isOnDebuff;
    public bool IsOnPenalty { get { return isOnDebuff; } }

    protected virtual void Awake()
    {
        
    }

    public float Persent
    {
        get
        {
            return value / maxValue;
        }
    }

    public virtual void AddPenaltyValue(float value)
    {
        this.value += value;
    }

    public virtual void OnStartPenalty()
    {
        isOnDebuff = true;
    }

    public virtual void OnStopPenalty()
    {
        isOnDebuff= false;
    }
}
