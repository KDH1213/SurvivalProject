using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = MonsterStateType.Attack;
    }

    public override void Enter()
    {

    }

    public override void ExecuteUpdate()
    {
    }

    public override void Exit()
    {

    }
}
