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

        if (MonsterFSM.isChase)
        {
            MonsterFSM.ChangeState(MonsterStateType.Chase);
        }
    }

    public override void Exit()
    {

    }

    private void FindTarget()
    {
        if(MonsterFSM.Target != null)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(MonsterFSM.transform.position, MonsterFSM.aggroRange * 1.2f);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                MonsterFSM.Target = collider.transform;
                Debug.Log($"Monster: {collider.name} �߰�! Ÿ�� ���� �Ϸ�");
                ChangeChaseState(); // Ÿ���� ã���� �ٷ� ���� ���� ����
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

        // �÷��̾ �߰��ϸ� ���� ����
        if (!MonsterFSM.isChase && MonsterFSM.TargetDistance < MonsterFSM.aggroRange)
        {
            MonsterFSM.SetIsChase(true);
            MonsterFSM.SetIsPlayerRange(true);
            Debug.Log("Monster: �÷��̾� ����! ���� ����");
        }
    }
}
