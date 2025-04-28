using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct RespawnInfo
{
    public GameObject owner;
    public float respawntime;
}

public class StageManager : MonoBehaviour, ISaveLoadData
{
    [SerializeField]
    private SceneSwitcher sceneSwitcher;

    [SerializedDictionary, SerializeField]
    private SerializedDictionary<InteractType, List<GameObject>> interactTable = new SerializedDictionary<InteractType, List<GameObject>>();

    private PriorityQueue<RespawnInfo, float> respawnObjectQueue = new PriorityQueue<RespawnInfo, float>();
    public UnityAction onSaveEvent; 

    private Collider[] colliders = new Collider[1];
    [SerializeField]
    private LayerMask respawnLayerMask;

    public UnityEvent OnInitializeEvent;

    [SerializeField]
    private StageTemperatureData stageTemperatureData;

    public UnityEvent<int> onChangeTemperatureEvent;
    private int stageTemperature = 0;

    private void Awake()
    {
        stageTemperature = stageTemperatureData.StageDefalutTemperature;

        var sceneSwitchers = GetComponentsInChildren<SceneSwitcher>();

        foreach (var sceneSwitcher in sceneSwitchers)
        {
            sceneSwitcher.onSceneSwitchEvent.AddListener(onSaveEvent);
        }

        onSaveEvent += Save;

        for (int i = 0; i < (int)InteractType.End; ++i)
        {
            var type = (InteractType)i;
            if (interactTable.ContainsKey(type))
            {
                foreach (var interact in interactTable[type])
                {
                    interact.GetComponent<IInteractable>().OnEndInteractEvent.AddListener(OnStartRespawn);
                }
            }
        }

        if (SaveLoadManager.Data != null)
        {
            Load();
        }

        OnInitializeEvent?.Invoke();
    }

    private void Start()
    {
        onChangeTemperatureEvent?.Invoke(stageTemperature);
    }

    public void OnStartRespawn(GameObject target)
    {
        var respawn = target.GetComponent<IRespawn>();

        var respawnInfo = new RespawnInfo();
        respawnInfo.owner = target;
        respawnInfo.respawntime = Time.time + respawn.RespawnTime;
        respawnObjectQueue.Enqueue(respawnInfo, respawnInfo.respawntime);
    }

    private void Update()
    {
        while (respawnObjectQueue.Count > 0 && respawnObjectQueue.Peek().respawntime <= Time.time)
        {
            var target = respawnObjectQueue.Dequeue();

            var interactable = target.owner.GetComponent<IInteractable>();
            if (interactable != null )
            {
                var targetTransform = target.owner.transform;
                if(Physics.OverlapBoxNonAlloc(targetTransform.position, targetTransform.lossyScale * 0.5f, colliders, Quaternion.identity, respawnLayerMask) == 0)
                {
                    target.owner.SetActive(true);
                }
                else
                {
                    target.respawntime += 1f;
                    respawnObjectQueue.Enqueue(target, target.respawntime);
                }

            }
            else
            {
                target.owner.SetActive(true);
            }
        }
    }

    private float currentTime;

    [ContextMenu("FindObject")]
    private void OnFindInteractObject()
    {
        interactTable.Clear();
        var GameObjects = FindObjectsOfType<GameObject>();

        foreach (var gameObject in GameObjects)
        {
            var interact = gameObject.GetComponent<IInteractable>();
            if (interact == null)
            {
                continue;
            }

            if (interactTable.ContainsKey(interact.InteractType))
            {
                interactTable[interact.InteractType].Add(gameObject);
            }
            else
            {
                List<GameObject> list = new List<GameObject>();
                list.Add(gameObject);
                interactTable.Add(interact.InteractType, list);
            }
        }
    }

    public void OnSave()
    {
        onSaveEvent?.Invoke();
        GameObject.FindWithTag(Tags.GameTimer).GetComponent<GameTimeManager>().Save();
        SaveLoadManager.Save();
    }

    private void OnApplicationQuit()
    {
        onSaveEvent?.Invoke();
        GameObject.FindWithTag(Tags.GameTimer).GetComponent<GameTimeManager>().Save();
        SaveLoadManager.Save();
    }

#if !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            onSaveEvent?.Invoke();
            GameObject.FindWithTag(Tags.GameTimer).GetComponent<GameTimeManager>().Save();
            SaveLoadManager.Save();
        }      
    }
