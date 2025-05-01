using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestStructure : PlacementObject
{
    [SerializeField]
    private float recoverCurTime;
    [SerializeField]
    private float recoverDuration;
    [SerializeField]
    private float recoverEndTime;
    private float recoverPerTime = 60;
    [SerializeField]
    private float recoverPerFatigue;
    [SerializeField]
    private float recoverTemperature;

    private GameObject target;
    public bool isRest = false;
    private int timeScale = 2;

    private void Update()
    {
        if(!isRest)
        {
            return;
        }
        if (Time.time > recoverEndTime)
        {
            Time.timeScale = 1;
            isRest = false;
        }
        else if (Time.time > recoverCurTime)
        {
            recoverCurTime = Time.time + recoverPerTime;
            target.GetComponent<FatigueStat>().SubPenaltyValue(recoverPerFatigue);
        }

    }

    [ContextMenu("setData")]
    public override void SetData()
    {
        //recoverEndTime = Time.time + recoverDuration;
        recoverCurTime = Time.time + recoverPerTime;
    }

    public override void Interact(GameObject interactor)
    {
        uiController.OnOpenRestUI(target, this);
        
        target = interactor;
    }

    public void SetRest(float endTime)
    {
        recoverEndTime = endTime;
        Time.timeScale *= timeScale;
        isRest = true;
    }
}
