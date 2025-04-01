using System.Collections;
using System.Collections.Generic;
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
    private float nextWaveTime;

    private float waveTime;

    private int activeSpawnerCount;
    private int currentWaveLevel = 0;
    public int CurrentWaveLevel { get  { return currentWaveLevel; } }

    private int bossMonsterCount = 0;
    private bool isActive = false;

    private Coroutine coSpawn;

    private void Start()
    {
        StartSpawn();
    }

    private void Update()
    {
        if (!isActive)
        {
            if(waveTime > Time.time)
            {
                StartSpawn();
            }
        }
    }


    public void StartSpawn()
    {
        isActive = true;
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
            waveTime = Time.time + nextWaveTime;
            ++currentWaveLevel;
        }
    }
}
