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
        if (MonsterFSM.Target == null)
        {
            MonsterFSM.ChangeState(MonsterStateType.Idle);
            return;
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

        if (MonsterFSM.CanAttack && MonsterFSM.TargetDistance <= MonsterFSM.Weapon.Range)  // ���� �Ÿ� ���� (���� 0.5 -> 1.0)
        {
            MonsterFSM.ChangeState(MonsterStateType.Attack);

        }
    }
}
