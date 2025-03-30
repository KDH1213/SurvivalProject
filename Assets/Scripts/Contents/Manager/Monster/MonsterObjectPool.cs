using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterObjectPool : MonoBehaviour
{
    public int CreateMonsterID { get; set; }
    public Dictionary<int, IObjectPool<MonsterFSM>> monsterPoolTable { get; private set; } = new Dictionary<int, IObjectPool<MonsterFSM>>();
    private GameObject monsterObject;

    private void Awake()
    {
        ObjectPoolManager.Instance.AddObjectPool(ObjectPoolType.Monster, this);
    }

    public void SetMonsterData(GameObject prefabObject, int id)
    {
        monsterObject = prefabObject;
        CreateMonsterID = id;

        if (!monsterPoolTable.ContainsKey(CreateMonsterID))
        {
            monsterPoolTable.Add(CreateMonsterID, new ObjectPool<MonsterFSM>(OnCreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, true, 100));
        }
    }

    private MonsterFSM OnCreateMonster()
    {
        Instantiate(monsterObject).TryGetComponent(out MonsterFSM monster);
        monster.SetPool(monsterPoolTable[CreateMonsterID]);
        return monster;
    }

    private void OnGetMonster(MonsterFSM monster)
    {
        monster.gameObject.SetActive(true);
    }

    private void OnReleaseMonster(MonsterFSM monster)
    {
        monster.gameObject.SetActive(false);
    }

    private void OnDestroyMonster(MonsterFSM monster)
    {
        Destroy(monster.gameObject);
    }

    public MonsterFSM GetMonster()
    {
        return monsterPoolTable[CreateMonsterID].Get();
    }
}
