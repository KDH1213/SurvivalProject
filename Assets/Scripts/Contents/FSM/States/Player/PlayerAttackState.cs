using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = PlayerStateType.Attack;
    }

    public override void Enter()
    {
        playerFSM.isMove = false;
        playerFSM.Animator.SetBool(AnimationHashCode.hashAttack, true);
    }

    public override void ExecuteUpdate()
    {
        if(playerFSM.IsAttack)
        {
            playerFSM.ChangeState(PlayerStateType.Attack);
        }
    }

    public override void Exit()
    {
        playerFSM.isMove = true;
    }

    public void OnEndAttackAnimationPlayer()
    {
        if (playerFSM.CurrentStateType == PlayerStateType.Attack)
        {
            playerFSM.Animator.SetBool(AnimationHashCode.hashAttack, false);
            playerFSM.IsAttack = false;
            playerFSM.SetCanAttack(false);
            playerFSM.ResetAttackTime();
            playerFSM.ChangeState(PlayerStateType.Idle);
        }
    }
}
