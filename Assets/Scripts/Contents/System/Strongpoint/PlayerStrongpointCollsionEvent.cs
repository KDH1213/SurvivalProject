using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStrongpointCollsionEvent : MonoBehaviour
{
    public UnityEvent collsionEnterEvent;
    public UnityEvent collsionExitEvent;

    private void Awake()
    {
        collsionExitEvent?.Invoke();
    }


    private void OnTriggerEnter(Collider other)
    {
        collsionEnterEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        collsionExitEvent?.Invoke();
    }
}
