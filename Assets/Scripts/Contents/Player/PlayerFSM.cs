using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFSM : FSMController<CharactorStateType>
{
    [SerializeField]
    protected PlayerJoyStickInput joyStickInput;

    [field: SerializeField]
    public Animator Animator { get; private set; }

    [SerializeField]
    public float attackDelay = 5f;

    [HideInInspector]
    public float attackTime;
    [HideInInspector]
    public bool isMove = true;
    [HideInInspector]
    public bool CanAttack;

    public Vector2 MoveValue { get; private set; }


    protected override void Awake()
    {
        attackTime = attackDelay;
        CanAttack = true;
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

}
