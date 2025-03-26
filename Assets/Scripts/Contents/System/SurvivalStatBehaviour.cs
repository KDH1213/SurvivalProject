using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent onStartPenaltyEvent;
    public UnityEvent onEndPenaltyEvent;

    protected virtual void Awake()
    {
        
    }

    protected virtual bool IsActivationCheckPenalty()
    {
        return false;
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
        onStartPenaltyEvent?.Invoke();
    }                      

    public virtual void OnStopPenalty()
    {
        isOnDebuff= false; 
        onEndPenaltyEvent?.Invoke();
    }
}
