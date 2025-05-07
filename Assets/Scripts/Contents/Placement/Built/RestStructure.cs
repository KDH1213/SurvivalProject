using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    private Vector3 startTargetRestPosition;

    private void Awake()
    {
        var charactorStats = GetComponent<CharactorStats>();

        if(charactorStats != null)
        {
            charactorStats.deathEvent.AddListener(OnEndRest);
        }
    }

    private void Update()
    {
        if(!isRest)
        {
            return;
        }
        if (Time.time > recoverEndTime)
        {
            Time.timeScale = 1;
            OnEndRest();
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
        // Time.timeScale *= timeScale;

        Time.timeScale = 8;
        isRest = true;

        startTargetRestPosition = target.transform.position;
        target.SetActive(false);
    }

    public void OnEndRest()
    {
        if(!isRest)
        {
            return;
        }

        Time.timeScale = 1;
        target.SetActive(true);

        var charactorStats = GetComponent<CharactorStats>();
        if(charactorStats.IsDead)
        {
            if(NavMesh.SamplePosition(transform.position, out var hit, 100f, NavMesh.AllAreas))
            {
                target.transform.position = hit.position;
            }
            else
            {
                target.transform.position = startTargetRestPosition;
            }
        }
        else
        {
            target.transform.position = startTargetRestPosition;
        }

        isRest = false;

    }

    public void OnStartRest()
    {

    }

    public override void Load()
    {
        var data = SaveLoadManager.Data.placementSaveInfoList.Find(x => x.position == Position && x.id == ID);
        Hp = data.hp;
    }
}
