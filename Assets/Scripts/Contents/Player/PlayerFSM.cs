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
    public CharacterController CC { get; private set; }
    [field: SerializeField]
    public Weapon Weapon { get; private set; }
    [field: SerializeField]
    public CharactorStats PlayerStats { get; private set; }

    public GameObject Target { get; set; }

    [HideInInspector]
    public bool isMove = true;

    [HideInInspector]
    public bool CanAttack;

    [HideInInspector]
    public bool IsAttack;

    [HideInInspector]
    public float attackRange;

    public Vector2 MoveValue { get; private set; }

    public bool IsPlayerInRange { get; private set; }


    protected override void Awake()
    {
        CanAttack = true;
        IsAttack = false;

        IsPlayerInRange = false;
        attackRange = 2f;
    }

    private void Update()
    {
        StateTable[currentStateType].ExecuteUpdate();
    }

    public void OnSetMoveValue(Vector2 moveValue)
    {
        this.MoveValue = moveValue;
    }

    public void OnSetAttack()
    {
        Debug.Log($"OnSetAttack »£√‚µ !");
        IsAttack = true;
        ChangeState(PlayerStateType.Attack);
    }

    public void OnSetInteract()
    {
        Debug.Log($"OnSetInteract »£√‚µ !");
        ChangeState(PlayerStateType.Interact);
    }

    public void OnSetIsPlayerInRange(bool value)
    {
        Debug.Log($"OnSetIsPlayerInRange »£√‚µ !");
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
}
