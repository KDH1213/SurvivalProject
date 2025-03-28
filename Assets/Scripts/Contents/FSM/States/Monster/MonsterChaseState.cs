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

        MonsterFSM.Agent.isStopped = false;  // 네비게이션 활성화
        MonsterFSM.Agent.speed = MonsterStats.Speed;
    }

    public override void ExecuteUpdate()
    { 
        MonsterFSM.Agent.SetDestination(MonsterFSM.TargetTransform.position);

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
            useReturn = true;
        }

        if (MonsterFSM.CanAttack && MonsterFSM.TargetDistance <= MonsterFSM.Weapon.Range)  // 공격 거리 조정 (기존 0.5 -> 1.0)
        {
            MonsterFSM.ChangeState(MonsterStateType.Attack);

        }
    }

    // TODO :: 초기 위치로 돌아갈 때 도착했는지 여부를 판단해주는 함수
    private bool HasReachedDestination()
    {
        return !MonsterFSM.Agent.pathPending && // 경로 탐색이 끝났는지 확인
               MonsterFSM.Agent.remainingDistance <= MonsterFSM.Agent.stoppingDistance && // 목적지 근처인지 확인
               !MonsterFSM.Agent.hasPath; // 경로가 없으면 도착한 것으로 간주
    }

    // TODO :: 소환된 위치로 돌아가는 코드
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
