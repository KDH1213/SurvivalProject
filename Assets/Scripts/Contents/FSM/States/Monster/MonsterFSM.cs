using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterFSM : FSMController<MonsterStateType>
{
    [SerializeField]
    private Button button;

    [field: SerializeField]
    public Animator Animator { get; private set; }
    [field: SerializeField]
    public NavMeshAgent Agent { get; private set; }
    [field: SerializeField]
    public Weapon Weapon { get; private set; }

    [field: SerializeField]
    public Data MonsterData { get; private set; }

    [HideInInspector]
    public GameObject Target { get; set; }

    [HideInInspector]
    public Transform TargetTransform { get; set; }

    [HideInInspector]
    public Vector3 firstPosition;

    public bool CanAttack { get; private set; }

    public bool IsChase { get; private set; }

    public bool IsAttack { get; private set; }

    public float TargetDistance { get; set; }

    public bool IsPlayerInRange { get; private set; }

    public bool IsDead { get; private set; }
    public bool CanRouting { get; private set; }

    public bool IsInteractable => IsDead;

    protected override void Awake()
    {
        CanAttack = true;
        IsChase = false;
        IsPlayerInRange = false;
        IsAttack = false;
        IsDead = false;
        CanRouting = false;
        firstPosition = gameObject.transform.position;
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

    // TODO :: TestMonster -> DestructedEvent 이벤트에 연결
    public void OnDeath()
    {
        Debug.Log("Monster: Die!!");
        IsDead = true;
        CanRouting = true;
        ChangeState(MonsterStateType.Death);
    }
}
