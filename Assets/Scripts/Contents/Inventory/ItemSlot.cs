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
    private GameObject textGameObject;
    public TextMeshProUGUI amountText;

    public int Amount { get { return itemInfo.Amount; } }
    public int SlotIndex { get; set; }

    public Button button;

    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;
    public UnityEvent onDragEnter;
    public UnityEvent<PointerEventData> onDragExit;

    public ItemInfo itemInfo {  get; private set; }
    public ItemData ItemData { get { return itemInfo.itemData != null ? itemInfo.itemData : null; } }

    private string amountFormat = "{0}";

    public void SetItemData(ItemInfo itemInfo)
    {
        this.itemInfo = itemInfo;

        if (itemInfo.itemData != null)
        {
            image.sprite = ItemData.ItemImage;
            image.color = new Color(1, 1, 1, 1);
            textGameObject.SetActive(true);
        }
        else
        {
            image.color = new Color(1, 1, 1, 0);
            textGameObject.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemInfo.itemData == null)
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
}
