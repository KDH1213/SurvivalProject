using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterPointSpawner : MonsterSpawner
{
    [SerializeField]
    private Transform[] createPoints;

    [SerializeField]
    private Vector3 targetPoint;
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

        // TODO :: 더미 코드
        var moveState = monsterController.StateTable[MonsterStateType.Move] as MonsterMoveState;
        moveState.SetMovePosition(targetPoint);
        monsterController.StateTable[MonsterStateType.Idle].enterStateEvent.AddListener(() => monsterController.ChangeState(MonsterStateType.Move));

        monsterController.ChangeState(MonsterStateType.Move);
        ++currentSpawnCount;
    }
}
