using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public struct DropItemInfo
{
    // �ӽ� �̸�
    public string ItemName;
    public int id;
    public int amount;
}

public class Inventory : MonoBehaviour
{
    private static int maxSlot = 20;

    // TODO :: �ӽ� �׽�Ʈ �ڵ�
    [SerializeField]
    private string name;
    [SerializeField]
    private ItemData itemData;

    [ContextMenu("Test1")]
    public void OnTest()
    {
        DropItemInfo dropItemInfo = new DropItemInfo();
        dropItemInfo.amount = 3;

        int.TryParse(name, out var result);
        dropItemInfo.id = result;
        dropItemInfo.ItemName = name;
        AddItem(dropItemInfo);
    }

    [SerializeField]
    private ItemSlot slotPrefab;

    [SerializeField]
    private Transform slotParent;

    [SerializeField]
    private ItemSlot[] itemSlots = new ItemSlot[maxSlot];

    private ItemInfo[] itemInfos = new ItemInfo[maxSlot];
    private Dictionary<string, List<ItemInfo>> inventoryItemTable = new Dictionary<string, List<ItemInfo>>();

    public int SelectedSlotIndex { get; private set; } = -1;

    public UnityEvent onClickAddButton;
    public UnityEvent onClickRemoveButton;
    public UnityEvent onUpdateSlots;

    private int dragSeletedSlotIndex;
    private bool isOnDrag = false;
    private int useSlotCount = 0;

    public void AddListeners(UnityAction action)
    {
        foreach (var slot in itemSlots)
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

                SelectedSlotIndex = -1;
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                foreach (var result in results)
                {
                    var uiItemSlot = result.gameObject.GetComponent<ItemSlot>();
                    if (uiItemSlot == null) 
                    {
                        continue; 
                    }

                    int targetSlotIndex = dragSeletedSlotIndex;
                    int sourceSlotIndex = uiItemSlot.SlotIndex;

                    if (targetSlotIndex < 0 || targetSlotIndex >= itemInfos.Length)
                    {
                        continue;
                    }

                    if (targetSlotIndex == sourceSlotIndex)
                    {
                        return;
                    }

                    // string id�� ����
                    if (itemInfos[targetSlotIndex].itemData != null && itemInfos[sourceSlotIndex].itemData.ItemName == itemInfos[targetSlotIndex].itemData.ItemName)
                    {
                        // ���� �������� ��� ��ġ��
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
                        itemInfos[targetSlotIndex].OnSwapItemInfo(itemInfos[sourceSlotIndex]);
                    }

                    UpdateSlot(targetSlotIndex);
                    UpdateSlot(sourceSlotIndex);
                    isOnDrag = false;
                    dragSeletedSlotIndex = -1;
                    break;
                }

                // �巡�� ���� �Ŀ��� �������� ����� �̵����� �ʾҴٸ� �ʱ�ȭ
                isOnDrag = false;
                dragSeletedSlotIndex = -1;
            });
            itemSlots[i] = slot;
        }

        UpdateSlots(itemInfos);
    }
    private void UpdateSlots(ItemInfo[] items)
    {
        for (int i = 0; i < maxSlot; ++i)
        {
            itemSlots[i].SetItemData(items[i]);
        }
    }

    public void OnUseItem()
    {
        if(SelectedSlotIndex == -1 || itemInfos[SelectedSlotIndex].itemData == null || itemInfos[SelectedSlotIndex].itemData.ItemType != itemType.Consumable)
        {
            return;
        }

        itemInfos[SelectedSlotIndex].Amount -= 1;

        if(itemInfos[SelectedSlotIndex].Amount == 0)
        {
            if (inventoryItemTable.TryGetValue(itemInfos[SelectedSlotIndex].itemData.ItemName, out var itemInfoList))
            {
                itemInfoList.Remove(itemInfos[SelectedSlotIndex]);
            }

            itemInfos[SelectedSlotIndex].Empty();
        }
        UpdateSlot(SelectedSlotIndex);
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
        foreach (var slot in itemSlots)
        {
            if (slot.ItemData == null)
            {
                return slot.SlotIndex;
            }
        }

        return -1;
    }

    // string���� ã�� ��� �� �� ������ id�� ����
    public void AddItem(DropItemInfo dropItemInfo)
    {
        if(inventoryItemTable.ContainsKey(dropItemInfo.ItemName))
        {
            var itemList = inventoryItemTable[dropItemInfo.ItemName];

            foreach (var item in itemList)
            {
                int addUseCount = (item.itemData.MaxAmount - item.Amount);
                if(dropItemInfo.amount <= addUseCount)
                {
                    item.Amount += dropItemInfo.amount;
                    UpdateSlot(item.index);
                    dropItemInfo.amount = 0;
                    break;
                }
                else
                {
                    item.Amount = item.itemData.MaxAmount;
                    dropItemInfo.amount -= addUseCount;
                }
               
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
        itemSlots[slotIndex].OnUpdateSlot();
    }

    private void CreateItem(DropItemInfo dropItemInfo, int slotIndex)
    {
        var itemInfo = new ItemInfo();
        itemInfo.index = slotIndex;
        itemInfo.Amount = dropItemInfo.amount;
        // TODO :: �ӽ� �ڵ�
        itemInfo.itemData = new ItemData(itemData);

        if (inventoryItemTable.ContainsKey(dropItemInfo.ItemName))
        {
            inventoryItemTable[dropItemInfo.ItemName].Add(itemInfo);
        }
        else
        {
            var itemInfoList = new List<ItemInfo>();
            itemInfoList.Add(itemInfo);
            inventoryItemTable.Add(dropItemInfo.ItemName, itemInfoList);
        }

        itemInfos[slotIndex] = itemInfo;
        itemSlots[slotIndex].SetItemData(itemInfo); 
        ++useSlotCount;
    }
}
