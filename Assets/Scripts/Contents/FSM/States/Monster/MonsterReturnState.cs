using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterReturnState : MonsterBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Return;
    }

    public override void Enter()
    {
        MonsterFSM.Agent.isStopped = false;
        MonsterFSM.Agent.speed = MonsterStats.Speed;
        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, MonsterFSM.Agent.speed);
    }

    public override void ExecuteUpdate()
    {
        if(HasReachedDestination())
        {
            MonsterFSM.ChangeState(MonsterStateType.Idle);
            return;
        }

        ReturnPosition();
    }

    public override void Exit()
    {
        MonsterFSM.Agent.isStopped = true;
    }

    private bool HasReachedDestination()
    {
        return !MonsterFSM.Agent.pathPending &&
               MonsterFSM.Agent.remainingDistance <= MonsterFSM.Agent.stoppingDistance &&
               !MonsterFSM.Agent.hasPath;
    }

    private void ReturnPosition()
    {
        if (MonsterFSM.Animator == null)
        {
            MonsterFSM.Animator.SetBool(AnimationHashCode.hashMove, true);
        }
        MonsterFSM.Agent.SetDestination(MonsterFSM.FirstPosition);
    }
}
