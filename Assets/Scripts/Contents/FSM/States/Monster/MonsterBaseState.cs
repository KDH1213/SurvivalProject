using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBaseState : BaseState<MonsterStateType>
{
    protected MonsterFSM monsterFSM;
    protected MonsterStats MonsterStats { get; private set; }
    public MonsterFSM MonsterFSM { get { return monsterFSM; } }

    // protected Animator Animator { get; private set; }
    protected NavMeshAgent Agent { get; private set; }

    protected virtual void Awake()
    {
        monsterFSM = GetComponent<MonsterFSM>();
        MonsterStats = GetComponent<MonsterStats>();
        Agent = GetComponent<NavMeshAgent>();
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
