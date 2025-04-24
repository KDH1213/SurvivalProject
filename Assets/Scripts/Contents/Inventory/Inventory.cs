using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    [field: SerializeField]
    public GameObject PrivewIcon { get; private set; }

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
    private Dictionary<int, List<ItemInfo>> inventoryItemTable = new Dictionary<int, List<ItemInfo>>();

    public Dictionary<int, List<ItemInfo>> InventroyItemTable => inventoryItemTable;
    public ItemInfo[] ItemInfos => itemInfos;

    private PlayerStats playerStats;

    public int SelectedSlotIndex { get; private set; } = -1;

    private int dragSeletedSlotIndex;
    private bool isOnDrag = false;
    private int useSlotCount = 0;

    private void OnEnable()
    {
        UpdateSlots();
    }

    private void Start()
    {
        playerStats = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerStats>();
    }

    //private void OnEnable()
    //{
    //    UpdateSlots();
    //}

    public void AddListeners(UnityAction action)
    {
        foreach (var slot in itemSlots)
        {
            slot.button.onClick.AddListener(action);
        }
    }

    public void Initialize()
    {
        itemInfoView.onDisableDivisionButtonFunc += IsFullInventory;
        itemInfoView.UIInventoryDivisionView.onDivisionEvent.AddListener(OnDivisionItem);

        for (int i = 0; i < maxSlot; ++i)
        {
            itemInfos[i] = new ItemInfo();
            itemInfos[i].index = i;

            var slot = Instantiate(slotPrefab, slotParent);
            slot.SlotIndex = i;
            slot.button.onClick.AddListener(() =>
            {
                SelectedSlotIndex = slot.SlotIndex;
                slot.OnClickSlot();
                itemInfoView.OnSetItemSlotInfo(itemInfos[SelectedSlotIndex]);
            });

            slot.OnDragEvent.AddListener((position) => { if (PrivewIcon != null) PrivewIcon.transform.position = position; });

            slot.onDragEnter.AddListener(() =>
            {
                isOnDrag = true;
                dragSeletedSlotIndex = slot.SlotIndex;

                if(PrivewIcon != null && slot.ItemData != null)
                {
                    PrivewIcon.SetActive(true);
                    PrivewIcon.GetComponent<Image>().sprite = slot.ItemData.ItemImage;
                }
            });

            slot.onDoubleClickEvent.AddListener(OnEquip);
            slot.onDragExit.AddListener(OnEndDrag);
            itemSlots[i] = slot;
        }

        UpdateSlots(itemInfos);

        if (SaveLoadManager.Data != null)
        {
            Load();
        }

        //if (useSlotCount == 0)
        //{
        //
        // var Item = DataTableManager.ItemTable.Get(1201002);
        // 
        // var testInfo = new DropItemInfo();
        // testInfo.amount = 100;
        // testInfo.ItemName = Item.ItemName;
        // testInfo.itemData = Item;
        // testInfo.id = 1201002;
        // AddItem(testInfo);

        //    for (int i = 0; i < 3; ++i)
        //    {
        //        var testItem = DataTableManager.ItemTable.Get(1201001 + i);

        //        var test = new DropItemInfo();
        //        test.amount = 1;
        //        test.ItemName = testItem.ItemName;
        //        test.itemData = testItem;
        //        AddItem(test);
        //    }

        //    for (int i = 0; i < 2; ++i)
        //    {
        //        var testItem = DataTableManager.ItemTable.Get(1202001 + i);

        //        var test = new DropItemInfo();
        //        test.amount = 1;
        //        test.ItemName = testItem.ItemName;
        //        test.itemData = testItem;
        //        AddItem(test);
        //    }
        //    for (int i = 0; i < 2; ++i)
        //    {
        //        var testItem = DataTableManager.ItemTable.Get(1203005 + i);

        //        var test = new DropItemInfo();
        //        test.amount = 1;
        //        test.ItemName = testItem.ItemName;
        //        test.itemData = testItem;
        //        AddItem(test);
        //    }
        //}
       
        equipmentSocketView.Initialize();

        //if (useSlotCount == 0)
        //{
        //    var equipmentItemList = SaveLoadManager.Data.equipmentItemIDList;

        //    if(equipmentItemList != null)
        //    {
        //        foreach (var equipmentItem in equipmentItemList)
        //        {
        //            if(equipmentItem != -1)
        //            {
        //                return;
        //            }
        //        }
        //    }

        //    for (int i = 0; i < 4; ++i)
        //    {
        //        var testItem = DataTableManager.ItemTable.Get(1111111 + i);

        //        var test = new DropItemInfo();
        //        test.amount = 1;
        //        test.ItemName = testItem.ItemName;
        //        test.itemData = testItem;
        //        AddItem(test);
        //    }

        //    var Item = DataTableManager.ItemTable.Get(1201004);

        //    var testInfo = new DropItemInfo();
        //    testInfo.amount = 500;
        //    testInfo.ItemName = Item.ItemName;
        //    testInfo.itemData = Item;
        //    testInfo.id = 1201004;
        //    AddItem(testInfo);
        //}

        for (int i = 0; i < 3; ++i)
        {
            var Item = DataTableManager.ItemTable.Get(1111115 + i);

            var testInfo = new DropItemInfo();
            testInfo.amount = 1;
            testInfo.ItemName = Item.ItemName;
            testInfo.itemData = Item;
            testInfo.id = 1111115 + i;
            AddItem(testInfo);
        }

        for (int i = 0; i < 3; ++i)
        {
            var Item = DataTableManager.ItemTable.Get(1111131 + i);

            var testInfo = new DropItemInfo();
            testInfo.amount = 1;
            testInfo.ItemName = Item.ItemName;
            testInfo.itemData = Item;
            testInfo.id = 1111131 + i;
            AddItem(testInfo);
        }

        for (int i = 0; i < 3; ++i)
        {
            var Item = DataTableManager.ItemTable.Get(1111119 + i);

            var testInfo = new DropItemInfo();
            testInfo.amount = 1;
            testInfo.ItemName = Item.ItemName;
            testInfo.itemData = Item;
            testInfo.id = 1111119 + i;
            AddItem(testInfo);
        }

        for (int i = 0; i < 3; ++i)
        {
            var Item = DataTableManager.ItemTable.Get(1111125 + i);

            var testInfo = new DropItemInfo();
            testInfo.amount = 1;
            testInfo.ItemName = Item.ItemName;
            testInfo.itemData = Item;
            testInfo.id = 1111125 + i;
            AddItem(testInfo);
        }

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

        if (inventoryItemTable.TryGetValue(itemInfos[SelectedSlotIndex].itemData.ID, out var itemInfoList))
        {
            itemInfoList.Remove(itemInfos[SelectedSlotIndex]);
        }

        itemInfoView.OnSetItemSlotInfo(null);
        itemInfos[SelectedSlotIndex].Empty();
        --useSlotCount;

        UpdateSlot(SelectedSlotIndex);
    }

    private void UseItem()
    {
        itemInfos[SelectedSlotIndex].Amount -= 1;
        playerStats.OnUseItem(itemInfos[SelectedSlotIndex].itemData);

        if (itemInfos[SelectedSlotIndex].Amount == 0)
        {
            if (inventoryItemTable.TryGetValue(itemInfos[SelectedSlotIndex].itemData.ID, out var itemInfoList))
            {
                itemInfoList.Remove(itemInfos[SelectedSlotIndex]);
            }

            itemInfoView.OnSetItemSlotInfo(null);
            itemInfos[SelectedSlotIndex].Empty();
            --useSlotCount;
        }

        UpdateSlot(SelectedSlotIndex);
    }

    public void ConsumeItem(int id, int count)
    {
        if (inventoryItemTable.ContainsKey(id))
        {
            var itemList = inventoryItemTable[id].OrderBy(item => item.Amount).
                ThenByDescending(item => item.index).ToList();
            for ( int i = 0; i < itemList.Count; i++ )
            {
                if (itemList[i].Amount >= count)
                {
                    itemList[i].Amount -= count;
                    UpdateSlot(itemList[i].index);
                    break;
                }
                else
                {
                    int leftCount = count - itemList[i].Amount;
                    itemList[i].Amount -= count - leftCount;
                    inventoryItemTable[id].Remove(itemList[i]);
                    itemInfos[itemList[i].index].Empty();
                    --useSlotCount;
                    count = leftCount;
                    UpdateSlot(itemList[i].index);
                }
                
            }
        }
        else
        {
            new KeyNotFoundException($"키를 찾을 수 없습니다. {id}");
        }
    }

    public int GetTotalItem(int id)
    {
        int count = 0;
        if (inventoryItemTable.ContainsKey(id))
        {
            var itemList = inventoryItemTable[id];
            foreach(var item in itemList)
            {
                count += item.Amount;
            }
            return count;
        }
        else
        {
            return count;
        }
    }

    public void OnEraseItem()
    {
        if (SelectedSlotIndex == -1 || itemInfos[SelectedSlotIndex].itemData == null)
        {
            return;
        }

        if (inventoryItemTable.TryGetValue(itemInfos[SelectedSlotIndex].itemData.ID, out var itemInfoList))
        {
            itemInfoList.Remove(itemInfos[SelectedSlotIndex]);
            itemInfos[SelectedSlotIndex].Empty();
            itemInfoView.OnSetItemSlotInfo(null);
            --useSlotCount;
        }

        UpdateSlot(SelectedSlotIndex);
    }

    public void OnDivisionItem(int count)
    {
        if (SelectedSlotIndex == -1 || itemInfos[SelectedSlotIndex].itemData == null)
        {
            return;
        }

        if (inventoryItemTable.TryGetValue(itemInfos[SelectedSlotIndex].itemData.ID, out var itemInfoList))
        {
            itemInfos[SelectedSlotIndex].Amount -= count;

            int slotIndex = FindEmptySlotIndex();

            itemInfos[slotIndex].Amount = count;
            itemInfos[slotIndex].itemData = itemInfos[SelectedSlotIndex].itemData;

            inventoryItemTable[itemInfos[SelectedSlotIndex].itemData.ID].Add(itemInfos[slotIndex]);

            ++useSlotCount;

            itemSlots[slotIndex].OnUpdateSlot();
            UpdateSlot(SelectedSlotIndex);
        }

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
        if(inventoryItemTable.ContainsKey(dropItemInfo.id))
        {
            var itemList = inventoryItemTable[dropItemInfo.id];

            foreach (var item in itemList)
            {
                int addUseCount = (item.itemData.MaxStack - item.Amount);
                if(dropItemInfo.amount <= addUseCount)
                {
                    item.Amount += dropItemInfo.amount;
                    UpdateSlot(item.index);
                    dropItemInfo.amount = 0;
                    break;
                }
                else
                {
                    item.Amount = item.itemData.MaxStack;
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

    public void OnEndDrag(PointerEventData eventData)
    {
        if(PrivewIcon != null)
        {
            PrivewIcon.SetActive(false);
        }

        if (dragSeletedSlotIndex < 0 || dragSeletedSlotIndex >= itemInfos.Length || itemInfos[dragSeletedSlotIndex] == null)
        {
            isOnDrag = false;
            dragSeletedSlotIndex = -1;
            itemInfoView.OnSetItemSlotInfo(null);
            return;
        }
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            var uiItemSlot = result.gameObject.GetComponent<ItemSlot>();
            var equipmentSocket = result.gameObject.GetComponent<EquipmentSocket>();

            if (uiItemSlot == null && equipmentSocket == null)
            {
                continue;
            }

            if(uiItemSlot != null)
            {
                OnEndDragTargetToItemSlot(uiItemSlot);
            }
            else if (equipmentSocket != null)
            {
                OnEndDragTargetToEquipmentSocket(equipmentSocket);
            }

            break;
        }

        // 드래그 해제 후에도 아이템이 제대로 이동되지 않았다면 초기화
        isOnDrag = false;

        SelectedSlotIndex = -1;
        dragSeletedSlotIndex = -1;
    }

    private void OnEndDragTargetToItemSlot(ItemSlot itemSlot)
    {
        int targetSlotIndex = dragSeletedSlotIndex;
        int sourceSlotIndex = itemSlot.SlotIndex;

        if (sourceSlotIndex < 0 || sourceSlotIndex >= itemInfos.Length || targetSlotIndex == sourceSlotIndex)
        {
            return;
        }

        // string id로 변경
        if (itemInfos[sourceSlotIndex].itemData != null && itemInfos[sourceSlotIndex].itemData.ID == itemInfos[targetSlotIndex].itemData.ID)
        {
            // 같은 아이템일 경우 합치기
            int totalAmount = itemInfos[targetSlotIndex].Amount + itemInfos[sourceSlotIndex].Amount;

            if (totalAmount <= itemInfos[targetSlotIndex].itemData.MaxStack)
            {
                if (inventoryItemTable.TryGetValue(itemInfos[sourceSlotIndex].itemData.ID, out var itemInfoList))
                {
                    itemInfoList.Remove(itemInfos[sourceSlotIndex]);
                }

                itemInfos[sourceSlotIndex].Amount = totalAmount;
                itemInfos[targetSlotIndex].Empty();
            }
            else
            {
                itemInfos[sourceSlotIndex].Amount = itemInfos[sourceSlotIndex].itemData.MaxStack;
                itemInfos[targetSlotIndex].Amount = totalAmount - itemInfos[sourceSlotIndex].itemData.MaxStack;
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
    }

    public void OnEndDragTargetToEquipmentSocket(EquipmentSocket equipmentSocket)
    {
        // itemInfos[dragSeletedSlotIndex].itemData
        // if ()
        if ((int)ItemData.ConvertEquipmentType(itemInfos[dragSeletedSlotIndex].itemData.ID, itemInfos[dragSeletedSlotIndex].itemData.ItemType) != (int)equipmentSocket.EquipmentType)
        {
            return;
        }

        SelectedSlotIndex = dragSeletedSlotIndex;
        OnEquip();
        // equipmentSocket.onChangeEquipEvent?.Invoke(equipmentSocket.eq)
    }

    public void OnSetEquipmentItem(ItemData itemData, int amount)
    {
        itemInfos[dragSeletedSlotIndex].itemData = itemData;
        itemInfos[dragSeletedSlotIndex].Amount = amount;

        if (inventoryItemTable.ContainsKey(itemData.ID))
        {
            inventoryItemTable[itemData.ID].Add(itemInfos[dragSeletedSlotIndex]);
        }
        else
        {
            var itemInfoList = new List<ItemInfo>();
            itemInfoList.Add(itemInfos[dragSeletedSlotIndex]);
            inventoryItemTable.Add(itemData.ID, itemInfoList);
        }

        itemSlots[dragSeletedSlotIndex].SetItemData(itemInfos[dragSeletedSlotIndex]);
        ++useSlotCount;
        dragSeletedSlotIndex = -1;
        isOnDrag = false;
    }

    private void CreateItem(DropItemInfo dropItemInfo, int slotIndex)
    {
        if (slotIndex == -1)
        {
            return;
        }

        itemInfos[slotIndex].Amount = dropItemInfo.amount;
        itemInfos[slotIndex].itemData = dropItemInfo.itemData;

        if (inventoryItemTable.ContainsKey(dropItemInfo.id))
        {
            inventoryItemTable[dropItemInfo.id].Add(itemInfos[slotIndex]);
        }
        else
        {
            var itemInfoList = new List<ItemInfo>();
            itemInfoList.Add(itemInfos[slotIndex]);
            inventoryItemTable.Add(dropItemInfo.id, itemInfoList);
        }

        itemSlots[slotIndex].OnUpdateSlot(); 
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
        if (SaveLoadManager.Data.isRestart)
        {
            return;
        }

        var itemSlotInfoList = SaveLoadManager.Data.ItemSlotInfoSaveDataList;

        for (var i = 0; i < itemSlotInfoList.Count; ++i)
        {
            if(itemSlotInfoList[i].itemID == -1)
            {
                continue;
            }

            itemInfos[i].Amount = itemSlotInfoList[i].amount;
            itemInfos[i].index = itemSlotInfoList[i].index;
            if(1201001 == itemSlotInfoList[i].itemID)
            {
                itemInfos[i].itemData = DataTableManager.ItemTable.Get(1200111);
            }
            else if (1201002 == itemSlotInfoList[i].itemID)
            {
                itemInfos[i].itemData = DataTableManager.ItemTable.Get(1200211);
            }
            else
            {
                itemInfos[i].itemData = DataTableManager.ItemTable.Get(itemSlotInfoList[i].itemID);
            }


            if(itemInfos[i].itemData == null)
            {
                continue;
            }

            if (inventoryItemTable.ContainsKey(itemInfos[i].itemData.ID))
            {
                inventoryItemTable[itemInfos[i].itemData.ID].Add(itemInfos[i]);
            }
            else
            {
                var itemInfoList = new List<ItemInfo>();
                itemInfoList.Add(itemInfos[i]);
                inventoryItemTable.Add(itemInfos[i].itemData.ID, itemInfoList);
            }

            ++useSlotCount;
        }

        UpdateSlots();
    }
}
