using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableRange : MonoBehaviour
{
    [SerializeField]
    private Button interactableButton;

    [SerializeField]
    private LayerMask interactableLayerMask;
    private Collider[] colliders = new Collider[1];

    private int colliderCount = 0;

    private void FixedUpdate()
    {
        if(Physics.OverlapSphereNonAlloc(transform.position, 5f, colliders, interactableLayerMask) != 0)
        {
            interactableButton.interactable = true;
        }
        else
        {
            interactableButton.interactable = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactCollision = other.GetComponent<IInteractCollision>();

        if(interactCollision != null)
        {
            interactCollision.OnEnterCollision();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactCollision = other.GetComponent<IInteractCollision>();

        if (interactCollision != null)
        {
            interactCollision.OnExitCollision();
        }
    }
}
