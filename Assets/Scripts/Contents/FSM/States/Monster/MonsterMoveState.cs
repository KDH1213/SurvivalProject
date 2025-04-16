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
        MonsterFSM.Agent.isStopped = false;
        MonsterFSM.Agent.speed = MonsterStats.Speed;
        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, MonsterFSM.Agent.speed);
        MonsterFSM.Agent.SetDestination(movePosition);
    }

    public override void ExecuteUpdate()
    {
        if (MonsterFSM.Target == null && !FindTarget())
        {
           
        }

        if(MonsterFSM.Target != null)
        {
            MonsterFSM.TargetDistance = Vector3.Distance(transform.position, MonsterFSM.TargetTransform.position);

            // 플레이어를 발견하면 추적 시작
            if (MonsterFSM.TargetDistance < MonsterFSM.MonsterData.aggroRange)
            {
                MonsterFSM.ChangeState(MonsterStateType.Chase);
                return;
            }
        }

        if (Vector3.Distance(movePosition, transform.position) < 1f)
        {
            MonsterFSM.StateTable[MonsterStateType.Idle].enterStateEvent.RemoveAllListeners();
            MonsterFSM.ChangeState(MonsterStateType.Idle);
            return;
        }

    }

    public override void Exit()
    {
    }

    private bool FindTarget()
    {
        int index = Physics.OverlapSphereNonAlloc(MonsterFSM.transform.position, MonsterFSM.MonsterData.aggroRange, MonsterFSM.Weapon.attackTargets, MonsterFSM.Weapon.WeaponLayerMask);

        for (int i = 0; i < index; ++i)
        {
            MonsterFSM.Target = MonsterFSM.Weapon.attackTargets[i].gameObject;
            MonsterFSM.TargetTransform = MonsterFSM.Target.transform;
            return true;
        }

        return false;
    }

    public void SetMovePosition(Vector3 movePoint)
    {
        movePosition = movePoint;
    }
}
