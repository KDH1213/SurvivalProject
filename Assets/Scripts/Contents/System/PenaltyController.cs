using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TemperaturePenaltyInfo
{
    public float step1SpeedDownPersent;
    public float step2SpeedDownPersent;
    public float step1PenaltyAttackSpeedPersent;
    public float step2PenaltyAttackSpeedPersent;
}

public class PenaltyController : MonoBehaviour, IAct, ISaveLoadData
{
    private SerializedDictionary<SurvivalStatType, SurvivalStatBehaviour> penaltyTable = new SerializedDictionary<SurvivalStatType, SurvivalStatBehaviour>();
    public SerializedDictionary<SurvivalStatType, SurvivalStatBehaviour> PenaltyTable {  get { return penaltyTable; } }
    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private ActInfoData actInfoData;    

    [SerializeField]
    private float hungerSpeedDownPersent = 0.2f;
    [SerializeField]
    private float fatiguePenaltyAttackSpeedPersent = 0.5f;

    [SerializeField]
    private TemperaturePenaltyInfo coldTemperaturePenalty;
    [SerializeField]
    private TemperaturePenaltyInfo heatTemperaturePenalty;

    private bool isOnHungerPenalty = false;
    private bool isOnFatiguePenalty = false;

    private float currentSpeedPersent = 1f;
    private float currentAttackSpeedPersent = 1f;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();

        penaltyTable.Clear();

        var penaltys = GetComponents<SurvivalStatBehaviour>();

        foreach (var penalty in penaltys)
        {
            penaltyTable.Add(penalty.PenaltyType, penalty);
            penalty.OnStartPenaltyEvent.AddListener(OnStartPanelty);
            penalty.OnEndPenaltyEvent.AddListener(OnEndPanelty);
        }

        var temperatureStat = GetComponent<TemperatureStat>();
        temperatureStat.onColdPenaltyEvenet.AddListener(OnColdPanelty);
        temperatureStat.onHeatPenaltyEvenet.AddListener(OnHeatPanelty);
    }

    public float CalculatePenaltySpeed()
    {
        return currentSpeedPersent;
    }

    public float CalculatePenaltyAttackSpeed()
    {
        return currentAttackSpeedPersent;
    }

    public void OnPlayAct(int id)
    {
        var actInfoList = actInfoData.actDataTable[(ActType)id].actInfoList; // ActManager.actDataTable[id].actInfoList;
        foreach (var act in actInfoList)
        {
            penaltyTable[act.penaltyType].AddPenaltyValue(act.value);
        }
    }

    public void OnPlayAct(PlacementObject placementObj)
    {
        var actInfoList = placementObj.actInfos; // ActManager.actDataTable[id].actInfoList;
        foreach (var act in actInfoList)
        {
            penaltyTable[act.penaltyType].AddPenaltyValue(act.value);
        }
    }

    public void OnPlayAct(Dictionary<SurvivalStatType, int> penalties)
    {
        foreach (var act in penalties)
        {
            penaltyTable[act.Key].AddPenaltyValue(act.Value);
        }
    }

    // TODO ::Hunger컴포넌트에 연결
    public void OnStartPenaltyHpDown()
    {
        var takeDamage = GetComponent<AttackedTakeDamage>();
        DamageInfo damageInfo = new DamageInfo();
        damageInfo.damage = playerStats.GetStat(StatType.HP).MaxValue * 0.01f;
        takeDamage.OnAttack(this.gameObject, damageInfo);
    }

    public void OnStartPanelty(SurvivalStatType survivalStatType)
    {
        switch (survivalStatType)
        {
            case SurvivalStatType.Fatigue:
                currentAttackSpeedPersent -= fatiguePenaltyAttackSpeedPersent;
                isOnFatiguePenalty = true;
                break;
            case SurvivalStatType.Hunger:
                currentSpeedPersent -= hungerSpeedDownPersent;
                isOnHungerPenalty = true;
                break;
            case SurvivalStatType.Thirst:
                break;
            case SurvivalStatType.End:
            default:
                break;
        }
    }

    public void OnEndPanelty(SurvivalStatType survivalStatType)
    {
        switch (survivalStatType)
        {
            case SurvivalStatType.Fatigue:
                currentAttackSpeedPersent += fatiguePenaltyAttackSpeedPersent; 
                isOnFatiguePenalty = false;
                break;
            case SurvivalStatType.Hunger:
                currentSpeedPersent += hungerSpeedDownPersent;
                isOnHungerPenalty = false;
                break;
            case SurvivalStatType.Thirst:
                break;
            case SurvivalStatType.End:
            default:
                break;
        }
    }

    public void OnColdPanelty(int step)
    {
        if(step == 0)
        {
            currentSpeedPersent = 1f;
            currentAttackSpeedPersent = 1f;
        }
        else if(step == 1)
        {
            currentSpeedPersent = coldTemperaturePenalty.step1SpeedDownPersent;
            currentAttackSpeedPersent = coldTemperaturePenalty.step1SpeedDownPersent;
        }
        else
        {
            currentSpeedPersent = coldTemperaturePenalty.step2SpeedDownPersent;
            currentAttackSpeedPersent = coldTemperaturePenalty.step2SpeedDownPersent;
        }

        CheckOtherPanelty();
    }

    public void OnHeatPanelty(int step)
    {
        if (step == 0)
        {
            currentSpeedPersent = 1f;
            currentAttackSpeedPersent = 1f;
        }
        else if (step == 1)
        {
            currentSpeedPersent = heatTemperaturePenalty.step1SpeedDownPersent;
            currentAttackSpeedPersent = heatTemperaturePenalty.step1SpeedDownPersent;
        }
        else
        {
            currentSpeedPersent = heatTemperaturePenalty.step2SpeedDownPersent;
            currentAttackSpeedPersent = heatTemperaturePenalty.step2SpeedDownPersent;
        }

        CheckOtherPanelty();
    }

    private void CheckOtherPanelty()
    {
        if(isOnFatiguePenalty)
        {
            currentAttackSpeedPersent -= fatiguePenaltyAttackSpeedPersent;
        }
        if(isOnHungerPenalty)
        {
            currentSpeedPersent -= hungerSpeedDownPersent;
        }
    }

    public void Save()
    {
        var SurvivalStatBehaviours = GetComponents<SurvivalStatBehaviour>();

        foreach (var SurvivalStatBehaviour in SurvivalStatBehaviours)
        {
            SurvivalStatBehaviour.Save();
        }
    }

    public void Load()
    {
    }
}
