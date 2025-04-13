using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SurvivalStatBehaviour : MonoBehaviour, IPenalty, ISaveLoadData
{
    [SerializeField]
    protected SurvivalStatType survivalStatType;
    public SurvivalStatType PenaltyType { get { return survivalStatType; } }

    [SerializeField]
    protected float maxValue = 10000f;

    protected float value;
    protected bool isOnDebuff;
    public bool IsOnPenalty { get { return isOnDebuff; } }

    public UnityEvent<SurvivalStatType> onStartPenaltyEvent;
    public UnityEvent<SurvivalStatType> onEndPenaltyEvent;
    public UnityEvent<SurvivalStatType> OnStartPenaltyEvent => onStartPenaltyEvent;
    public UnityEvent<SurvivalStatType> OnEndPenaltyEvent => onEndPenaltyEvent;
    protected virtual void Awake()
    {
        Load();
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
        onStartPenaltyEvent?.Invoke(survivalStatType);
    }                      

    public virtual void OnStopPenalty()
    {
        isOnDebuff= false; 
        onEndPenaltyEvent?.Invoke(survivalStatType);
    }

    public virtual void Save()
    {
        if(SaveLoadManager.Data != null)
        {
            SaveLoadManager.Data.PlayerSaveData.playerSaveInfo.survivalStatValues[(int)survivalStatType] = value;
        }
    }

    public virtual void Load()
    {
        if (SaveLoadManager.Data != null && SaveLoadManager.Data.PlayerSaveData.playerSaveInfo.survivalStatValues != null)
        {
            value = SaveLoadManager.Data.PlayerSaveData.playerSaveInfo.survivalStatValues[(int)survivalStatType];
        }
    }
}
