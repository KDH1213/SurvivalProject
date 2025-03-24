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
        MonsterFSM.Agent.speed = MonsterFSM.Speed;

        MonsterFSM.Agent.updateRotation = true;  // ȸ�� �ڵ� ����
        MonsterFSM.Agent.updatePosition = true;  // ��ġ �ڵ� ����

        if (!MonsterFSM.Agent.isOnNavMesh)
        {
            Debug.LogError("Monster is not on NavMesh!");
        }

        Debug.Log($"Monster: {MonsterFSM.Agent.speed}");
    }

    public override void ExecuteUpdate()
    {
        if (MonsterFSM.Target == null)
        {
            MonsterFSM.ChangeState(MonsterStateType.Idle);
            return;
        }

        MonsterFSM.Agent.SetDestination(MonsterFSM.Target.position);

        // Ÿ�ٰ��� �Ÿ� üũ �� ���� ��ȯ
        Chase();

        if (MonsterFSM.CanAttack)
        {
            MonsterFSM.ChangeState(MonsterStateType.Attack);
        }
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
        MonsterFSM.TargetDistance = Vector3.Distance(transform.position, MonsterFSM.Target.position);

        // �ʹ� �ָ� �������� ���� ����
        if (MonsterFSM.IsChase && MonsterFSM.TargetDistance > MonsterFSM.aggroRange * 1.5f)
        {
            MonsterFSM.IsChase = false;
            MonsterFSM.Target = null; // Ÿ�� �ʱ�ȭ
            Debug.Log("Monster: �÷��̾ ���ƽ��ϴ�. Idle ���·� ����");
        }

        if (MonsterFSM.TargetDistance <= 1.0f)  // ���� �Ÿ� ���� (���� 0.5 -> 1.0)
        {
            MonsterFSM.CanAttack = true;
            MonsterFSM.Agent.speed = MonsterFSM.Speed * 0.5f;  // ����������� �ӵ� ���̱�
        }
        else
        {
            MonsterFSM.CanAttack = false;
            MonsterFSM.Agent.speed = MonsterFSM.Speed;  // ���� �ӵ��� �̵�
        }
    }
}
