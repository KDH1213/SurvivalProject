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
        // TODO :: 현재 체력 0이 되면 오브젝트 삭제
        Destroy(gameObject);
    }

    public override void ExecuteUpdate()
    {
    }

    public override void Exit()
    {

    }
}
