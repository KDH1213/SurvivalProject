using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.Port;

public class Inventory : MonoBehaviour, IDragHandler
{
    private static int maxSlot = 20;

    [SerializeField]
    private TextMeshProUGUI infomationText;

    private ItemData[] items = new ItemData[maxSlot];

    public List<ItemData> itemList;

    [SerializeField]
    private Slot slotPrefab;
    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots = new Slot[maxSlot];
    [SerializeField]
    private Slot[] EquipmentSlots = new Slot[6];

    public int SelectedSlotIndex { get; private set; } = -1;

    public UnityEvent onClickAddButton;
    public UnityEvent onClickRemoveButton;
    public UnityEvent onUpdateSlots;

    private int dragSeletedSlotIndex;
    private bool isOnDrag = false;

    public void AddListeners(UnityAction action)
    {
        foreach (var slot in slots)
        {
            slot.button.onClick.AddListener(action);
        }
    }

    private void Awake()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            items[i] = itemList[i];
        }

        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            EquipmentSlots[i].SlotIndex = i;
        }

        for (int i = 0; i < maxSlot; i++)
        {
            var slot = Instantiate(slotPrefab, scrollRect.content);
            slot.SlotIndex = i;
            slot.button.onClick.AddListener(() => { SelectedSlotIndex = slot.SlotIndex; });
            slot.button.onClick.AddListener(ShowIventoryItemInfomation);
            slot.button.onClick.AddListener(ShowSorketItemInfomation);

            slot.onDragEnter.AddListener(() =>
            {
                isOnDrag = true;
                dragSeletedSlotIndex = slot.SlotIndex;
            });

            slot.onDragExit.AddListener((PointerEventData eventData) =>
            {
                if (dragSeletedSlotIndex < 0 || dragSeletedSlotIndex >= items.Length || items[dragSeletedSlotIndex] == null)
                {
                    isOnDrag = false;
                    dragSeletedSlotIndex = -1;
                    return;
                }

                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                foreach (var result in results)
                {
                    var uiItemSlot = result.gameObject.GetComponent<Slot>();
                    if (uiItemSlot == null) continue;

                    int targetSlotIndex = uiItemSlot.SlotIndex;
                    int sourceSlotIndex = dragSeletedSlotIndex;

                    if (targetSlotIndex < 0 || targetSlotIndex >= items.Length) continue;

                    // 대상 슬롯이 비어 있는 경우
                    if (items[targetSlotIndex] == null)
                    {
                        items[targetSlotIndex] = items[sourceSlotIndex];
                        items[sourceSlotIndex] = null;
                    }
                    else if (items[sourceSlotIndex].ItemName == items[targetSlotIndex].ItemName)
                    {
                        // 같은 아이템일 경우 합치기
                        int totalAmount = items[sourceSlotIndex].Amount + items[targetSlotIndex].Amount;

                        if (totalAmount <= items[targetSlotIndex].MaxAmount)
                        {
                            items[targetSlotIndex].Amount = totalAmount;
                            items[sourceSlotIndex] = null;
                        }
                        else
                        {
                            items[targetSlotIndex].Amount = items[targetSlotIndex].MaxAmount;
                            items[sourceSlotIndex].Amount = totalAmount - items[targetSlotIndex].MaxAmount;
                        }
                    }
                    else
                    {
                        // 서로 다른 아이템 교환
                        var temp = items[targetSlotIndex];
                        items[targetSlotIndex] = items[sourceSlotIndex];
                        items[sourceSlotIndex] = temp;
                    }

                    isOnDrag = false;
                    dragSeletedSlotIndex = -1;

                    UpdateSlots(items);
                    break;
                }

                // 드래그 해제 후에도 아이템이 제대로 이동되지 않았다면 초기화
                isOnDrag = false;
                dragSeletedSlotIndex = -1;
            });
            slots[i] = slot;
        }

        UpdateSlots(items);
    }

    private void UpdateSlots(ItemData[] items)
    {
        for (int i = 0; i < maxSlot; i++)
        {
            if (i < items.Length && items[i] != null)
            {
                if (!slots[i].useSetAmout)
                {
                    slots[i].Item = items[i];
                    SetSlotAmount(i, items[i].Amount, items[i].MaxAmount);
                    slots[i].SetAmount();
                }
                else
                {
                    slots[i].Item = items[i];
                }
                slots[i].amountText.text = $"{slots[i].Item.Amount}";
            }
            else
            {
                slots[i].Item = null;
                slots[i].Amount = 0;
                slots[i].amountText.text = "0";
            }
        }
    }

    private void SetSlotAmount(int index, int amount, int maxAmount)
    {
        slots[index].Amount = amount;
        slots[index].MaxAmount = maxAmount;
    }

    public void ShowIventoryItemInfomation()
    {
        ItemData item = items[SelectedSlotIndex];

        if (item != null)
        {
            infomationText.text = $"Name: {item.ItemName}\n\nInfo: {item.ItemInfomation}\n\nType: {item.type}";
        }
        else
        {
            infomationText.text = "";
        }
    }

    public void ShowSorketItemInfomation()
    {
        ItemData item = EquipmentSlots[SelectedSlotIndex].Item;

        if (item != null)
        {
            infomationText.text = $"Name: {item.ItemName}\n\nInfo: {item.ItemInfomation}\n\nType: {item.type}";
        }
        else
        {
            infomationText.text = "";
        }
    }

    public void ResetInfomation()
    {
        infomationText.text = "";
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < maxSlot;
    }

    //public void RemoveItem()
    //{
    //    ItemData item = slots[SelectedSlotIndex].Item;

    //    itemList.Remove(item);
    //    UpdateSlots(itemList);

    //    onClickRemoveButton?.Invoke();
    //}

    public void EuipItem()
    {
        ItemData item = slots[SelectedSlotIndex].Item;

        if (item.type == itemType.Material)
        {
            return;
        }

        switch (item.type)
        {
            case itemType.Helmet:
                if (EquipmentSlots[0].Item != null)
                {
                    var empty = EquipmentSlots[0].Item;
                    EquipmentSlots[0].Item = item;
                    items[SelectedSlotIndex] = empty;
                }
                else
                {
                    EquipmentSlots[0].Item = item;
                    items[SelectedSlotIndex] = null;
                }
                break;
            case itemType.Weapon:
                if (EquipmentSlots[1].Item != null)
                {
                    var empty = EquipmentSlots[1].Item;
                    EquipmentSlots[1].Item = item;
                    items[SelectedSlotIndex] = empty;
                }
                else
                {
                    EquipmentSlots[1].Item = item;
                    items[SelectedSlotIndex] = null;
                }
                break;
            case itemType.Consumable:
                if (EquipmentSlots[2].Item != null)
                {
                    var empty = EquipmentSlots[2].Item;
                    EquipmentSlots[2].Item = item;
                    items[SelectedSlotIndex] = empty;
                }
                else
                {
                    EquipmentSlots[2].Item = item;
                    items[SelectedSlotIndex] = null;
                }
                break;
            case itemType.Armor:
                if (EquipmentSlots[3].Item != null)
                {
                    var empty = EquipmentSlots[3].Item;
                    EquipmentSlots[3].Item = item;
                    items[SelectedSlotIndex] = empty;
                }
                else
                {
                    EquipmentSlots[3].Item = item;
                    items[SelectedSlotIndex] = null;
                }
                break;
            case itemType.Pants:
                if (EquipmentSlots[4].Item != null)
                {
                    var empty = EquipmentSlots[4].Item;
                    EquipmentSlots[4].Item = item;
                    items[SelectedSlotIndex] = empty;
                }
                else
                {
                    EquipmentSlots[4].Item = item;
                    items[SelectedSlotIndex] = null;
                }
                break;
            case itemType.Shoes:
                if (EquipmentSlots[5].Item != null)
                {
                    var empty = EquipmentSlots[5].Item;
                    EquipmentSlots[5].Item = item;
                    items[SelectedSlotIndex] = empty;
                }
                else
                {
                    EquipmentSlots[5].Item = item;
                    items[SelectedSlotIndex] = null;
                }
                break;
        }

        UpdateSlots(items);
    }

    public void UnEquipItem()
    {
        ItemData item = EquipmentSlots[SelectedSlotIndex].Item;


    }

    private int FindEmptySlotIndex()
    {
        foreach (var slot in slots)
        {
            if (slot.Item == null)
            {
                return slot.SlotIndex;
            }
        }

        return -1;
    }

    public void AddItem(ItemData item, int amount)
    {

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
}
