using UnityEngine;

public class MonsterChaseState : MonsterBaseState
{
    private bool useReturn;

    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Chase;
        useReturn = false;
    }

    public override void Enter()
    {
        Debug.Log("Monster: ChaseState!!");

        if (MonsterFSM.Animator != null)
        {
            MonsterFSM.Animator.SetBool(AnimationHashCode.hashMove, true);
        }

        MonsterFSM.Agent.isStopped = false;  // �׺���̼� Ȱ��ȭ
        MonsterFSM.Agent.speed = MonsterStats.Speed;
    }

    public override void ExecuteUpdate()
    {
        if (MonsterFSM.TargetTransform != null)
        {
            MonsterFSM.Agent.SetDestination(MonsterFSM.TargetTransform.position);
        }

        Chase();

        if (useReturn)
        {
            ReturnPosition();
        }
    }

    public override void Exit()
    {
        if (MonsterFSM.Animator != null)
        {
            MonsterFSM.Animator.SetBool(AnimationHashCode.hashMove, false);
        }

        useReturn = false;
        MonsterFSM.Agent.isStopped = true;  // �׺���̼� ����
    }

    private void Chase()
    {
        MonsterFSM.TargetDistance = Vector3.Distance(transform.position, MonsterFSM.TargetTransform.position);

        // �ʹ� �ָ� �������� ���� ����
        if (MonsterFSM.IsChase && MonsterFSM.TargetDistance > MonsterFSM.MonsterData.aggroRange)
        {
            MonsterFSM.SetIsChase(false);
            MonsterFSM.SetIsPlayerRange(false);
            MonsterFSM.Target = null; // Ÿ�� �ʱ�ȭ
            MonsterFSM.ChangeState(MonsterStateType.Idle);
            useReturn = true;

            return;
        }

        if (MonsterFSM.CanAttack && MonsterFSM.TargetDistance <= MonsterFSM.Weapon.Range)
        {
            MonsterFSM.ChangeState(MonsterStateType.Attack);

        }
    }

    // TODO :: �ʱ� ��ġ�� ���ư� �� �����ߴ��� ���θ� �Ǵ����ִ� �Լ�
    private bool HasReachedDestination()
    {
        return !MonsterFSM.Agent.pathPending && // ��� Ž���� �������� Ȯ��
               MonsterFSM.Agent.remainingDistance <= MonsterFSM.Agent.stoppingDistance && // ������ ��ó���� Ȯ��
               !MonsterFSM.Agent.hasPath; // ��ΰ� ������ ������ ������ ����
    }

    // TODO :: ��ȯ�� ��ġ�� ���ư��� �ڵ�
    private void ReturnPosition()
    {
        if (MonsterFSM.Animator == null)
        {
            MonsterFSM.Animator.SetBool(AnimationHashCode.hashMove, true);
        }
        MonsterFSM.Agent.SetDestination(MonsterFSM.firstPosition);

        if (HasReachedDestination())
        {
            MonsterFSM.ChangeState(MonsterStateType.Idle);
        }
    }
}
