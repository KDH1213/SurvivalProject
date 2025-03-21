using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class ButtonInteract : MonoBehaviour
{
    private PlayerFSM playerFSM;

    private void Start()
    {
        playerFSM = GetComponent<PlayerFSM>();
    }

    public void OnMainToolUsedAndAttck(InputAction.CallbackContext context)
    {
        if (playerFSM.CanAttack && context.phase == InputActionPhase.Performed )
        {
            playerFSM.ChangeState(CharactorStateType.Attack);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Interact();
        }
    }

    public void OnSubToolUsed(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            SubToolUsed();
        }
    }

    public void OnBuild(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            Build();
        }
    }

    private void Interact()
    {
        Debug.Log("Interact!!");
    }

    private void SubToolUsed()
    {
        Debug.Log("SubTool Uesd!!");
    }

    private void Build()
    {
        Debug.Log("Build Button!!");
    }
}
