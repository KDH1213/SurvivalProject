using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class RestStructure : PlacementObject
{
    [SerializeField]
    private DateTime recoverCurTime;
    [SerializeField]
    private float recoverDuration;
    [SerializeField]
    private DateTime recoverEndTime;
    private float recoverPerTime = 60;
    [SerializeField]
    private float recoverPerFatigue = 100;
    [SerializeField]
    private float recoverTemperature;
    [SerializeField]
    private GameTimeManager timeManager;
    private TimeService service;

    private GameObject target;
    public bool isRest = false;
    private int timeScale = 2;

    public UnityEvent endRest;
    
    private Vector3 startTargetRestPosition;

    private void Awake()
    {
        var charactorStats = GetComponent<CharactorStats>();
        
        if (charactorStats != null)
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
        if (service.CurrentTime > recoverEndTime)
        {
            Time.timeScale = 1;
            endRest.Invoke();
        }
        if (service.CurrentTime > recoverCurTime)
        {
            recoverCurTime = service.GetCalculateTime(recoverPerTime / 5 / 60);
            target.GetComponent<FatigueStat>().SubPenaltyValue(recoverPerFatigue);
        }

    }

    [ContextMenu("setData")]
    public override void SetData()
    {
        if (ObjectPoolManager.Instance.ObjectPoolTable.TryGetValue(ObjectPoolType.HpBar, out var component))
        {
            var stats = GetComponent<StructureStats>();
            var hpBarObjectPool = component.GetComponent<UIHpBarObjectPool>();
            var hpBar = hpBarObjectPool.GetHpBar();
            hpBar.GetComponent<UITargetFollower>().SetTarget(transform, Vector3.zero);
            hpBar.SetTarget(stats);

            stats.deathEvent.AddListener(() => { hpBar.gameObject.SetActive(false); });
            disableEvent.AddListener(() => { if (hpBar != null) { hpBar.gameObject.SetActive(false); } });
            enableEvent.AddListener(() => { if (hpBar != null) { hpBar.gameObject.SetActive(true); } });
        }
        recoverPerFatigue = DataTableManager.StructureTable.Get(ID).FatigueReductionPerMinute;

    }

    public override void Interact(GameObject interactor)
    {
        uiController.OnOpenRestUI(target, this);
        
        target = interactor;
    }

    public void SetRest(float endTime)
    {
        timeManager = GameObject.FindWithTag(Tags.GameTimer).GetComponent<GameTimeManager>();
        service = timeManager.TimeService;
        recoverEndTime = service.GetCalculateTime(endTime / 5);
        recoverCurTime = service.GetCalculateTime(recoverPerTime / 5 / 60);

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
        var table = GetComponent<StructureStats>().CurrentStatTable;
        table[StatType.HP].SetValue(Hp);
    }
}
