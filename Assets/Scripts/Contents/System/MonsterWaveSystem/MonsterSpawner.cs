using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MonsterSpawner : MonoBehaviour, IMonsterSpawner
{
    [SerializeField]
    private MonsterWaveData[] waveDatas;

    [SerializeField]
    protected MonsterSpawnSystem monsterSpawnSystem;
    [SerializeField]
    protected MonsterObjectPool monsterObjectPool;

    [SerializeField]
    protected UIHpBarObjectPool hpBarObjectPool;

    protected float spawnTime;
    protected float currentSpawnTime = 0f;
    protected int currentSpawnCount = 0;
    protected int currentSpawnIndex = 0;

    protected bool isActive = false;
    protected bool isRepeat = true;

    protected MonsterWaveData waveData;
    // private MonsterData monsterData;
    protected Coroutine spawnCoroutine;

    public UnityAction<MonsterSpawner> onDestroySpawnerEvent;
    //public virtual void SetMonsterWaveData(WaveData monsterSpawnInfo)
    //{
    //    waveData = monsterSpawnInfo;
    //    spawnTime = waveData.SpawnInterval;
    //    monsterData = DataTableManager.MonsterDataTable.Get(waveData.MonsterID);
    //    monsterObjectPool.SetMonsterData(monsterData.PrefabObject, monsterData.Id);
    //}
    public virtual void SetWaveIndex(int index)
    {
        currentSpawnIndex = index;

        currentSpawnIndex = Mathf.Clamp(currentSpawnIndex, 0, waveDatas.Length);

        waveData = waveDatas[currentSpawnIndex];
        spawnTime = waveData.SpawnTime;
    }
    public virtual void SetMonsterWaveData(MonsterWaveData monsterWaveData)
    {
        waveData = monsterWaveData;
        spawnTime = waveData.SpawnTime;
    }

    public virtual void StartSpawn()
    {
        spawnCoroutine = StartCoroutine(StartSpawnCoroutine());

        isActive = true;
        enabled = true;
    }

    public virtual void StartSpawn(bool isRepeat)
    {
        if(waveData.SpawnCount == 0)
        {
            monsterSpawnSystem.EndSpawn();
            return;
        }

        if(isRepeat)
            spawnCoroutine = StartCoroutine(StartSpawnRepeatCoroutine());
        else
            spawnCoroutine = StartCoroutine(StartSpawnCoroutine());

        isActive = true;
        enabled = true;
    }

    public virtual void RestartSpawn(int count, float time)
    {
        currentSpawnTime = time;
        currentSpawnCount = count;

        spawnCoroutine = StartCoroutine(RestartSpawnCoroutine());
        isActive = true;
        enabled = true;
    }
    public virtual void StopSpawn()
    {
        currentSpawnCount = 0;
        isActive = false;
        //enabled = false;
        if(spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator StartSpawnCoroutine()
    {
        currentSpawnCount = 0; 
        ISpawn();

        while (currentSpawnCount < waveData.SpawnCount)
        {
            currentSpawnTime += Time.deltaTime;

            if(currentSpawnTime >= spawnTime)
            {
                ISpawn();
                currentSpawnTime -= spawnTime;
            }

            yield return null;
        }

        monsterSpawnSystem.EndSpawn();
    }

    private IEnumerator RestartSpawnCoroutine()
    {
        while (currentSpawnCount < waveData.SpawnCount)
        {
            currentSpawnTime += Time.deltaTime;

            if (currentSpawnTime >= spawnTime)
            {
                ISpawn();
                currentSpawnTime -= spawnTime;
            }

            yield return null;
        }

        monsterSpawnSystem.EndSpawn();
    }

    private IEnumerator StartSpawnRepeatCoroutine()
    {
        ISpawn();

        while (true)
        {
            currentSpawnTime += Time.deltaTime;

            if (currentSpawnTime >= spawnTime)
            {
                ISpawn();
                currentSpawnTime -= spawnTime;

                //if (monsterSpawnInfo.SpawnCount == currentSpawnCount)
                //{
                //    StopSpawn();
                //}
            }

            yield return null;
        }
    }

    public virtual void ISpawn()
    {
        monsterObjectPool.SetMonsterData(waveData.GetRandomMonster());
        var monsterController = monsterObjectPool.GetMonster();

        if(NavMesh.SamplePosition(transform.position, out var hitPoint, 100f, NavMesh.AllAreas))
        {
            monsterController.transform.position = hitPoint.position;
        }
        else
        {
            monsterController.transform.position = transform.position;
        }
        monsterController.ChangeState(MonsterStateType.Idle);
        monsterSpawnSystem.createMonsterTable.Add(monsterController);

        ++currentSpawnCount;
    }

    public void OnDestroySpawnerEvent()
    {
        StopSpawn();
        onDestroySpawnerEvent?.Invoke(this);
    }

    public void Save()
    {
        if(!gameObject.activeSelf)
        {
            SaveLoadManager.Data.spawnerSaveInfoList.Add(new SpawnerSaveInfo(0f, 0, true));
        }
        else
        {
            SaveLoadManager.Data.spawnerSaveInfoList.Add(new SpawnerSaveInfo(currentSpawnTime, currentSpawnCount, isActive));
        }

    }
}
