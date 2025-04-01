using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterWaveData", menuName = "System/MonsterSpawnData/MonsterSpawnInfo", order = 3)]
[System.Serializable]
public class MonsterWaveData : ScriptableObject
{
    [SerializeField]
    private List<GameObject> monsterPrefabList = new List<GameObject>();
    public List<GameObject> MonsterPrefabList { get { return monsterPrefabList; } }

    [SerializeField]
    private int spawnCount;
    public int SpawnCount { get { return spawnCount; } }

    [SerializeField]
    private float spawnTime;
    public float SpawnTime { get { return spawnTime; } }

    [SerializeField]
    private bool isRepeat;
    public bool IsRepeat { get { return isRepeat; } }


    public GameObject GetRandomMonster()
    {
        return monsterPrefabList[Random.Range(0, monsterPrefabList.Count)];
    }
}
