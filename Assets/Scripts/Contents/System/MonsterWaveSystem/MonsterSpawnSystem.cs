using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MonsterSpawnSystem : MonoBehaviour, ISaveLoadData
{
    //[SerializeField]
    //private MonsterManager monsterManager;

    [SerializeField]
    private List<MonsterSpawner> monsterSpawnerList;
    private List<MonsterSpawner> monsterActiveSpawnerList = new List<MonsterSpawner>();

    public HashSet<MonsterFSM> createMonsterTable {  get; private set; } = new HashSet<MonsterFSM>();
    // private List<WaveData> waveDataList = new List<WaveData>();

    [SerializeField]
    private StageManager stageManager;

    [SerializeField]
    private List<MonsterWaveData> monsterWaveDatas;

    [SerializeField]
    private GameObject wavePanel;
    [SerializeField]
    private TextMeshProUGUI timerText;

    public UnityEvent<bool> onWaveActiveEvent;

    [SerializeField]
    private float nextWaveTime;

    private float waveTime = 0f;

    private int activeSpawnerCount;
    private int currentWaveLevel = 0;
    public int CurrentWaveLevel { get  { return currentWaveLevel; } }

    private bool isActive = false;

    private Coroutine coSpawn;

    private readonly string timerFormat = "남은 시간 : {0:F}";
    private bool isSave = false;

    private void Awake()
    {
        for (int i = 0; i < monsterSpawnerList.Count; ++i)
        {
            monsterSpawnerList[i].onDestroySpawnerEvent += OnDestroySpawner;
            monsterActiveSpawnerList.Add(monsterSpawnerList[i]);
        }

        if (SaveLoadManager.Data != null)
        {
            Load();
        }

        var stageManager = GameObject.FindGameObjectWithTag("StageManager");
        if (stageManager != null)
        {
            stageManager.GetComponent<StageManager>().onSaveEvent += Save;
        }
    }

    private void Start()
    {
        //StartSpawn();
        isActive = false;
        wavePanel.SetActive(true);

        if(waveTime == 0f)
        {
            waveTime = Time.time + monsterWaveDatas[currentWaveLevel].StartSpawnTime;
        }
    }

    private void Update()
    {
        if (!isActive)
        {
            timerText.text = string.Format(timerFormat, waveTime - Time.time);
            if(waveTime < Time.time)
            {
                StartSpawn();
            }

            if(!isSave && waveTime - Time.time < 1f)
            {
                isSave = true;
                stageManager?.OnSave();
            }
        }
    }


    public void StartSpawn()
    {
        isActive = true;
        wavePanel.SetActive(false);
        activeSpawnerCount = 0;

        currentWaveLevel = Mathf.Clamp(currentWaveLevel, 0, monsterWaveDatas.Count - 1);
        if (currentWaveLevel < 0)
        {
            return;
        }

        onWaveActiveEvent?.Invoke(isActive);

        foreach (var spawner in monsterActiveSpawnerList)
        {
            // spawner.SetMonsterWaveData(monsterWaveDatas[currentWaveLevel]);
            spawner.SetWaveIndex(currentWaveLevel);
            spawner.StartSpawn();
            ++activeSpawnerCount; 
        }
    }

    public void RestartSpawn()
    {
        isActive = true;
        wavePanel.SetActive(false);
        activeSpawnerCount = 0;

        currentWaveLevel = Mathf.Clamp(currentWaveLevel, 0, monsterWaveDatas.Count - 1);
        if (currentWaveLevel < 0)
        {
            return;
        }

        var spawnerSaveInfoList = SaveLoadManager.Data.spawnerSaveInfoList;

        for (int i = 0; i < monsterSpawnerList.Count; i++)
        {
            if(spawnerSaveInfoList[i].isEnd)
            {
                continue;
            }

            monsterSpawnerList[i].RestartSpawn(spawnerSaveInfoList[i].spawnCount, spawnerSaveInfoList[i].currentSpawnTime);
            monsterSpawnerList[i].SetWaveIndex(currentWaveLevel);
            monsterSpawnerList[i].StartSpawn();
            ++activeSpawnerCount;
        }
    }

    public void StopSpawn()
    {
        foreach (var spawner in monsterActiveSpawnerList)
        {
            spawner.StopSpawn();
        }
    }

    public void EndSpawn()
    {
        --activeSpawnerCount;

        if(activeSpawnerCount == 0)
        {
            isActive = false;
            wavePanel.SetActive(!isActive);
            ++currentWaveLevel;
            currentWaveLevel = Mathf.Clamp(currentWaveLevel, 0, monsterWaveDatas.Count - 1);
            waveTime = Time.time + monsterWaveDatas[currentWaveLevel].StartSpawnTime;

            isSave = false;

            onWaveActiveEvent?.Invoke(isActive);
        }
    }

    public void OnDestroySpawner(MonsterSpawner monsterSpawner)
    {
        monsterActiveSpawnerList.Remove(monsterSpawner);

        if(monsterActiveSpawnerList.Count == 0)
        {
            isActive = false;
            enabled = false;
            wavePanel.SetActive(false);
        }
    }

    public void Save()
    {
        var monsterWaveSaveInfo = SaveLoadManager.Data.monsterWaveSaveInfo;

        monsterWaveSaveInfo.activeSpawners = new bool[monsterSpawnerList.Count];
        monsterWaveSaveInfo.waveTime = waveTime - Time.time;
        monsterWaveSaveInfo.waveLevel = currentWaveLevel;
        monsterWaveSaveInfo.isStartWave = isActive;

        for (int i = 0; i < monsterSpawnerList.Count; ++i)
        {
            monsterWaveSaveInfo.activeSpawners[i] = monsterSpawnerList[i].gameObject.activeSelf;
        }

        if(monsterWaveSaveInfo.isStartWave)
        {
            var spawnerSaveInfoList = SaveLoadManager.Data.spawnerSaveInfoList;
            spawnerSaveInfoList.Clear(); 
            for (int i = 0; i < monsterSpawnerList.Count; ++i)
            {
                monsterSpawnerList[i].Save();
            }
        }

        var waveMonsterSaveInfos = SaveLoadManager.Data.waveMonsterSaveInfos;
        waveMonsterSaveInfos.Clear();

        foreach (var monster in createMonsterTable)
        {
            if(!monster.gameObject.activeSelf)
            {
                continue;
            }

            var transform = monster.transform;
            waveMonsterSaveInfos.Add(new WaveMonsterSaveInfo(transform.position, transform.rotation, monster.GetComponent<MonsterStats>().Hp, monster.ID));
        } 

    }

    public void Load()
    {
        if(SaveLoadManager.Data.isRestart)
        {
            return;
        }

        var monsterWaveSaveInfo = SaveLoadManager.Data.monsterWaveSaveInfo;

        if(monsterWaveSaveInfo == null ||monsterWaveSaveInfo.activeSpawners == null)
        {
            return;
        }

        for (int i = 0; i < monsterWaveSaveInfo.activeSpawners.Length; ++i)
        {
            if (!monsterWaveSaveInfo.activeSpawners[i])
            {
                monsterSpawnerList[i].gameObject.SetActive(false);
                monsterSpawnerList[i].OnDestroySpawnerEvent();
            }
        }

        LoadMonster();

        if(monsterWaveSaveInfo.isStartWave)
        {
            RestartSpawn();
        }
        waveTime = monsterWaveSaveInfo.waveTime + Time.time;
        currentWaveLevel = monsterWaveSaveInfo.waveLevel;
    }

    private void LoadMonster()
    {
        var waveMonsterSaveInfos = SaveLoadManager.Data.waveMonsterSaveInfos;
        var monsterObjectPool = GetComponent<MonsterObjectPool>();
        createMonsterTable.Clear();

        foreach (var monster in waveMonsterSaveInfos)
        {
            var monsterPrefab = DataTableManager.MonsterTable.Get(monster.id).monsterPrefab;
            monsterObjectPool.SetMonsterData(monsterPrefab, monster.id);
            var createMonster = monsterObjectPool.GetMonster();
            createMonster.transform.position = monster.position;
            createMonster.transform.rotation = monster.rotation;
            createMonster.GetComponent<MonsterStats>().GetStat(StatType.HP).SetValue(monster.hp);

            var moveState = createMonster.StateTable[MonsterStateType.Move] as MonsterMoveState;
            // TODO :: 수정 예정
            moveState.SetMovePosition(Vector3.zero);
            createMonster.StateTable[MonsterStateType.Idle].enterStateEvent.AddListener(() => createMonster.ChangeState(MonsterStateType.Move));
            createMonster.ChangeState(MonsterStateType.Move);
            createMonsterTable.Add(createMonster);
        }
    }

}
