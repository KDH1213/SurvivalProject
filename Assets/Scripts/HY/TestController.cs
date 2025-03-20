using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class TestController : MonoBehaviour
{
    public static readonly int hashSpeed = Animator.StringToHash("Speed");
    public static readonly int hashWalk = Animator.StringToHash("walk");


    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private GameObject target;

    private Vector2 move;
    private Animator animator;
    private NavMeshAgent agent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }


    public void OnColorChange(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Color color = Random.ColorHSV();
            ChangeColor(color);
        }
    }

    public void OnSit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            target.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    private void Update()
    {
        MoveAndRotate(move);
        //animator.SetFloat(hashSpeed, agent.velocity.magnitude);
    }


    private void MoveAndRotate(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
        {
            animator.SetBool(hashWalk, false);
            return;
        }
        else
        {
            animator.SetBool(hashWalk, true);

            // 이동 벡터 계산 (월드 좌표 기준)
            Vector3 moveDir = new Vector3(direction.x, 0, direction.y).normalized;
            target.transform.position += moveDir * (moveSpeed * Time.deltaTime);

            // 이동하는 방향으로 캐릭터 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            target.transform.rotation = Quaternion.Slerp(target.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void ChangeColor(Color color)
    {
        target.GetComponent<Renderer>().material.color = color;
    }
}
