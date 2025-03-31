using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    InteractType InteractType { get; }
    bool IsInteractable { get; }
    void Interact(GameObject interactor);
    UnityEvent<GameObject> OnEndInteractEvent { get; }
}
