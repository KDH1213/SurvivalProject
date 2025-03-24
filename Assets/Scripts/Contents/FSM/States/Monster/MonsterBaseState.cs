using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBaseState : BaseState<MonsterStateType>
{
    protected MonsterFSM monsterFSM;
    public MonsterFSM MonsterFSM { get { return monsterFSM; } }

    protected virtual void Awake()
    {
        monsterFSM = GetComponent<MonsterFSM>();
    }

    public override void Enter()
    {
        enterStateEvent?.Invoke();
        this.enabled = true;
    }
    public override void ExecuteUpdate()
    {
        executeUpdateStateEvent?.Invoke();
    }
    public override void ExecuteFixedUpdate()
    {
        executeFixedUpdateStateEvent?.Invoke();
    }

    public override void Exit()
    {
        exitStateEvent?.Invoke();
        this.enabled = false;
    }
}
