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
        MoveAndRotate(playerFSM.MoveValue);
    }

    public override void Exit()
    {
    }

    private void MoveAndRotate(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
        {
            playerFSM.ChangeState(PlayerStateType.Idle);
            return;
        }

        Vector3 dir = new Vector3(direction.x, 0, direction.y) * PlayerStats.Speed;

        transform.rotation = Quaternion.LookRotation(dir);

        PlayerFSM.Animator.SetFloat(PlayerAnimationHashCode.hashSpeed, PlayerStats.Speed);
        PlayerFSM.CharacterController.Move(dir * Time.deltaTime);
    }
}
