using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    public UnityEvent onTriggerEnterEvent;
    public UnityEvent onTriggerStayEvent;
    public UnityEvent onTriggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnterEvent?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerStayEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExitEvent?.Invoke();
    }
}
