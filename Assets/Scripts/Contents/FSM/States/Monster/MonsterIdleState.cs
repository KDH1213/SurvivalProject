using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : MonsterBaseState
{
    private Collider[] findTargets = new Collider[2];
    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Idle;

    }

    public override void Enter()
    {
        MonsterFSM.Target = null;

        Agent.isStopped = true;
        Agent.speed = 0f;
        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, Agent.speed);
        enterStateEvent?.Invoke();
    }

    public override void ExecuteUpdate()
    {
        if (MonsterFSM.Target == null)
        {
            FindTarget();
        }
    }

    public override void Exit()
    {
        Agent.isStopped = false;
        Agent.speed = MonsterStats.Speed;
    }

    private void FindTarget()
    {
        int index = Physics.OverlapSphereNonAlloc(MonsterFSM.transform.position, MonsterFSM.MonsterData.ChaseRadius, findTargets, MonsterFSM.Weapon.WeaponLayerMask);
        Transform target = null;

        float distance = MonsterFSM.MonsterData.ChaseRadius * MonsterFSM.MonsterData.ChaseRadius;
        Vector3 position = MonsterFSM.transform.position;
        Vector3 positionCheck;

        for (int i = 0; i < index; ++i)
        {
            positionCheck = findTargets[i].gameObject.transform.position - position;
            positionCheck.y = 0f;
            if (distance > positionCheck.sqrMagnitude)
            {
                distance = positionCheck.sqrMagnitude;
                target = findTargets[i].gameObject.transform;
            }
        }

        if(target != null)
        {
            MonsterFSM.Target = target.gameObject;
            MonsterFSM.ChangeState(MonsterStateType.Chase);
        }
    }

}
