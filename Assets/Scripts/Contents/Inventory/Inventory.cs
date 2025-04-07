using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public struct DropItemInfo
{
    // 임시 이름
    public string ItemName;
    public int id;
    public int amount;
}

public class Inventory : MonoBehaviour
{
    private static int maxSlot = 20;
    private int useSlotCount = 0;

    private ItemInfo[] itemInfos = new ItemInfo[maxSlot];
    private Dictionary<string, List<ItemInfo>> inventoryItemTable;

    [SerializeField]
    private ItemSlot slotPrefab;

    [SerializeField]
    private Transform slotParent;

    [SerializeField]
    private ItemSlot[] slots = new ItemSlot[maxSlot];

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
        for (int i = 0; i < maxSlot; ++i)
        {
            itemInfos[i] = new ItemInfo();
            itemInfos[i].index = i;

            var slot = Instantiate(slotPrefab, slotParent);
            slot.SlotIndex = i;
            slot.button.onClick.AddListener(() => { SelectedSlotIndex = slot.SlotIndex;});

            slot.onDragEnter.AddListener(() =>
            {
                isOnDrag = true;
                dragSeletedSlotIndex = slot.SlotIndex;
            });

            slot.onDragExit.AddListener((PointerEventData eventData) =>
            {
                if (dragSeletedSlotIndex < 0 || dragSeletedSlotIndex >= itemInfos.Length || itemInfos[dragSeletedSlotIndex] == null)
                {
                    isOnDrag = false;
                    dragSeletedSlotIndex = -1;
                    return;
                }

                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                foreach (var result in results)
                {
                    var uiItemSlot = result.gameObject.GetComponent<ItemSlot>();
                    if (uiItemSlot == null) 
                    {
                        continue; 
                    }

                    int targetSlotIndex = uiItemSlot.SlotIndex;
                    int sourceSlotIndex = dragSeletedSlotIndex;

                    if (targetSlotIndex < 0 || targetSlotIndex >= itemInfos.Length)
                    {
                        continue;
                    }

                    if (targetSlotIndex == sourceSlotIndex)
                    {
                        return;
                    }

                    // string id로 변경
                    if (itemInfos[sourceSlotIndex].itemData.ItemName == itemInfos[targetSlotIndex].itemData.ItemName)
                    {
                        // 같은 아이템일 경우 합치기
                        int totalAmount = itemInfos[sourceSlotIndex].Amount + itemInfos[targetSlotIndex].Amount;

                        if (totalAmount <= itemInfos[targetSlotIndex].itemData.MaxAmount)
                        {
                            if (inventoryItemTable.TryGetValue(itemInfos[sourceSlotIndex].itemData.ItemName, out var itemInfoList))
                            {
                                itemInfoList.Remove(itemInfos[sourceSlotIndex]);
                            }

                            itemInfos[targetSlotIndex].Amount = totalAmount;
                            itemInfos[sourceSlotIndex].Empty();
                        }
                        else
                        {
                            itemInfos[targetSlotIndex].Amount = itemInfos[targetSlotIndex].itemData.MaxAmount;
                            itemInfos[sourceSlotIndex].Amount = totalAmount - itemInfos[targetSlotIndex].itemData.MaxAmount;
                        }
                    }
                    else
                    {
                        (itemInfos[targetSlotIndex], itemInfos[sourceSlotIndex]) = (itemInfos[sourceSlotIndex], itemInfos[targetSlotIndex]);
                    }

                    UpdateSlot(targetSlotIndex);
                    UpdateSlot(sourceSlotIndex);
                    isOnDrag = false;
                    dragSeletedSlotIndex = -1;
                    break;
                }

                // 드래그 해제 후에도 아이템이 제대로 이동되지 않았다면 초기화
                isOnDrag = false;
                dragSeletedSlotIndex = -1;
            });
            slots[i] = slot;
        }

        UpdateSlots(itemInfos);
    }
    private void UpdateSlots(ItemInfo[] items)
    {
        for (int i = 0; i < maxSlot; ++i)
        {
            slots[i].SetItemData(items[i]);
        }
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

    private int FindEmptySlotIndex()
    {
        foreach (var slot in slots)
        {
            if (slot.ItemData == null)
            {
                return slot.SlotIndex;
            }
        }

        return -1;
    }

    // string으로 찾는 경우 추 후 아이템 id로 변경
    public void AddItem(DropItemInfo dropItemInfo)
    {
        if(inventoryItemTable.ContainsKey(dropItemInfo.ItemName))
        {
            var itemList = inventoryItemTable[dropItemInfo.ItemName];

            foreach (var item in itemList)
            {
                int addCount = dropItemInfo.amount % (item.itemData.MaxAmount - item.Amount);
                item.Amount += addCount;
                dropItemInfo.amount -= addCount;

                UpdateSlot(item.index);
            }

            if(dropItemInfo.amount > 0 &&  useSlotCount < maxSlot)
            {
                int slotIndex = FindEmptySlotIndex();

                if (slotIndex == -1)
                {
                    return;
                }

                CreateItem(dropItemInfo, slotIndex);
            }
        }
        else
        {
            int slotIndex = FindEmptySlotIndex();

            if(slotIndex == -1)
            {
                return;
            }
            CreateItem(dropItemInfo, slotIndex);
        }
    }

    public void UpdateSlot(int slotIndex)
    {
        slots[slotIndex].SetItemData(itemInfos[slotIndex]);
    }

    private void CreateItem(DropItemInfo dropItemInfo, int slotIndex)
    {
        var itemInfo = new ItemInfo();
        itemInfo.index = slotIndex;
        itemInfo.itemData = null;
        itemInfo.Amount = dropItemInfo.amount;

        var itemInfoList = new List<ItemInfo>();
        itemInfoList.Add(itemInfo);
        inventoryItemTable.Add(dropItemInfo.ItemName, itemInfoList);
        slots[slotIndex].SetItemData(itemInfo); 
        UpdateSlot(slotIndex);
        ++useSlotCount;
    }
}
