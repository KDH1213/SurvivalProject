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

    [field: SerializeField]
    public BaseData MonsterData { get; set; }

    public Transform Target { get; set; }

    [HideInInspector]
    public float Hp { get; set; }

    [HideInInspector]
    public float aggroRange;

    [HideInInspector]
    public float Speed { get; private set; }

    [HideInInspector]
    public bool canAttack;

    [HideInInspector]
    public bool isChase;

    [HideInInspector]
    public bool isAttack;

    [HideInInspector]
    public float TargetDistance { get; set; }

    [HideInInspector]
    public bool isPlayerInRange;

    protected override void Awake()
    {
        canAttack = false;
        isChase = false;
        isPlayerInRange = false;
        isAttack = false;
        Speed = Agent.speed;
        aggroRange = 5f;

        Hp = MonsterData.HP;
    }

    private void Update()
    {
        StateTable[currentStateType].ExecuteUpdate();
    }

    public void SetCanAttack(bool value)
    {
        canAttack = value;
    }

    public void SetIsChase(bool value)
    {
        isChase = value;
    }

    public void SetIsPlayerRange(bool value)
    {
        isPlayerInRange = value;
    }

    public void SetIsAttack(bool value)
    {
        isAttack = value;
    }

    public void OnDeath()
    {
        ChangeState(MonsterStateType.Death);
    }
}
