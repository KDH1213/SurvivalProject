using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    InteractType InteractType { get; }
    bool IsInteractable { get; }

    float InteractTime { get; }
    void Interact(GameObject interactor);
    UnityEvent<GameObject> OnEndInteractEvent { get; }
}


public interface IInteractCollision
{
    void OnEnterCollision();
    void OnExitCollision();
}