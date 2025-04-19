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
        MonsterFSM.TargetTransform = null;

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
        int index = Physics.OverlapSphereNonAlloc(MonsterFSM.transform.position, MonsterFSM.MonsterData.aggroRange, findTargets, MonsterFSM.Weapon.WeaponLayerMask);
        Transform target = null;

        float distance = float.MaxValue;
        Vector3 position = MonsterFSM.transform.position;
        Vector3 positionCheck;

        for (int i = 0; i < index; ++i)
        {
            target = findTargets[i].gameObject.transform;
            positionCheck = target.position - position;
            positionCheck.y = 0f;
            if (distance > positionCheck.sqrMagnitude)
            {
                distance = positionCheck.sqrMagnitude;
            }
        }

        if(target != null)
        {
            MonsterFSM.Target = target.gameObject;
            monsterFSM.TargetTransform = target;
            MonsterFSM.ChangeState(MonsterStateType.Chase);
        }
    }

}
