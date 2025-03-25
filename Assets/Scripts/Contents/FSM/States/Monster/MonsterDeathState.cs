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
        Debug.Log("Monster: Daath State!!");
    }

    public override void ExecuteUpdate()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            MonsterFSM.Hp = 10f;
            Debug.Log($"{MonsterFSM.Hp}");
            MonsterFSM.ChangeState(MonsterStateType.Idle);
        }
    }

    public override void Exit()
    {

    }
}
