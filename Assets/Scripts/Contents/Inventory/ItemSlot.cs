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

    private float prevClickTime;
    private float clickTime;
    private float doubleClickTime = 0.3f;

    private bool isDrag = false;

    public Button button;
    public UnityEvent onDoubleClickEvent;
    public UnityEvent onDragEnter;
    public UnityEvent<Vector2> OnDragEvent;
    public UnityEvent<PointerEventData> onDragExit;

    public ItemSlotInfo ItemInfo {  get; private set; }
    public ItemData ItemData => ItemInfo.itemData;

    private string amountFormat = "{0}";

    public void SetItemData(ItemSlotInfo itemInfo)
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

            if(ItemData.ItemType <= ItemType.Relics)
            {
                amountText.text = string.Format(amountFormat, ItemInfo.Amount);
                textGameObject.SetActive(true);
                durabilitySlider.gameObject.SetActive(false);
            }
            else
            {
                durabilitySlider.gameObject.SetActive(true);
                durabilitySlider.value = (float)ItemInfo.Durability / ItemData.Durability;
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
        OnDragEvent?.Invoke(eventData.position);
        // = eventData.position; 
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
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

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        onDragExit?.Invoke(eventData);
    }

    public void OnClickSlot()
    {
        prevClickTime = clickTime;
        clickTime = Time.time;

        if (!isDrag && doubleClickTime > clickTime - prevClickTime)
        {
            onDoubleClickEvent?.Invoke();
        }
    }

    public void OnSwapItemInfo(ItemSlot itemSlot)
    {
        (ItemInfo, itemSlot.ItemInfo) = (itemSlot.ItemInfo, ItemInfo);
    }
}
