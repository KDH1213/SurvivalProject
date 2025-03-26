using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterChaseState : MonsterBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Chase;
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
        if (MonsterFSM.CanAttack && MonsterFSM.IsAttack)
        {
            MonsterFSM.ChangeState(MonsterStateType.Attack);
        }

        if (MonsterFSM.Target == null)
        {
            MonsterFSM.ChangeState(MonsterStateType.Idle);
        }

        MonsterFSM.Agent.SetDestination(MonsterFSM.TargetTransform.position);

        // Ÿ�ٰ��� �Ÿ� üũ �� ���� ��ȯ
        Chase();
    }

    public override void Exit()
    {
        if (MonsterFSM.Animator != null)
        {
            MonsterFSM.Animator.SetBool(AnimationHashCode.hashMove, false);
        }

        MonsterFSM.Agent.isStopped = true;  // �׺���̼� ����
    }

    private void Chase()
    {
        MonsterFSM.TargetDistance = Vector3.Distance(transform.position, MonsterFSM.TargetTransform.position);

        // �ʹ� �ָ� �������� ���� ����
        if (MonsterFSM.IsChase && MonsterFSM.TargetDistance > MonsterFSM.aggroRange * 1.5f)
        {
            MonsterFSM.SetIsChase(false);
            MonsterFSM.SetIsPlayerRange(false);
            MonsterFSM.Target = null; // Ÿ�� �ʱ�ȭ
            Debug.Log("Monster: �÷��̾ ���ƽ��ϴ�. Idle ���·� ����");
        }

        if (MonsterFSM.TargetDistance <= 1.0f)  // ���� �Ÿ� ���� (���� 0.5 -> 1.0)
        {
            MonsterFSM.SetCanAttack(true);
            MonsterFSM.SetIsChase(false);
            MonsterFSM.Agent.speed = MonsterStats.Speed * 0.5f;  // ����������� �ӵ� ���̱�

            MonsterFSM.ChangeState(MonsterStateType.Attack);

        }
    }
}
