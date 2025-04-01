using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterPointSpawner : MonsterSpawner
{
    [SerializeField]
    private Transform[] createPoints;
    public override void ISpawn()
    {
        monsterObjectPool.SetMonsterData(waveData.GetRandomMonster());
        var monsterController = monsterObjectPool.GetMonster();

        int randomPoint = Random.Range(0, createPoints.Length);

        if (NavMesh.SamplePosition(createPoints[randomPoint].position, out var hitPoint, 100f, NavMesh.AllAreas))
        {
            monsterController.transform.position = hitPoint.position;
        }
        else
        {
            monsterController.transform.position = createPoints[randomPoint].position;
        }
        monsterController.ChangeState(MonsterStateType.Idle);

        ++currentSpawnCount;
    }
}
