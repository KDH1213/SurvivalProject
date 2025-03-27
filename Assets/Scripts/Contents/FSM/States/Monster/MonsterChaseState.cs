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

        MonsterFSM.Agent.isStopped = false;  // 네비게이션 활성화
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

        // 타겟과의 거리 체크 및 상태 전환
        Chase();
    }

    public override void Exit()
    {
        if (MonsterFSM.Animator != null)
        {
            MonsterFSM.Animator.SetBool(AnimationHashCode.hashMove, false);
        }

        MonsterFSM.Agent.isStopped = true;  // 네비게이션 정지
    }

    private void Chase()
    {
        MonsterFSM.TargetDistance = Vector3.Distance(transform.position, MonsterFSM.TargetTransform.position);

        // 너무 멀리 떨어지면 추적 중지
        if (MonsterFSM.IsChase && MonsterFSM.TargetDistance > MonsterFSM.aggroRange * 1.5f)
        {
            MonsterFSM.SetIsChase(false);
            MonsterFSM.SetIsPlayerRange(false);
            MonsterFSM.Target = null; // 타겟 초기화
            Debug.Log("Monster: 플레이어를 놓쳤습니다. Idle 상태로 변경");
        }

        if (MonsterFSM.CanAttack && MonsterFSM.TargetDistance <= MonsterFSM.Weapon.Range)  // 공격 거리 조정 (기존 0.5 -> 1.0)
        {
            MonsterFSM.ChangeState(MonsterStateType.Attack);

        }
    }
}
