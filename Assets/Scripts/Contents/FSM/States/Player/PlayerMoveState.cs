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
        if (playerFSM.Animator != null)
            playerFSM.Animator.SetBool(AnimationHashCode.hashMove, true);
    }

    public override void ExecuteUpdate()
    {
        MoveAndRotate(playerFSM.MoveValue);
    }

    public override void Exit()
    {
        if (playerFSM.Animator != null)
        {
            playerFSM.Animator.SetBool(AnimationHashCode.hashMove, false);
        }
    }

    private void MoveAndRotate(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
        {
            playerFSM.ChangeState(PlayerStateType.Idle);
            return;
        }

        //// �̵� ���� ���
        //Vector3 moveDir = new Vector3(direction.x, 0, direction.y).normalized;
        //transform.position += moveDir * (moveSpeed * Time.deltaTime);
        //// �̵��ϴ� �������� ĳ���� ȸ��
        //transform.rotation = Quaternion.LookRotation(moveDir);

        Vector3 dir = new Vector3(direction.x, 0, direction.y) * PlayerStats.Speed;

        transform.rotation = Quaternion.LookRotation(dir);

        PlayerFSM.CharacterController.Move(dir * Time.deltaTime);
    }
}
