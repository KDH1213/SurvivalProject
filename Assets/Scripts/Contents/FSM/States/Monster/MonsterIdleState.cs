using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : MonsterBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Idle;

    }

    public override void Enter()
    {
        Debug.Log("Monster: Idle State!!");

        enterStateEvent?.Invoke();
        MonsterFSM.Agent.isStopped = true;
    }

    public override void ExecuteUpdate()
    {
        if (MonsterFSM.IsChase)
        {
            MonsterFSM.ChangeState(MonsterStateType.Chase);
            return;
        }

        if (MonsterFSM.Target == null)
        {
            FindTarget();
        }

        ChangeChaseState();
    }

    public override void Exit()
    {

    }

    private void FindTarget()
    {
        if (MonsterFSM.Target != null)
        {
            return;
        }

        int index = Physics.OverlapSphereNonAlloc(MonsterFSM.transform.position, MonsterFSM.MonsterData.aggroRange, MonsterFSM.Weapon.attackTargets, MonsterFSM.Weapon.WeaponLayerMask);

        for(int i = 0; i < index; i++)
        {
            MonsterFSM.Target = MonsterFSM.Weapon.attackTargets[0].gameObject;
            MonsterFSM.TargetTransform = MonsterFSM.Target.transform;
            ChangeChaseState();
            return;
        }
    }

    private void ChangeChaseState()
    {
        if (MonsterFSM.Target == null)
        {
            return;
        }

        MonsterFSM.TargetDistance = Vector3.Distance(transform.position, MonsterFSM.TargetTransform.position);

        // 플레이어를 발견하면 추적 시작
        if (!MonsterFSM.IsChase && MonsterFSM.TargetDistance < MonsterFSM.MonsterData.aggroRange)
        {
            MonsterFSM.SetIsChase(true);
            MonsterFSM.SetIsPlayerRange(true);
        }
    }
}
