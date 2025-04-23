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
    [SerializeField]
    private float recoverPerTime;
    [SerializeField]
    private float recoverPerFatigue;
    [SerializeField]
    private float recoverTemperature;

    private GameObject target;
    private bool isRest = false;
    [SerializeField]
    private int timeScale;

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
        recoverEndTime = Time.time + recoverDuration;
        recoverCurTime = Time.time + recoverPerTime;
    }

    public override void Interact(GameObject interactor)
    {
        isRest = true;
        Time.timeScale *= timeScale;
        target = interactor;
    }
}
