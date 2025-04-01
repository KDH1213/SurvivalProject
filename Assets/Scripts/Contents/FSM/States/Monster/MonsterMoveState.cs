using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveState : MonsterBaseState
{
    public Vector3 movePosition;

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Move;

    }

    public override void Enter()
    {
        if (MonsterFSM.Animator != null)
        {
            MonsterFSM.Animator.SetBool(AnimationHashCode.hashMove, true);
        }

        MonsterFSM.Agent.isStopped = false;
        MonsterFSM.Agent.speed = MonsterStats.Speed;
        MonsterFSM.Agent.SetDestination(movePosition);
    }

    public override void ExecuteUpdate()
    {
        if (MonsterFSM.Target == null)
        {
            FindTarget();
        }

        ChangeChaseState();

        if (Vector3.Distance(movePosition, transform.position) < 5f)
        {
            MonsterFSM.StateTable[MonsterStateType.Idle].enterStateEvent.RemoveAllListeners();
            MonsterFSM.ChangeState(MonsterStateType.Idle);
            return;
        }

        if (MonsterFSM.IsChase)
        {
            MonsterFSM.ChangeState(MonsterStateType.Chase);
        }
    }

    public override void Exit()
    {
        if (MonsterFSM.Animator != null)
        {
            MonsterFSM.Animator.SetBool(AnimationHashCode.hashMove, false);
        }
    }

    private void FindTarget()
    {
        if (MonsterFSM.Target != null)
        {
            return;
        }

        int index = Physics.OverlapSphereNonAlloc(MonsterFSM.transform.position, MonsterFSM.MonsterData.aggroRange, MonsterFSM.Weapon.attackTargets, MonsterFSM.Weapon.WeaponLayerMask);

        for (int i = 0; i < index; i++)

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

    public void SetMovePosition(Vector3 movePoint)
    {
        movePosition = movePoint;
    }
}
