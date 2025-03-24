using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFSM : FSMController<PlayerStateType>
{
    [field: SerializeField]
    public Animator Animator { get; private set; }
    [field: SerializeField]
    public CharacterController CC { get; private set; }

    [SerializeField]
    public float attackDelay = 5f;

    private float attackTime;
    [HideInInspector]
    public bool isMove = true;
    [HideInInspector]
    public bool CanAttack;
    [HideInInspector]
    public bool IsAttack;
    [HideInInspector]
    public GameObject target;

    public static float gravity = -9.81f;
    [HideInInspector]
    public Vector3 velocity;

    public Vector2 MoveValue { get; private set; }
    public bool IsPlayerInRange { get; private set; }


    protected override void Awake()
    {
        attackTime = attackDelay;
        CanAttack = true;
        IsAttack = false;

        IsPlayerInRange = false;

        target = null;
    }

    private void Update()
    {
        if (!CanAttack)
        {
            attackTime += Time.deltaTime;
        }

        if (attackTime >= attackDelay)
        {
            CanAttack = true;
            attackTime = attackDelay;
        }

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

    public void ResetAttackTime()
    {
        attackTime = 0f;
    }

    public void SetCanAttack(bool value)
    {
        CanAttack = value;
    }
}
