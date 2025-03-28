using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gather : MonoBehaviour, IGather, IInteractable
{
    public UnityEvent<int> OnEndInteractionEvent => onEndInteractionEvent;
    public UnityEvent<int> onEndInteractionEvent;

    public int TileID { get; set; }

    public bool IsInteractable => gameObject.activeSelf;

    public void OnInteraction()
    {
    }
     
    public void OnEndInteraction()
    {
        OnEndInteractionEvent?.Invoke(TileID);
    }

    public void Interact(GameObject interactor)
    {
    }
}
