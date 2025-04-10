using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private Slider durabilitySlider;

    [SerializeField]
    private GameObject textGameObject;

    [SerializeField]
    private TextMeshProUGUI amountText;

    public int Amount { get { return ItemInfo.Amount; } }
    public int SlotIndex { get; set; }

    public Button button;
    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;
    public UnityEvent onDragEnter;
    public UnityEvent<PointerEventData> onDragExit;

    public ItemInfo ItemInfo {  get; private set; }
    public ItemData ItemData => ItemInfo.itemData;

    private string amountFormat = "{0}";

    public void SetItemData(ItemInfo itemInfo)
    {
        this.ItemInfo = itemInfo;
        OnUpdateSlot();
    }

    public void OnUpdateSlot()
    {
        if (ItemData != null)
        {
            image.sprite = ItemData.ItemImage;
            image.color = new Color(1, 1, 1, 1);

            if(ItemData.ItemType >= ItemType.Consumable)
            {
                amountText.text = string.Format(amountFormat, ItemInfo.Amount);
                textGameObject.SetActive(true);
                durabilitySlider.gameObject.SetActive(false);
            }
            else
            {
                durabilitySlider.gameObject.SetActive(true);
                textGameObject.SetActive(false);
            }
            
        }
        else
        {
            image.color = new Color(1, 1, 1, 0);
            textGameObject.SetActive(false);
            durabilitySlider.gameObject.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(ItemData == null)
        {
            eventData.pointerDrag = null;
            onDragExit?.Invoke(eventData);
        }
        else
        {
            onDragEnter?.Invoke();
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onDragExit?.Invoke(eventData);
    }

    public void OnSwapItemInfo(ItemSlot itemSlot)
    {
        (ItemInfo, itemSlot.ItemInfo) = (itemSlot.ItemInfo, ItemInfo);
    }
}
