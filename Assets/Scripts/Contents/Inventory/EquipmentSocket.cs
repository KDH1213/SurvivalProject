using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EquipmentSocket : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField]
    private EquipmentType equipmentType;

    [SerializeField]
    private Sprite itemIcon;

    public ItemData ItemData { get; private set; }

    private float prevClickTime;
    private float clickTime;
    private float doubleClickTime = 0.3f;

    public UnityEvent onEquipEvent;
    public UnityEvent onUnEquipEvent;
    public UnityEvent onChangeEquipEvent;

    public void InitializeSocket(EquipmentType equipmentType, ItemData itemData)
    {
        this.equipmentType = equipmentType;
        this.ItemData = itemData;
        itemIcon = itemData.ItemImage;
    }

    private void OnEquipment(EquipmentType equipmentType, ItemData itemData)
    {

    }

    private void OnEmpty()
    {
        equipmentType = EquipmentType.None;
        itemIcon = null;
        ItemData = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
    public void OnEndDrag(PointerEventData eventData)
    {
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        prevClickTime = clickTime;
        clickTime = Time.time;

        if(doubleClickTime > clickTime - prevClickTime)
        {
            onUnEquipEvent?.Invoke();
            OnEmpty();
        }
    }
}
