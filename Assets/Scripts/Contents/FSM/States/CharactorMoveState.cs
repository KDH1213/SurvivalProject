using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class CharactorMoveState : CharactorBaseState
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;

    private Vector2 inputDirection;

    protected override void Awake()
    {
        base.Awake();
        stateType = CharactorStateType.Move;
    }

    public override void Enter()
    {
        if (playerFSM.Animator != null)
            playerFSM.Animator.SetBool(AnimationHashCode.hashMove, true);
    }

    public override void ExecuteUpdate()
    {
        inputDirection = joyStickInput.inputDirection;

        MoveAndRotate(inputDirection);
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
            playerFSM.ChangeState(CharactorStateType.Idle);
            return;
        }

        // �̵� ���� ���
        Vector3 moveDir = new Vector3(direction.x, 0, direction.y).normalized;
        transform.position += moveDir * (moveSpeed * Time.deltaTime);

        // �̵��ϴ� �������� ĳ���� ȸ��
        Quaternion rotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }
}
