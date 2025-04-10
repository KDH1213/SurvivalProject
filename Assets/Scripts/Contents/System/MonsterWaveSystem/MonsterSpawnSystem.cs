using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterSpawnSystem : MonoBehaviour, ISaveLoadData
{
    //[SerializeField]
    //private MonsterManager monsterManager;

    [SerializeField]
    private List<MonsterSpawner> monsterSpawnerList;
    private List<MonsterSpawner> monsterActiveSpawnerList = new List<MonsterSpawner>();
    // private List<WaveData> waveDataList = new List<WaveData>();

    [SerializeField]
    private List<MonsterWaveData> monsterWaveDatas;

    [SerializeField]
    private GameObject wavePanel;
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private float nextWaveTime;

    private float waveTime;

    private int activeSpawnerCount;
    private int currentWaveLevel = 0;
    public int CurrentWaveLevel { get  { return currentWaveLevel; } }

    private bool isActive = false;

    private Coroutine coSpawn;

    private readonly string timerFormat = "남은 시간 : {0:F}";

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
        waveTime = Time.time + monsterWaveDatas[currentWaveLevel].StartSpawnTime;
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

        foreach (var spawner in monsterActiveSpawnerList)
        {
            spawner.SetMonsterWaveData(monsterWaveDatas[currentWaveLevel]);
            spawner.StartSpawn();
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

        for (int i = 0; i < monsterSpawnerList.Count; ++i)
        {
            monsterWaveSaveInfo.activeSpawners[i] = monsterSpawnerList[i].gameObject.activeSelf;
        }
    }

    public void Load()
    {
        var monsterWaveSaveInfo = SaveLoadManager.Data.monsterWaveSaveInfo;

        for (int i = 0; i < monsterWaveSaveInfo.activeSpawners.Length; ++i)
        {
            if (!monsterWaveSaveInfo.activeSpawners[i])
            {
                monsterSpawnerList[i].gameObject.SetActive(false);
                monsterSpawnerList[i].OnDestroySpawnerEvent();
            }
        }

        waveTime =  monsterWaveSaveInfo.waveTime + Time.time;
        currentWaveLevel = monsterWaveSaveInfo.waveLevel;
    }
}