#endif

    public void OnGameOver()
    {

    }

    public void OnRestart()
    {
        SaveLoadManager.Data.ResetStageInfo();
        sceneSwitcher.SwitchScene(SceneName.Develop);
        SaveLoadManager.Data.playerSaveInfo.hp = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerStats>().GetStat(StatType.HP).MaxValue;
        SaveLoadManager.Save();
    }

    public void Save()
    {
        if(SaveLoadManager.Data == null)
        {
            return;
        }

        SaveLoadManager.Data.isRestart = false;
        SaveLoadManager.Data.gatherSaveInfoTable.Clear();
        var currentTime = Time.time;

        while (respawnObjectQueue.Count > 0)
        {
            var target = respawnObjectQueue.Dequeue();
            var respawn = target.owner.GetComponent<IRespawn>();
            respawn.SetRemainTime(target.respawntime - currentTime);
        }

        for (int i = 0; i < (int)InteractType.Monster; ++i)
        {
            var interactType = (InteractType)i;
            if (interactTable.ContainsKey((InteractType)i))
            {
                var gatherSaveInfoList = new List<GatherSaveInfo>();
                var list = interactTable[(InteractType)i];
                for (int j = 0; j < list.Count; ++j)
                {
                    var gatherSaveInfo = new GatherSaveInfo();
                    var respawnInfo = list[j].GetComponent<IRespawn>();
                    gatherSaveInfo.remainingTime = respawnInfo.RemainingTime;
                    gatherSaveInfo.respawnPosition = respawnInfo.RespawnPosition;
                    gatherSaveInfo.isRespawn = respawnInfo.IsRespawn;
                    gatherSaveInfo.position = list[j].transform.position;
                    gatherSaveInfoList.Add(gatherSaveInfo);
                }

                SaveLoadManager.Data.gatherSaveInfoTable.Add(interactType, gatherSaveInfoList);
            }
        }

        var monsterSaveInfoList = new List<MonsterSaveInfo>();
        var monsterList = interactTable[InteractType.Monster];
        for (int i = 0; i < monsterList.Count; ++i)
        {
            var monsterSaveInfo = new MonsterSaveInfo();
            var respawnInfo = monsterList[i].GetComponent<IRespawn>();
            monsterSaveInfo.remainingTime = respawnInfo.RemainingTime;
            monsterSaveInfo.respawnPosition = respawnInfo.RespawnPosition;
            monsterSaveInfo.isRespawn = respawnInfo.IsRespawn;
            monsterSaveInfo.position = monsterList[i].transform.position;

            var monsterStats = monsterList[i].GetComponent<MonsterStats>();

            if(monsterSaveInfo.isRespawn)
            {
                monsterSaveInfo.hp = 0f;
            }
            else
            {
                if(monsterStats.CurrentStatTable.Count == 0)
                {
                    monsterStats.OnInitialize();
                }
                monsterSaveInfo.hp = monsterList[i].GetComponent<MonsterStats>().Hp;
            }

            monsterSaveInfoList.Add(monsterSaveInfo);
        }

        SaveLoadManager.Data.monsterSaveInfoList = monsterSaveInfoList;
}

    public void Load()
    {
        if(SaveLoadManager.Data.isRestart)
        {
            return;
        }

        var gatherSaveInfoTable = SaveLoadManager.Data.gatherSaveInfoTable;

        for (int i = 0; i < (int)InteractType.Monster; ++i)
        {
            var interactType = (InteractType)i;
            if (gatherSaveInfoTable.ContainsKey((InteractType)i))
            {
                var gatherSaveInfoList = gatherSaveInfoTable[interactType];
                var list = interactTable[(InteractType)i];
                for (int j = 0; j < gatherSaveInfoList.Count; ++j)
                {
                    var respawn = list[j].GetComponent<Gather>();
                    respawn.LoadData(gatherSaveInfoList[j]);

                    if(gatherSaveInfoList[j].isRespawn)
                    {
                        var respawnInfo = new RespawnInfo();
                        respawnInfo.owner = list[j];
                        respawnInfo.respawntime = Time.time + respawn.RemainingTime;
                        respawnObjectQueue.Enqueue(respawnInfo, respawn.RemainingTime);
                    }
                }
            }
        }

        var monsterSaveInfoList = SaveLoadManager.Data.monsterSaveInfoList;
        var monsterList = interactTable[InteractType.Monster];
        for (int i = 0; i < monsterSaveInfoList.Count; ++i)
        {
            var monsterFSM = monsterList[i].GetComponent<MonsterFSM>();
            monsterFSM.LoadData(monsterSaveInfoList[i]);

            if (monsterSaveInfoList[i].isRespawn)
            {
                var respawnInfo = new RespawnInfo();
                respawnInfo.owner = monsterList[i];
                respawnInfo.respawntime = Time.time + monsterFSM.RemainingTime;
                respawnObjectQueue.Enqueue(respawnInfo, monsterFSM.RemainingTime);
            }
        }

    }
}
