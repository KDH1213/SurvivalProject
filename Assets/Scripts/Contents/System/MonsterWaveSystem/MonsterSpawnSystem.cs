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

    private int activeSpawnerCount;
    private int currentWaveLevel = 0;
    public int CurrentWaveLevel { get  { return currentWaveLevel; } }

    private int bossMonsterCount = 0;
    private bool isActive = false;

    private Coroutine coSpawn;
    // private UniTask uniTaskSpawn;

    private void Awake()
    {
        // waveDataList = DataTableManager.WaveDataTable.List;
    }

    private void Start()
    {
        //foreach (var spawner in monsterSpawnerList)
        //{
        //    spawner.spawnEvent.AddListener(monsterManager.OnAddMonster);
        //    spawner.SetMonsterDeathAction(monsterManager.OnDeathMonster);
        //    spawner.SetMonsterDestroyAction(monsterManager.OnDestroyMonster);
        //}

        // coSpawn = StartCoroutine(CoStartSpawn());
        StartSpawn();
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
        }
    }

    //private IEnumerator CoStartSpawn()
    //{
    //    int maxWave = waveDataList.Count;
    //    float currentTime = 0f;

    //    while (currentWaveLevel < maxWave)
    //    {
    //        currentTime = waveDataList[currentWaveLevel].SpawnWaitTime;
    //        while (currentTime > 0f)
    //        {
    //            yield return new WaitForEndOfFrame();
    //            currentTime -= Time.deltaTime;
    //            changeWaveTimeEvent?.Invoke(currentTime);
    //        }

    //        StartSpawn();

    //        currentTime += waveDataList[currentWaveLevel].SpawnTime;
    //        if (currentTime < waveDataList[currentWaveLevel].SpawnInterval * waveDataList[currentWaveLevel].SpawnCount)
    //            currentTime = waveDataList[currentWaveLevel].SpawnInterval * waveDataList[currentWaveLevel].SpawnCount;

    //        ++currentWaveLevel;

    //        while (currentTime > 0f)
    //        {
    //            yield return new WaitForEndOfFrame();
    //            currentTime -= Time.deltaTime;
    //            changeWaveTimeEvent?.Invoke(currentTime);

    //            if (isActive && monsterManager.CurrentMonsterCount == 0)
    //                break;

    //        }
    //    }
    //}

    //private async UniTask UniTaskStartSpawn()
    //{
    //    int maxWave = waveDataList.Count;
    //    float currentTime = 0f;
    //    bool isGameOver = false;
    //    while (currentWaveLevel < maxWave)
    //    {
    //        currentTime = waveDataList[currentWaveLevel].SpawnWaitTime;
    //        while (currentTime > 0f)
    //        {
    //            await UniTask.Yield(PlayerLoopTiming.Update);
    //            currentTime -= Time.deltaTime;
    //            changeWaveTimeEvent?.Invoke(currentTime);

    //        }

    //        if (bossMonsterCount != 0)
    //        {
    //            GameController.GameOver();
    //            isGameOver = true;
    //            break;
    //        }


    //        GameController.AddCurrencyType(waveDataList[currentWaveLevel].CurrencyType, waveDataList[currentWaveLevel].WaveStartCurrencyValue);
    //        StartSpawn();

    //        currentTime += waveDataList[currentWaveLevel++].SpawnTime;
    //        while (currentTime > 0f)
    //        {
    //            await UniTask.Yield(PlayerLoopTiming.Update);
    //            currentTime -= Time.deltaTime;
    //            changeWaveTimeEvent?.Invoke(currentTime);
    //        }
    //    }

    //    if (!isGameOver)
    //        GameController.GameClear();
    //}
}
