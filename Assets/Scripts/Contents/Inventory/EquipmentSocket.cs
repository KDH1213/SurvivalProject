using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField]
    private Slider durabilitySlider;

    [SerializeField]
    private TextMeshProUGUI amountText;

    public ItemData ItemData { get; private set; }
    public int Amount { get; private set; }

    private float prevClickTime;
    private float clickTime;
    private float doubleClickTime = 0.3f;

    public UnityEvent onEquipEvent;
    public UnityEvent onUnEquipEvent;
    public UnityEvent<EquipmentType> onClickEvent;
    public UnityEvent<EquipmentType, int> onChangeEquipEvent;


    public void InitializeSocket(EquipmentType equipmentType, ItemData itemData, int amount)
    {
        this.equipmentType = equipmentType;
        OnEquipment(equipmentType, itemData, amount);
    }

    public void OnEquipment(EquipmentType equipmentType, ItemData itemData, int amount)
    {
        if(ItemData != null)
        {
            ChangeEquipment(itemData, amount);
        }
        else
        {
            Equiment(itemData, amount);
        }
    }

    private void Equiment(ItemData itemData, int amount)
    {
        this.ItemData = itemData;
        this.Amount = amount;

        if (ItemData != null)
        {
            itemIcon.sprite = itemData.ItemImage;
            itemIcon.color = new Color(1, 1, 1, 1);
            durabilitySlider.gameObject.SetActive(true);

            if(EquipmentType.Consumable == equipmentType)
            {
                durabilitySlider.gameObject.SetActive(false);
                amountText.text = amount.ToString();
                amountText.gameObject.SetActive(true);
            }
        }
        else
        {
            itemIcon.color = new Color(1, 1, 1, 0);
            durabilitySlider.gameObject.SetActive(false);
            amountText.gameObject.SetActive(false);
        }
    }
    private void ChangeEquipment(ItemData itemData, int amount)
    {
        onChangeEquipEvent?.Invoke(equipmentType, amount);
        Equiment(itemData, amount);
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
        durabilitySlider.gameObject.SetActive(false);
        amountText.gameObject.SetActive(false);
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
