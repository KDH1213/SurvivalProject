using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestructedEvent : MonoBehaviour, IDestructible
{
    public UnityEvent<GameObject> destructedEvent;
    public void OnDestruction(GameObject attacker)
    {
        destructedEvent?.Invoke(attacker);
    }
}
