using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    protected override void Awake()
    {
        base.Awake();
        stateType = PlayerStateType.Move;

    }

    public override void Enter()
    {
    }

    public override void ExecuteUpdate()
    {
        var moveDirection = playerFSM.MoveValue;
        if (moveDirection.sqrMagnitude < 0.01f)
        {
            playerFSM.ChangeState(PlayerStateType.Idle);
            return;
        }

        Vector3 dir = new Vector3(moveDirection.x, 0, moveDirection.y).normalized * PlayerStats.Speed;

        transform.rotation = Quaternion.LookRotation(dir);
        PlayerFSM.Animator.SetFloat(PlayerAnimationHashCode.hashSpeed, PlayerStats.Speed);
        PlayerFSM.CharacterController.Move(dir * Time.deltaTime);
    }

    public override void Exit()
    {
        PlayerFSM.Animator.SetFloat(PlayerAnimationHashCode.hashSpeed, 0f);
    }
}
