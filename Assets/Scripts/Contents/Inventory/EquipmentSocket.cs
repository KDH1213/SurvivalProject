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

    public EquipmentType EquipmentType { get { return equipmentType; } }

    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private Slider durabilitySlider;

    [SerializeField]
    protected GameObject amountView;

    [SerializeField]
    protected TextMeshProUGUI amountText;

    public ItemInfo ItemInfo { get; protected set; } = new ItemInfo();
    public ItemData ItemData { get => ItemInfo.itemData; protected set { ItemInfo.itemData = value; } }
    public int Amount { get => ItemInfo.Amount; protected set { ItemInfo.Amount = value; } }
    public int Durability { get => ItemInfo.Durability; protected set { ItemInfo.Durability = value; } }

    private float prevClickTime;
    private float clickTime;
    private float doubleClickTime = 0.3f;

    public UnityEvent onEquipEvent;
    public UnityEvent onUnEquipEvent;
    public UnityEvent<EquipmentType> onClickEvent;
    public UnityEvent<EquipmentType, int, int> onChangeEquipEvent;

    public UnityEvent onDragEnter;
    public UnityEvent<Vector2> onDragEvent;
    public UnityEvent<PointerEventData> onDragExit;
    public UnityEvent<int> onBreakItemEvent;

    public virtual void InitializeSocket(EquipmentType equipmentType, ItemData itemData, int amount, int durability)
    {
        this.equipmentType = equipmentType;
        OnEquipment(equipmentType, itemData, amount, durability);
    }

    public virtual void OnEquipment(EquipmentType equipmentType, ItemData itemData, int amount, int durability)
    {
        if(ItemData != null)
        {
            ChangeEquipment(itemData, amount, durability);
        }
        else
        {
            Equiment(itemData, amount, durability);
        }
    }

    protected void Equiment(ItemData itemData, int amount, int durability)
    {
        ItemInfo.itemData = itemData;
        ItemInfo.Amount = amount;
        ItemInfo.Durability = durability; 

        if (ItemData != null)
        {
            itemIcon.sprite = itemData.ItemImage;
            itemIcon.color = new Color(1, 1, 1, 1);
            durabilitySlider.gameObject.SetActive(true);
            durabilitySlider.value = (float)Durability / ItemData.Durability;

            if (EquipmentType.Consumable == equipmentType)
            {
                durabilitySlider.gameObject.SetActive(false);
                amountText.text = amount.ToString();
                amountView.gameObject.SetActive(true);
            }
        }
        else
        {
            itemIcon.color = new Color(1, 1, 1, 0);
            durabilitySlider.gameObject.SetActive(false);
            amountView.gameObject.SetActive(false);
        }
    }
    protected void ChangeEquipment(ItemData itemData, int amount, int durability)
    {
        onChangeEquipEvent?.Invoke(equipmentType, Amount,Durability);
        Equiment(itemData, amount, durability);
    }

    public void OnUnEquipment()
    {
        OnEmpty();
    }

    public void OnUseDurability()
    {
        if(ItemData == null)
        {
            return;
        }

        --Durability;
        durabilitySlider.value = (float)Durability / ItemData.Durability;

        if (Durability <= 0)
        {
            onBreakItemEvent?.Invoke((int)equipmentType);
        }
    }

    protected void OnEmpty()
    {
        itemIcon.sprite = null;
        ItemData = null;

        itemIcon.color = new Color(1, 1, 1, 0);
        durabilitySlider.gameObject.SetActive(false);
        amountView.gameObject.SetActive(false);
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (ItemData == null)
        {
            eventData.pointerDrag = null;
            onDragExit?.Invoke(eventData);
        }
        else
        {
            onDragEnter?.Invoke();
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        onDragEvent?.Invoke(eventData.position);
    }
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        onDragExit?.Invoke(eventData);
    }


    public virtual void OnPointerClick(PointerEventData eventData)
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
