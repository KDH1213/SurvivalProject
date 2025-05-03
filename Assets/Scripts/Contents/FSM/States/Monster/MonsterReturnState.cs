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
        Agent.isStopped = false;
        Agent.speed = MonsterStats.Speed;
        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, Agent.speed);
    }

    public override void ExecuteUpdate()
    {
        if(HasReachedDestination())
        {
            MonsterFSM.ChangeState(MonsterStateType.Idle);
            return;
        }

        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, Agent.speed);
    }

    public override void ExecuteFixedUpdate()
    {
        Agent.SetDestination(MonsterFSM.FirstPosition);
    }

    public override void Exit()
    {
        Agent.isStopped = true;
    }

    private bool HasReachedDestination()
    {
        return !Agent.pathPending &&
               Agent.remainingDistance <= Agent.stoppingDistance &&
               !Agent.hasPath;
    }
}
