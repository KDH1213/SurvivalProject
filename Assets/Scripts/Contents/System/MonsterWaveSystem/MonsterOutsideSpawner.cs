using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterOutsideSpawner : MonsterSpawner
{
    [SerializeField]
    private float randomRadin;

    [SerializeField]
    private Vector3 targetPoint;
    public override void ISpawn()
    {
        monsterObjectPool.SetMonsterData(waveData.GetRandomMonster());
        var monsterController = monsterObjectPool.GetMonster();

        float angle = Random.Range(0f, 360f);

        Vector3 createPoint = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad)) * randomRadin;

        if (NavMesh.SamplePosition(createPoint, out var hitPoint, 100f, NavMesh.AllAreas))
        {
            monsterController.transform.position = hitPoint.position;
        }
        else
        {
            monsterController.transform.position = createPoint;
        }    
        
        // TODO :: 더미 코드
        var moveState = monsterController.StateTable[MonsterStateType.Move] as MonsterMoveState;
        moveState.SetMovePosition(targetPoint);
        monsterController.StateTable[MonsterStateType.Idle].enterStateEvent.AddListener(() => monsterController.ChangeState(MonsterStateType.Move));

        monsterController.ChangeState(MonsterStateType.Move);

        ++currentSpawnCount;
    }

}
