using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestStructure : PlacementObject
{
    [SerializeField]
    private float recoverHp;
    [SerializeField]
    private float recoverThirsty;
    [SerializeField]
    private float recoverHunger;
    [SerializeField]
    private float recoverFatigue; 
    [SerializeField]
    private float recoverTemperature;

    public override void SetData()
    {
        
    }

    public override void Interact(GameObject interactor)
    {
        interactor.GetComponent<PlayerStats>().AddStatType(StatType.HP, recoverHp);
        interactor.GetComponent<ThirstStat>().AddPenaltyValue(recoverThirsty);
        interactor.GetComponent<HungerStat>().AddPenaltyValue(recoverHunger);
        interactor.GetComponent<FatigueStat>().SubPenaltyValue(recoverFatigue);
        interactor.GetComponent<TemperatureStat>().AddPenaltyValue(recoverTemperature);
    }
}
