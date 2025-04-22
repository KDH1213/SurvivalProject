using UnityEngine;

public class MonsterChaseState : MonsterBaseState
{
    private bool useReturn;

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Chase;
    }

    public override void Enter()
    {
        useReturn = false;

        Agent.isStopped = false;
        Agent.speed = MonsterStats.Speed;
        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, MonsterFSM.Agent.speed);
    }

    public override void ExecuteUpdate()
    {
        if (useReturn)
        {
            MonsterFSM.ChangeState(MonsterStateType.Return);
            return;
        }

        if (MonsterFSM.Target != null)
        {
            MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, MonsterFSM.Agent.speed);
            Chase();
        }
        else
        {
            MonsterFSM.ChangeState(MonsterStateType.Idle);
        }

        
    }

    public override void ExecuteFixedUpdate()
    {
        if (MonsterFSM.Target != null)
        {
            Agent.SetDestination(MonsterFSM.TargetTransform.position);
        }
    }

    public override void Exit()
    {
        useReturn = false;
        MonsterFSM.Agent.isStopped = true;
    }

    private void Chase()
    {
        MonsterFSM.TargetDistance = Vector3.Distance(transform.position, MonsterFSM.TargetTransform.position);

        if (MonsterFSM.TargetDistance <= MonsterFSM.Weapon.Range)
        {
            MonsterFSM.ChangeState(MonsterStateType.Attack);
        }
        
        if (MonsterFSM.TargetDistance > MonsterFSM.MonsterData.ChaseRadius)
        {
            MonsterFSM.Target = null;
            //MonsterFSM.ChangeState(MonsterStateType.Idle);
            useReturn = true;

            return;
        }
    }
}
