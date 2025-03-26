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

    // TODO :: // TODO :: TestPlayer -> PlayerInputHandler -> On Move And Rotate Event�� ����
    public void OnSetMoveValue(Vector2 moveValue)
    {
        this.MoveValue = moveValue;
    }

    // TODO :: TestPlayer -> PlayerInputHandler -> On Attack Event�� ����
    public void OnInputAttack()
    {
        if(CanAttack)
        {
            ChangeState(PlayerStateType.Attack);
        }
    }

    // TODO :: TestPlayer -> PlayerInputHandler -> On Interact Event�� ����
    public void OnSetInteract()
    {
        Debug.Log($"OnSetInteract ȣ���!");

        bool isFindTarget = false;
        // TODO :: �����ڵ�

        if(isFindTarget)
        {
            GameObject taget = gameObject;
            onTargetInteractEvent?.Invoke(taget);

            ChangeState(PlayerStateType.Interact);
        }
       
    }

    // TODO :: ������ TestObject ��ũ��Ʈ���� ��� ��
    public void OnSetIsPlayerInRange(bool value)
    {
        Debug.Log($"OnSetIsPlayerInRange ȣ���!");
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
