using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MonsterSpawnSystem : MonoBehaviour
{
    //[SerializeField]
    //private MonsterManager monsterManager;
    [SerializeField] 
    private List<MonsterSpawner> monsterSpawnerList;
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

    private int bossMonsterCount = 0;
    private bool isActive = false;

    private Coroutine coSpawn;

    private readonly string timerFormat = "남은 시간 : {0:F}";

    private void Awake()
    {
        foreach (var monsterSpawner in monsterSpawnerList)
        {
            monsterSpawner.onDestroySpawnerEvent += OnDestroySpawner;
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

        foreach (var spawner in monsterSpawnerList)
        {
            spawner.SetMonsterWaveData(monsterWaveDatas[currentWaveLevel]);
            spawner.StartSpawn();
            ++activeSpawnerCount; 
        }
    }

    public void StopSpawn()
    {
        foreach (var spawner in monsterSpawnerList)
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
        monsterSpawnerList.Remove(monsterSpawner);

        if(monsterSpawnerList.Count == 0)
        {
            isActive = false;
            enabled = false;
            wavePanel.SetActive(false);
        }
    }
}
