using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterFSM : FSMController<MonsterStateType>
{
    [field: SerializeField]
    public Animator Animator { get; private set; }
    [field: SerializeField]
    public NavMeshAgent Agent { get; private set; }

    public Transform Target { get; set; }

    [HideInInspector]
    public float aggroRange;

    [HideInInspector]
    public float Speed { get; private set; }

    [HideInInspector]
    public bool CanAttack { get; set; }

    [HideInInspector]
    public bool IsChase { get; set; }

    [HideInInspector]
    public float TargetDistance { get; set; }

    protected override void Awake()
    {
        CanAttack = false;
        Speed = Agent.speed;
        aggroRange = 5f;
    }

    private void Update()
    {
        StateTable[currentStateType].ExecuteUpdate();
    }
}
