using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public Dictionary<ObjectPoolType, Component> ObjectPoolTable { get; private set; } = new Dictionary<ObjectPoolType, Component>();

    public void AddObjectPool(ObjectPoolType objectPoolType, Component monsterObjectPool)
    {
        if(!ObjectPoolTable.ContainsKey(objectPoolType))
        {
            ObjectPoolTable.Add(objectPoolType, monsterObjectPool);
        }
    }

    public MonsterObjectPool MonsterObjectPool => ObjectPoolTable[ObjectPoolType.Monster] as MonsterObjectPool;
}
