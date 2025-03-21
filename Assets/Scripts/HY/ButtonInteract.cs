using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class ButtonInteract : MonoBehaviour
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

    public void OnMainToolUsedAndAttck(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            MainToolUsedAndAttack();
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

    private void MainToolUsedAndAttack()
    {
        Debug.Log("MainTool Used Or Attack!!");
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
