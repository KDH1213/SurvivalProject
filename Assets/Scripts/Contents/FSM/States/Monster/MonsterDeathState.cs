using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathState : MonsterBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Death;
    }

    public override void Enter()
    {
        // TODO :: ���� ü�� 0�� �Ǹ� ������Ʈ ����
        Destroy(gameObject);
    }

    public override void ExecuteUpdate()
    {
    }

    public override void Exit()
    {

    }
}
