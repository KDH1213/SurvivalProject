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
    // 임시 코드
    public ItemData itemData;
}

public class Inventory : MonoBehaviour, ISaveLoadData
{
    private static int maxSlot = 20;

    [SerializeField]
    private EquipmentSocketView equipmentSocketView;
    [SerializeField]
    private ItemSlot slotPrefab;

    [SerializeField]
    private ItemInfoView itemInfoView;

    [SerializeField]
    private Transform slotParent;

    [SerializeField]
    private ItemSlot[] itemSlots = new ItemSlot[maxSlot];

    private ItemInfo[] itemInfos = new ItemInfo[maxSlot];
    private Dictionary<string, List<ItemInfo>> inventoryItemTable = new Dictionary<string, List<ItemInfo>>();

    public int SelectedSlotIndex { get; private set; } = -1;

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

    public void Initialize()
    {

        for (int i = 0; i < maxSlot; ++i)
        {
            itemInfos[i] = new ItemInfo();
            itemInfos[i].index = i;

            var slot = Instantiate(slotPrefab, slotParent);
            slot.SlotIndex = i;
            slot.button.onClick.AddListener(() =>
            {
                SelectedSlotIndex = slot.SlotIndex;
                itemInfoView.OnSetItemInfo(itemInfos[SelectedSlotIndex].itemData);
            });

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
                    itemInfoView.OnSetItemInfo(null);
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

                    // string id로 변경
                    if (itemInfos[sourceSlotIndex].itemData != null && itemInfos[sourceSlotIndex].itemData.ItemName == itemInfos[targetSlotIndex].itemData.ItemName)
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
                        itemInfos[targetSlotIndex].index = targetSlotIndex;
                        itemInfos[sourceSlotIndex].index = sourceSlotIndex;
                        itemSlots[targetSlotIndex].OnSwapItemInfo(itemSlots[sourceSlotIndex]);
                        // itemInfos[targetSlotIndex].OnSwapItemInfo(itemInfos[sourceSlotIndex]);
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
            itemSlots[i] = slot;
        }

        UpdateSlots(itemInfos);

        if (SaveLoadManager.Data != null)
        {
            Load();
        }

        if (useSlotCount == 0)
        {
            for (int i = 0; i < 3; ++i)
            {
                var testItem = DataTableManager.ItemTable.Get(1201001 + i);

                var test = new DropItemInfo();
                test.amount = 1;
                test.ItemName = testItem.ItemName;
                test.itemData = testItem;
                AddItem(test);
            }

            for (int i = 0; i < 2; ++i)
            {
                var testItem = DataTableManager.ItemTable.Get(1202001 + i);

                var test = new DropItemInfo();
                test.amount = 1;
                test.ItemName = testItem.ItemName;
                test.itemData = testItem;
                AddItem(test);
            }
            for (int i = 0; i < 2; ++i)
            {
                var testItem = DataTableManager.ItemTable.Get(1203005 + i);

                var test = new DropItemInfo();
                test.amount = 1;
                test.ItemName = testItem.ItemName;
                test.itemData = testItem;
                AddItem(test);
            }

        }
        var Item = DataTableManager.ItemTable.Get(1202001);

        var testInfo = new DropItemInfo();
        testInfo.amount = 10;
        testInfo.ItemName = Item.ItemName;
        testInfo.itemData = Item;
        AddItem(testInfo);

        Item = DataTableManager.ItemTable.Get(1202002);

        testInfo = new DropItemInfo();
        testInfo.amount = 10;
        testInfo.ItemName = Item.ItemName;
        testInfo.itemData = Item;
        AddItem(testInfo);
        equipmentSocketView.Initialize();
    }

