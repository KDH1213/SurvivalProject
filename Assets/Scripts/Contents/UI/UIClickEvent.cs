using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIClickEvent : MonoBehaviour, IPointerEnterHandler
{
    public UnityEvent onClickEvent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.selectedObject == gameObject)
        {
            onClickEvent?.Invoke();
        }
    }
}
