using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Image image;

    public TextMeshProUGUI amountText;

    public int Amount { get; set; }
    public int MaxAmount { get; set; }
    public int SlotIndex { get; set; }

    public Button button;

    public bool useSetAmout = false;

    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;
    public UnityEvent onDragEnter;
    public UnityEvent<PointerEventData> onDragExit;

    private ItemData item;
    public ItemData Item
    {
        get { return item; }
        set
        {
            item = value;
            if (item != null)
            {
                image.sprite = item.ItemImage;
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void SetAmount()
    {
        useSetAmout = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onDragEnter?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onDragExit?.Invoke(eventData);
    }
}
