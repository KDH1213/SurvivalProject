using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerJoyStickInput : MonoBehaviour
{
    [HideInInspector]
    public Vector2 inputDirection;

    public UnityEvent<Vector2> onMoveAndRotateEvent;

    private void Awake()
    {
    }

    public void OnMoveAndRotate(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
        onMoveAndRotateEvent?.Invoke(inputDirection);
    }
}
