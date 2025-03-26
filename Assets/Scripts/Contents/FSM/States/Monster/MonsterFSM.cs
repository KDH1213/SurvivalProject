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
    public Weapon Weapon { get; private set; }

    [HideInInspector]
    public GameObject Target { get; set; }

    [HideInInspector]
    public Transform TargetTransform { get; set; }

    [HideInInspector]
    public float aggroRange;

    public bool CanAttack { get; private set; }

    public bool IsChase { get; private set; }

    public bool IsAttack { get; private set; }

    public float TargetDistance { get; set; }

    public bool IsPlayerInRange { get; private set; }

    public bool IsDead { get; private set; }

    protected override void Awake()
    {
        CanAttack = false;
        IsChase = false;
        IsPlayerInRange = false;
        IsAttack = false;
        IsDead = false;
        aggroRange = 5f; 
    }

    private void Start()
    {
    }

    private void Update()
    {
        StateTable[currentStateType].ExecuteUpdate();
    }

    public void SetCanAttack(bool value)
    {
        CanAttack = value;
    }

    public void SetIsChase(bool value)
    {
        IsChase = value;
    }

    public void SetIsPlayerRange(bool value)
    {
        IsPlayerInRange = value;
    }

    public void SetIsAttack(bool value)
    {
        IsAttack = value;
    }

    public void OnDeath()
    {
        Debug.Log("Monster: Die!!");
        IsDead = true;
        ChangeState(MonsterStateType.Death);
    }
}
