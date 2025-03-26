using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerFSM : FSMController<PlayerStateType>
{
    [field: SerializeField]
    public Animator Animator { get; private set; }
    [field: SerializeField]
    public CharacterController CharacterController { get; private set; }
    [field: SerializeField]
    public Weapon Weapon { get; private set; }

    public GameObject Target { get; set; }

    [HideInInspector]
    public bool useMove = true;
    public bool CanAttack { get; private set; }

    public bool IsAttack { get; private set; }

    [HideInInspector]
    public float attackRange;

    public Vector2 MoveValue { get; private set; }

    public bool IsPlayerInRange { get; private set; }


    public UnityEvent<GameObject> onTargetInteractEvent;


    protected override void Awake()
    {
        SetCanAttack(true);
        OnEndAttack();

        IsPlayerInRange = false;
        attackRange = 2f;
    }

    private void Start()
    {
        onTargetInteractEvent.AddListener(((PlayerInteractState)StateTable[PlayerStateType.Interact]).OnSetTarget);
    }

    private void Update()
    {
        StateTable[currentStateType].ExecuteUpdate();
    }

    // TODO :: // TODO :: TestPlayer -> PlayerInputHandler -> On Move And Rotate Event에 연결
    public void OnSetMoveValue(Vector2 moveValue)
    {
        this.MoveValue = moveValue;
    }

    // TODO :: TestPlayer -> PlayerInputHandler -> On Attack Event에 연결
    public void OnInputAttack()
    {
        if(CanAttack)
        {
            ChangeState(PlayerStateType.Attack);
        }
    }

    // TODO :: TestPlayer -> PlayerInputHandler -> On Interact Event에 연결
    public void OnSetInteract()
    {
        Debug.Log($"OnSetInteract 호출됨!");

        bool isFindTarget = false;
        // TODO :: 예시코드

        if(isFindTarget)
        {
            GameObject taget = gameObject;
            onTargetInteractEvent?.Invoke(taget);

            ChangeState(PlayerStateType.Interact);
        }
       
    }

    // TODO :: 지금은 TestObject 스크립트에서 사용 중
    public void OnSetIsPlayerInRange(bool value)
    {
        Debug.Log($"OnSetIsPlayerInRange 호출됨!");
        IsPlayerInRange = value;
    }

    public void SetCanAttack(bool value)
    {
        CanAttack = value;
    }

    public void OnDeath()
    {
        ChangeState(PlayerStateType.Idle);
    }

    public void OnEndAttack()
    {
        IsAttack = false;
    }
}
