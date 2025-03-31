using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RespawnInfo
{
    public GameObject owner;
    public float respawntime;
}

public class StageManager : MonoBehaviour, ISaveLoadData
{
    [SerializedDictionary, SerializeField]
    private SerializedDictionary<InteractType, List<GameObject>> interactTable = new SerializedDictionary<InteractType, List<GameObject>>();

    private PriorityQueue<RespawnInfo, float> respawnObjectQueue = new PriorityQueue<RespawnInfo, float>();

    private void Awake()
    {
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
            target.owner.SetActive(true);
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

    private void OnApplicationQuit()
    {
        Save();
        SaveLoadManager.Save();
    }

    public void Save()
    {

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
            monsterSaveInfoList.Add(monsterSaveInfo);
        }

        SaveLoadManager.Data.monsterSaveInfoList = monsterSaveInfoList;
    }

    public void Load()
    {
        //    for (int i = 0; i < (int)InteractType.Monster; ++i)
        //    {
        //        var interactType = (InteractType)i;
        //        if (interactTable.ContainsKey((InteractType)i))
        //        {
        //            var gatherSaveInfoList = new List<GatherSaveInfo>();
        //            var list = interactTable[(InteractType)i];
        //            for (int j = 0; j < list.Count; ++j)
        //            {
        //                var gatherSaveInfo = new GatherSaveInfo();
        //                var respawnInfo = list[j].GetComponent<IRespawn>();
        //                gatherSaveInfo.remainingTime = respawnInfo.RemainingTime;
        //                gatherSaveInfo.respawnPosition = respawnInfo.RespawnPosition;
        //                gatherSaveInfo.isRespawn = respawnInfo.IsRespawn;
        //                gatherSaveInfo.position = list[j].transform.position;
        //                gatherSaveInfoList.Add(gatherSaveInfo);
        //            }

        //            SaveLoadManager.Data.gatherSaveInfoTable.Add(interactType, gatherSaveInfoList);
        //        }
        //    }

        //    var monsterSaveInfoList = new List<MonsterSaveInfo>();
        //    var monsterList = interactTable[InteractType.Monster];
        //    for (int i = 0; i < monsterList.Count; ++i)
        //    {
        //        var monsterSaveInfo = new MonsterSaveInfo();
        //        var respawnInfo = monsterList[i].GetComponent<IRespawn>();
        //        monsterSaveInfo.remainingTime = respawnInfo.RemainingTime;
        //        monsterSaveInfo.respawnPosition = respawnInfo.RespawnPosition;
        //        monsterSaveInfo.isRespawn = respawnInfo.IsRespawn;
        //        monsterSaveInfo.position = monsterList[i].transform.position;
        //        monsterSaveInfoList.Add(monsterSaveInfo);
        //    }

        //    SaveLoadManager.Data.monsterSaveInfoList = monsterSaveInfoList;
        //
    }
}
