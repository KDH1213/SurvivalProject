using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorAttackState : CharactorBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = CharactorStateType.Attack;
    }

    public override void Enter()
    {
        playerFSM.isMove = false;
        playerFSM.Animator.SetBool(AnimationHashCode.hashAttack, true);
    }

    public override void ExecuteUpdate()
    {

    }

    public override void Exit()
    {
        playerFSM.ChangeState(CharactorStateType.Idle);
        playerFSM.isMove = true;
    }

    public void OnEndAttackAnimation()
    {
        if (playerFSM.CurrentStateType == CharactorStateType.Attack)
        {
            playerFSM.Animator.SetBool(AnimationHashCode.hashAttack, false);
            playerFSM.CanAttack = false;
            playerFSM.attackTime = 0f;
            playerFSM.ChangeState(CharactorStateType.Idle);
        }
    }
}
