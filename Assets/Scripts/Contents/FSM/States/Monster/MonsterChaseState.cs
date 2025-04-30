using UnityEngine;

public class MonsterChaseState : MonsterBaseState
{
    private bool useReturn;

    private Vector3 closestPoint;
    private Collider targetCollider;

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

        if(MonsterFSM.Target != null)
        {
            targetCollider = MonsterFSM.Target.GetComponentInChildren<Collider>();
            closestPoint = targetCollider.ClosestPoint(transform.position);
            Agent.SetDestination(closestPoint);
        }
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
            closestPoint = targetCollider.ClosestPoint(transform.position);
            Agent.SetDestination(closestPoint);
        }
    }

    public override void Exit()
    {
        useReturn = false;
        MonsterFSM.Agent.isStopped = true;
    }

    private void Chase()
    {
        closestPoint = targetCollider.ClosestPoint(transform.position);
        float distance = (closestPoint - transform.position).ConvertVector2().magnitude;

        if (distance <= MonsterFSM.MonsterData.AttackRadius)
        {
            MonsterFSM.ChangeState(MonsterStateType.Attack);
        }
        
        if (distance > MonsterFSM.MonsterData.ChaseRadius)
        {
            MonsterFSM.Target = null;
            //MonsterFSM.ChangeState(MonsterStateType.Idle);
            useReturn = true;

            return;
        }
    }
}
