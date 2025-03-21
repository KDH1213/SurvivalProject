using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : FSMController<CharactorStateType>
{
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
}
