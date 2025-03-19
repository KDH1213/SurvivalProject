using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class TestController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private GameObject target;

    private Vector2 m_Move;
    private Vector2 m_Rotate;

    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        m_Rotate = context.ReadValue<Vector2>();
    }

    public void OnColorChange(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            Color color = Random.ColorHSV();
            ChangeColor(color);
        }
    }

    private void Update()
    {
        Move(m_Move);
        Rotate(m_Rotate);
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;

        var scaleMoveSpeed = moveSpeed * Time.deltaTime;

        var move = Quaternion.Euler(0, target.transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        target.transform.position += move * scaleMoveSpeed;
    }

    private void Rotate(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.01f) // 입력이 있는 경우에만 실행
        {
            float rotationAmount = rotateSpeed * direction.x * Time.deltaTime;
            target.transform.Rotate(Vector3.up * rotationAmount);
        }
    }

    private void ChangeColor(Color color)
    {
        target.GetComponent<Renderer>().material.color = color;
    }
}
