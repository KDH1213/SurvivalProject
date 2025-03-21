using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class JoyStickInteract : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;

    private Vector2 inputDirection;

    [SerializeField]
    private GameObject target;

    private NavMeshAgent agent;
    private PlayerCurrentState playerCurrentState;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerCurrentState = GetComponent<PlayerCurrentState>();
    }

    private void Update()
    {
        MoveAndRotate(inputDirection);
    }

    public void OnMoveAndRotate(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    private void MoveAndRotate(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
        {
            playerCurrentState.CurrentPlayerState = CharactorStateType.Idle;
            return;
        }
        else
        {
            playerCurrentState.CurrentPlayerState = CharactorStateType.Move;

            // 이동 벡터 계산
            Vector3 moveDir = new Vector3(direction.x, 0, direction.y).normalized;
            target.transform.position += moveDir * (moveSpeed * Time.deltaTime);

            // 이동하는 방향으로 캐릭터 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            target.transform.rotation = Quaternion.Slerp(target.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}
