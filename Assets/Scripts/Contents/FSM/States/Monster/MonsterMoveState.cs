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
        Agent.isStopped = false;
        Agent.speed = MonsterStats.Speed;
        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, Agent.speed);
        Agent.SetDestination(movePosition);
    }

    public override void ExecuteUpdate()
    {
        if(!FindTarget())
        {
            var position = movePosition - transform.position;
            position.y = 0f;

            if (position.sqrMagnitude < 1f)
            {
                MonsterFSM.StateTable[MonsterStateType.Idle].enterStateEvent.RemoveAllListeners();
                MonsterFSM.ChangeState(MonsterStateType.Idle);
            }
        }
    }

    public override void ExecuteFixedUpdate()
    {
        Agent.SetDestination(movePosition);
    }

    public override void Exit()
    {
    }


    private bool FindTarget()
    {
        int index = Physics.OverlapSphereNonAlloc(MonsterFSM.transform.position, MonsterFSM.MonsterData.ChaseRadius, MonsterFSM.Weapon.FindTargetColliders, MonsterFSM.Weapon.WeaponLayerMask);
        Transform target = null;

        float distance = float.MaxValue;
        Vector3 position = MonsterFSM.transform.position;
        Vector3 positionCheck;

        for (int i = 0; i < index; ++i)
        {
            target = MonsterFSM.Weapon.FindTargetColliders[i].gameObject.transform;
            positionCheck = target.position - position;
            positionCheck.y = 0f;
            if (distance > positionCheck.sqrMagnitude)
            {
                distance = positionCheck.sqrMagnitude;
            }
        }

        if (target != null)
        {
            MonsterFSM.Target = target.gameObject;
            MonsterFSM.ChangeState(MonsterStateType.Chase);
            return true;
        }

        return false;
    }

    public void SetMovePosition(Vector3 movePoint)
    {
        movePosition = movePoint;
    }
}
