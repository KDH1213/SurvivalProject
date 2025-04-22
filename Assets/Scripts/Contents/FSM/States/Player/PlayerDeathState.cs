using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = PlayerStateType.Death;
    }

    public override void Enter()
    {
        PlayerFSM.Animator.SetTrigger(PlayerAnimationHashCode.hashDeath);
        PlayerFSM.Animator.SetBool(PlayerAnimationHashCode.hashIsDeath, true);
    }

    public override void ExecuteUpdate()
    {
    }

    public override void Exit()
    {
        PlayerFSM.Animator.SetBool(PlayerAnimationHashCode.hashIsDeath, false);
    }
}
