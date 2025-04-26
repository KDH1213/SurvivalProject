using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitState : MonsterBaseState
{

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Hit;

    }

    public override void Enter()
    {
        MonsterFSM.Agent.isStopped = true;
        MonsterFSM.Agent.velocity = Vector3.zero;
        MonsterFSM.Agent.speed = 0f;
        MonsterFSM.Animator.SetTrigger(MonsterAnimationHashCode.hashHit);

        int random = Random.Range(0, 1);
        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashHitFromX, (float)random);
    }

    public override void ExecuteUpdate()
    {
    }

    public override void ExecuteFixedUpdate()
    {
    }

    public override void Exit()
    {
    }

}