    private void UpdateSlots(ItemInfo[] items)
    {
        for (int i = 0; i < maxSlot; ++i)
        {
            itemSlots[i].SetItemData(items[i]);
        }
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < maxSlot; ++i)
        {
            itemSlots[i].SetItemData(itemInfos[i]);
        }
    }

    public void OnUseItem()
    {
        if(SelectedSlotIndex == -1 || itemInfos[SelectedSlotIndex].itemData == null || itemInfos[SelectedSlotIndex].itemData.ItemType != ItemType.Consumable)
        {
            return;
        }

        UseItem();
    }

    public void OnEquip()
    {
        if (SelectedSlotIndex == -1 || itemInfos[SelectedSlotIndex].itemData == null || itemInfos[SelectedSlotIndex].itemData.ItemType == ItemType.Material)
        {
            return;
        }

        var equipItemData = itemInfos[SelectedSlotIndex].itemData;
        int amount = itemInfos[SelectedSlotIndex].Amount;
        EquipItem();
        equipmentSocketView.OnEquipment(equipItemData.ItemType, equipItemData, amount);
    }

    private void EquipItem()
    {
        itemInfos[SelectedSlotIndex].Amount = 0;

        if (inventoryItemTable.TryGetValue(itemInfos[SelectedSlotIndex].itemData.ItemName, out var itemInfoList))
        {
            itemInfoList.Remove(itemInfos[SelectedSlotIndex]);
        }

        itemInfoView.OnSetItemInfo(null);
        itemInfos[SelectedSlotIndex].Empty();
        --useSlotCount;

        UpdateSlot(SelectedSlotIndex);
    }

    private void UseItem()
    {
        itemInfos[SelectedSlotIndex].Amount -= 1;

        if (itemInfos[SelectedSlotIndex].Amount == 0)
        {
            if (inventoryItemTable.TryGetValue(itemInfos[SelectedSlotIndex].itemData.ItemName, out var itemInfoList))
            {
                itemInfoList.Remove(itemInfos[SelectedSlotIndex]);
            }

            itemInfoView.OnSetItemInfo(null);
            itemInfos[SelectedSlotIndex].Empty();
            --useSlotCount;
        }
        UpdateSlot(SelectedSlotIndex);
    }

    public void OnEraseItem()
    {
        if (SelectedSlotIndex == -1 || itemInfos[SelectedSlotIndex].itemData == null)
        {
            return;
        }

        if (inventoryItemTable.TryGetValue(itemInfos[SelectedSlotIndex].itemData.ItemName, out var itemInfoList))
        {
            itemInfoList.Remove(itemInfos[SelectedSlotIndex]);
            itemInfos[SelectedSlotIndex].Empty();
            itemInfoView.OnSetItemInfo(null);
            --useSlotCount;
        }

        UpdateSlot(SelectedSlotIndex);
    }

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

    // string으로 찾는 경우 추 후 아이템 id로 변경
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

    private void ChangeItem(DropItemInfo dropItemInfo)
    {
        CreateItem(dropItemInfo, SelectedSlotIndex);
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
        // TODO :: 임시 코드
        itemInfo.itemData = dropItemInfo.itemData;

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

    public bool IsFullInventory()
    {
        return useSlotCount == maxSlot;
    }

    public void OnChangeEquimentItem(ItemData itemData, int amount)
    {
        var unEquimentInfo = new DropItemInfo();
        unEquimentInfo.amount = amount;
        unEquimentInfo.id = itemData.ID;
        unEquimentInfo.itemData = itemData;
        unEquimentInfo.ItemName = itemData.ItemName;
        ChangeItem(unEquimentInfo);
    }

    public void Save()
    {
        var itemSlotList = SaveLoadManager.Data.ItemSlotInfoSaveDataList;
        itemSlotList.Clear();

        foreach (var itemInfo in itemInfos)
        {
            var itemInfoSaveData = new ItemInfoSaveData();
            itemInfoSaveData.amount = itemInfo.Amount;
            itemInfoSaveData.itemID = itemInfo.itemData == null ? -1 : itemInfo.itemData.ID;
            itemInfoSaveData.index = itemInfo.index;
            itemSlotList.Add(itemInfoSaveData);
        }

        equipmentSocketView.Save();
    }

    public void Load()
    {
        var itemSlotInfoList = SaveLoadManager.Data.ItemSlotInfoSaveDataList;

        for (var i = 0; i < itemSlotInfoList.Count; ++i)
        {
            if(itemSlotInfoList[i].itemID == -1)
            {
                continue;
            }

            itemInfos[i].Amount = itemSlotInfoList[i].amount;
            itemInfos[i].index = itemSlotInfoList[i].index;
            itemInfos[i].itemData = DataTableManager.ItemTable.Get(itemSlotInfoList[i].itemID);

            if (inventoryItemTable.ContainsKey(itemInfos[i].itemData.ItemName))
            {
                inventoryItemTable[itemInfos[i].itemData.ItemName].Add(itemInfos[i]);
                ++useSlotCount;
            }
            else
            {
                var itemInfoList = new List<ItemInfo>();
                itemInfoList.Add(itemInfos[i]);
                inventoryItemTable.Add(itemInfos[i].itemData.ItemName, itemInfoList);
            }
        }

        UpdateSlots();
    }
}
