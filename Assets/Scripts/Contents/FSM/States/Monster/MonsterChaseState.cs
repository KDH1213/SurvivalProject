using Unity.VisualScripting;
using UnityEngine;

public class MonsterChaseState : MonsterBaseState
{
    private bool useReturn;

    private Vector3 closestPoint;
    private Collider targetCollider;

    private int target = 0;

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
        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, Agent.velocity.sqrMagnitude);

        if(MonsterFSM.Target != null)
        {
            if(MonsterFSM.Target.CompareTag(Tags.Player))
            {
                target = 0;
                Agent.SetDestination(MonsterFSM.Target.transform.position);
            }
            else
            {
                target = 1;

                targetCollider = MonsterFSM.Target.GetComponentInChildren<Collider>();
                closestPoint = targetCollider.ClosestPoint(transform.position);
                Agent.SetDestination(closestPoint);
            }
        }
    }

    public override void ExecuteUpdate()
    {
        if (useReturn)
        {
            MonsterFSM.ChangeState(MonsterStateType.Return);
            return;
        }
        MonsterFSM.Animator.SetFloat(MonsterAnimationHashCode.hashMove, Agent.velocity.sqrMagnitude);

        if (MonsterFSM.Target != null)
        {
            if (target == 0)
            {
                OnChasePlayer();
            }
            else
            {
                OnChaseBuilding();
            }
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
            if(target == 0)
            {
                Agent.SetDestination(MonsterFSM.Target.transform.position);
            }
            else
            {
                closestPoint = targetCollider.ClosestPoint(transform.position);
                Agent.SetDestination(closestPoint);
            }            
        }
    }

    public override void Exit()
    {
        useReturn = false;
        Agent.isStopped = true;
    }

    private void OnChasePlayer()
    {
        var targetDistance = (MonsterFSM.Target.transform.position - MonsterFSM.transform.position).ConvertVector2();
        var distance = targetDistance.sqrMagnitude;

        if (distance < MonsterFSM.MonsterData.AttackRadius * MonsterFSM.MonsterData.AttackRadius)
        {
            if(MonsterFSM.CanAttack)
            {
                MonsterFSM.ChangeState(MonsterStateType.Attack);
            }
        }
        else if (distance > MonsterFSM.MonsterData.ChaseRadius * MonsterFSM.MonsterData.ChaseRadius)
        {
            MonsterFSM.Target = null;
            useReturn = true;
            MonsterFSM.ChangeState(MonsterStateType.Idle);
        }
    }

    private void OnChaseBuilding()
    {
        closestPoint = targetCollider.ClosestPoint(transform.position);
        float distance = (closestPoint - transform.position).ConvertVector2().sqrMagnitude;

        if (distance < MonsterFSM.MonsterData.AttackRadius * MonsterFSM.MonsterData.AttackRadius)
        {
            MonsterFSM.ChangeState(MonsterStateType.Attack);
        }
        else if (distance > MonsterFSM.MonsterData.ChaseRadius * MonsterFSM.MonsterData.ChaseRadius)
        {
            MonsterFSM.Target = null;
            useReturn = true;
        }
    }
}
