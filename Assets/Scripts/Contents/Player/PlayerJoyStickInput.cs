using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoyStickInput : MonoBehaviour
{
    [HideInInspector]
    public Vector2 inputDirection;

    private PlayerFSM playerFSM;

    private void Awake()
    {
        playerFSM = GetComponent<PlayerFSM>();
    }

    public void OnMoveAndRotate(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
        if (playerFSM.isMove)
        {
            playerFSM.ChangeState(CharactorStateType.Move);
        }
    }
}
