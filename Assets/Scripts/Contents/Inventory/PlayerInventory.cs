using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private TextMeshProUGUI infomationText;
    public List<Item> items;

    public Slot prefabSlot;
    public ScrollRect scrollRect;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private List<Slot> slots;

    public int SelectedSlotIndex { get; private set; } = -1;
    public int maxSlot = 20;

    public UnityEvent onClickAddButton;
    public UnityEvent onClickRemoveButton;
    public UnityEvent onUpdateSlots;

    private int dragSeletedSlotIndex;
    private bool isOnDrag = false;

    private List<Item> tempItemList;

    public void AddListeners(UnityAction action)
    {
        foreach (var slot in slots)
        {
            slot.button.onClick.AddListener(action);
        }
    }

    void Awake()
    {
        foreach (var item in items)
        {
            Debug.Log(item);
        }
        //items = new List<Item>();

        for (int i = 0; i < maxSlot; i++)
        {
            var slot = Instantiate(prefabSlot, scrollRect.content);
            slot.SlotIndex = i;
            slot.button.onClick.AddListener(() => { SelectedSlotIndex = slot.SlotIndex; });
            slot.button.onClick.AddListener(ShowInfomation);

            slot.onDragEnter.AddListener(() =>
            {
                isOnDrag = true;
                dragSeletedSlotIndex = slot.SlotIndex;
            });

            slot.onDragExit.AddListener((PointerEventData eventData) =>
            {
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                foreach (var result in results)
                {
                    var uiItemSlot = result.gameObject.GetComponent<Slot>();
                    if (uiItemSlot != null)
                    {
                        if (items[dragSeletedSlotIndex].itemName == items[uiItemSlot.SlotIndex].itemName)
                        {
                            //slots[uiItemSlot.SlotIndex].Amount += slots[dragSeletedSlotIndex].Amount;
                            slots[uiItemSlot.SlotIndex].Item.amount += slots[dragSeletedSlotIndex].Item.amount;

                            items[dragSeletedSlotIndex] = null;

                            items = items.Where(item => item != null).ToList();
                        }
                        else
                        {
                            (items[dragSeletedSlotIndex], items[uiItemSlot.SlotIndex]) = (items[uiItemSlot.SlotIndex], items[dragSeletedSlotIndex]);
                        }

                        UpdateSlots(items);
                        break;
                    }
                    //else if(uiItemSlot == null)
                    //{
                    //    slots[uiItemSlot.SlotIndex].Item = items[dragSeletedSlotIndex];
                    //    items[dragSeletedSlotIndex] = null;
                    //}
                }

                isOnDrag = false;
                dragSeletedSlotIndex = -1;
            });
            slots.Add(slot);
        }

        UpdateSlots(items);
    }

    //private void OnEnable()
    //{
    //    tempItemList = SaveLoadManager.Data.itemList.ToList();
    //    UpdateSlots(tempItemList);
    //}

    //private void OnDisable()
    //{
    //    SaveLoadManager.Data.itemList = tempItemList.ToList();
    //    SaveLoadManager.Save();
    //}

    private void UpdateSlots(List<Item> items)
    {
        for (int i = 0; i < maxSlot; i++)
        {
            if (i < items.Count && items[i] != null)
            {
                slots[i].Item = items[i];
                slots[i].Item.amount = items[i].amount;
                slots[i].amountText.text = $"{slots[i].Item.amount}";
            }
            else
            {
                slots[i].Item = null;
                slots[i].Amount = 0;
                slots[i].amountText.text = "0";
            }
        }
    }

    private void UpdateAmount(List<Item> items)
    {
        for (int i = 0; i < maxSlot; i++)
        {
            if (i < items.Count && items[i] != null)
            {
                slots[i].Item = items[i];
                slots[i].amountText.text = $"{slots[i].Amount}";
            }
            else
            {
                slots[i].Item = null;
                slots[i].Amount = 0;
                slots[i].amountText.text = "0";
            }
        }
    }

    //public void OnClickRemove()
    //{
    //    if (SelectedSlotIndex == -1)
    //        return;

    //    // Data에 지워주는 메소드를 제작해야 함.
    //    // 해당 메소드에서 유효성 검사를 진행
    //    tempItemList.Remove(slots[SelectedSlotIndex].Item);

    //    UpdateSlots(tempItemList);

    //    // UpdateSlots(tempItemList);
    //    onClickRemoveButton?.Invoke();
    //}

    public void ShowInfomation()
    {
        Item item = slots[SelectedSlotIndex].Item;

        if (item != null)
        {
            infomationText.text = $"Name: {item.itemName}\nInfo: {item.itemInfomation}\nType: {item.type}";
        }
        else
        {
            infomationText.text = $"Name:\nInfo:\nType:";
        }
    }

    public void RemoveItem()
    {
        Item item = slots[SelectedSlotIndex].Item;

        items.Remove(item);
        UpdateSlots(items);

        onClickRemoveButton?.Invoke();
    }

    public void ResetInfomation()
    {
        infomationText.text = $"Name:\nInfo:\nType:";
    }

    public void OnDrag(PointerEventData eventData)
    {
        //var results = new List<RaycastResult>();
        //EventSystem.current.RaycastAll(eventData, results);

        //foreach(var item in results)
        //{
        //    Debug.Log(item.gameObject.name);
        //}
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        var selectedObject = EventSystem.current.currentSelectedGameObject;
        var uiItemSlot = selectedObject?.GetComponent<Slot>();

        if (uiItemSlot != null)
        {
            dragSeletedSlotIndex = uiItemSlot.SlotIndex;
            isOnDrag = true;
        }
        else
        {
            isOnDrag = false;
            dragSeletedSlotIndex = -1;
        }
    }


    public void OnDragEnterItemSlot()
    {
        //dragSeletedSlotIndex = uiItemSlot.SlotIndex;
        //isOnDrag = true;
    }
    public void OnDragExitItemSlot()
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isOnDrag)
        {
            isOnDrag = false;
            dragSeletedSlotIndex = -1;
        }


        var selectedObject = EventSystem.current.currentSelectedGameObject;
        var uiItemSlot = selectedObject?.GetComponent<Slot>();

        if (uiItemSlot == null)
            return;

        (items[dragSeletedSlotIndex], items[uiItemSlot.SlotIndex]) = (items[uiItemSlot.SlotIndex], items[dragSeletedSlotIndex]);
        UpdateSlots(items);
        isOnDrag = false;
        dragSeletedSlotIndex = -1;
    }
}
