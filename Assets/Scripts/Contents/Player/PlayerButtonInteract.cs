using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerButtonInteract : MonoBehaviour
{
    private PlayerFSM playerFSM;

    public UnityEvent onAttackEvent;
    public UnityEvent onInteractEvent;
    public UnityEvent onDamageEvent;

    private void Start()
    {
        playerFSM = GetComponent<PlayerFSM>();
    }

    public void OnMainToolUsedAndAttck(InputAction.CallbackContext context)
    {
        if (playerFSM.CanAttack && context.phase == InputActionPhase.Performed )
        {
            onAttackEvent?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (playerFSM.IsPlayerInRange && context.phase == InputActionPhase.Performed)
        {
            onInteractEvent?.Invoke();
        }
    }

    public void OnSubToolUsed(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            SubToolUsed();
            onDamageEvent?.Invoke();
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
