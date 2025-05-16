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

    private void Start()
    {
        var stats = GetComponent<StructureStats>();
        if (ObjectPoolManager.Instance.ObjectPoolTable.TryGetValue(ObjectPoolType.HpBar, out var component))
        {
            
            var hpBarObjectPool = component.GetComponent<UIHpBarObjectPool>();
            var hpBar = hpBarObjectPool.GetHpBar();
            hpBar.GetComponent<UITargetFollower>().SetTarget(transform, Vector3.down);
            hpBar.SetTarget(stats);
            stats.CurrentStatTable[StatType.HP].SetValue(stats.CurrentStatTable[StatType.HP].MaxValue);

            stats.deathEvent.AddListener(() => { hpBar.gameObject.SetActive(false); });
        }
        stats.OnChangeHp();
    }
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

        monsterController.transform.rotation = Quaternion.identity;
        // TODO :: 더미 코드
        var moveState = monsterController.StateTable[MonsterStateType.Move] as MonsterMoveState;
        moveState.SetMovePosition(targetPoint);
        monsterController.StateTable[MonsterStateType.Idle].enterStateEvent.AddListener(() => monsterController.ChangeState(MonsterStateType.Move));
        monsterController.ChangeState(MonsterStateType.Move);
        ++currentSpawnCount;

        monsterSpawnSystem.createMonsterTable.Add(monsterController);
    }
}
