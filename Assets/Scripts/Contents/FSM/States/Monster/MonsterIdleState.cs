using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : MonsterBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Idle;
    }

    public override void Enter()
    {
        Debug.Log("Monster: Idle State!!");
        MonsterFSM.Agent.isStopped = true;
    }

    public override void ExecuteUpdate()
    {
        if (MonsterFSM.Target == null)
        {
            FindTarget();
        }

        ChangeChaseState();

        if (MonsterFSM.IsChase)
        {
            MonsterFSM.ChangeState(MonsterStateType.Chase);
        }
    }

    public override void Exit()
    {

    }

    private void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(MonsterFSM.transform.position, MonsterFSM.aggroRange * 1.2f);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                MonsterFSM.Target = collider.transform;
                Debug.Log($"Monster: {collider.name} 발견! 타겟 설정 완료");
                ChangeChaseState(); // 타겟을 찾으면 바로 추적 여부 결정
                return;
            }
        }
    }

    private void ChangeChaseState()
    {
        if (MonsterFSM.Target == null)
        {
            return;
        }

        MonsterFSM.TargetDistance = Vector3.Distance(transform.position, MonsterFSM.Target.position);

        // 플레이어를 발견하면 추적 시작
        if (!MonsterFSM.IsChase && MonsterFSM.TargetDistance < MonsterFSM.aggroRange)
        {
            MonsterFSM.IsChase = true;
            Debug.Log("Monster: 플레이어 감지! 추적 시작");
        }

        // 너무 멀리 떨어지면 추적 중지
        if (MonsterFSM.IsChase && MonsterFSM.TargetDistance > MonsterFSM.aggroRange * 1.5f)
        {
            MonsterFSM.IsChase = false;
            MonsterFSM.Target = null; // 타겟 초기화
            Debug.Log("Monster: 플레이어를 놓쳤습니다. Idle 상태로 변경");
        }
    }
}
