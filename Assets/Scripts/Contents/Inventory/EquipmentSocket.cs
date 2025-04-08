using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSocket : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField]
    private EquipmentType equipmentType;

    [SerializeField]
    private Image itemIcon;

    public ItemData ItemData { get; private set; }

    private float prevClickTime;
    private float clickTime;
    private float doubleClickTime = 0.3f;

    public UnityEvent onEquipEvent;
    public UnityEvent onUnEquipEvent;
    public UnityEvent<EquipmentType> onClickEvent;
    public UnityEvent<EquipmentType> onChangeEquipEvent;

    public void InitializeSocket(EquipmentType equipmentType, ItemData itemData)
    {
        this.equipmentType = equipmentType;
        OnEquipment(equipmentType, itemData);
    }

    public void OnEquipment(EquipmentType equipmentType, ItemData itemData)
    {
        if(ItemData != null)
        {
            ChangeEquipment(itemData);
        }
        else
        {
            Equiment(itemData);
        }
    }

    private void Equiment(ItemData itemData)
    {
        this.ItemData = itemData;

        if (ItemData != null)
        {
            itemIcon.sprite = itemData.ItemImage;
            itemIcon.color = new Color(1, 1, 1, 1);
        }
        else
        {
            itemIcon.color = new Color(1, 1, 1, 0);
        }
    }
    private void ChangeEquipment(ItemData itemData)
    {
        onChangeEquipEvent?.Invoke(equipmentType);
        Equiment(itemData);
    }

    public void OnUnEquipment()
    {
        OnEmpty();
    }

    private void OnEmpty()
    {
        itemIcon.sprite = null;
        ItemData = null;

        itemIcon.color = new Color(1, 1, 1, 0);

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
        onClickEvent?.Invoke(equipmentType);

        if (doubleClickTime > clickTime - prevClickTime)
        {
            onUnEquipEvent?.Invoke();
        }
    }
}
