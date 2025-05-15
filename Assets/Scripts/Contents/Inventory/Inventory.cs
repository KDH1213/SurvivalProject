using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, ISaveLoadData
{
    private static int maxSlot = 20;

    [field: SerializeField]
    public GameObject PrivewIcon { get; private set; }

    [SerializeField]
    private EquipmentSocketView equipmentSocketView;
    public EquipmentSocketView EquipmentSocketView => equipmentSocketView;

    [SerializeField]
    private ItemSlot slotPrefab;

    [SerializeField]
    private ItemInfoView itemInfoView;

    [SerializeField]
    private Transform slotParent;

    [SerializeField]
    private ItemSlot[] itemSlots = new ItemSlot[maxSlot];

    private ItemSlotInfo[] itemSlotInfos = new ItemSlotInfo[maxSlot];
    private Dictionary<int, List<ItemSlotInfo>> inventoryItemTable = new Dictionary<int, List<ItemSlotInfo>>();

    public Dictionary<int, List<ItemSlotInfo>> InventroyItemTable => inventoryItemTable;
    public List<List<ItemSlotInfo>> GatherItemSlotInfoList { get; private set; } = new List<List<ItemSlotInfo>>();

    public ItemSlotInfo[] ItemInfos => itemSlotInfos;

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
        GatherItemSlotInfoList.Add(new List<ItemSlotInfo>());
        GatherItemSlotInfoList.Add(new List<ItemSlotInfo>());

        itemInfoView.onDisableDivisionButtonFunc += IsFullInventory;
        itemInfoView.UIInventoryDivisionView.onDivisionEvent.AddListener(OnDivisionItem);

        for (int i = 0; i < maxSlot; ++i)
        {
            itemSlotInfos[i] = new ItemSlotInfo();
            itemSlotInfos[i].index = i;

            var slot = Instantiate(slotPrefab, slotParent);
            slot.SlotIndex = i;
            slot.button.onClick.AddListener(() =>
            {
                SelectedSlotIndex = slot.SlotIndex;
                slot.OnClickSlot();
                itemInfoView.OnSetItemSlotInfo(itemSlotInfos[SelectedSlotIndex]);
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

            itemSlotInfos[i].onUseDurabilityAction += OnUseDurability;

            slot.onDoubleClickEvent.AddListener(OnEquip);
            slot.onDragExit.AddListener(OnEndDrag);
            itemSlots[i] = slot;
        }

        UpdateSlots(itemSlotInfos);

        if (SaveLoadManager.Data != null)
        {
            Load();
        }

        equipmentSocketView.Initialize();

        if (useSlotCount == 0)
        {
            var equipmentItemList = SaveLoadManager.Data.equipmentItemIDList;

            if (equipmentItemList != null)
            {
                foreach (var equipmentItem in equipmentItemList)
                {
                    if (equipmentItem != -1)
                    {
                        return;
                    }
                }
            }

            var Item = DataTableManager.ItemTable.Get(1201004);

            var testInfo = new DropItemInfo();
            testInfo.amount = 500;
            testInfo.itemData = Item;
            testInfo.id = Item.ID;
            AddItem(testInfo);

        }
    }

    private void UpdateSlots(ItemSlotInfo[] items)
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
            itemSlots[i].SetItemData(itemSlotInfos[i]);
        }
    }

    public void OnUseItem()
    {
        if(SelectedSlotIndex == -1 || itemSlotInfos[SelectedSlotIndex].itemData == null || itemSlotInfos[SelectedSlotIndex].itemData.ItemType != ItemType.Consumable)
        {
            return;
        }

        UseItem();
    }

    public void OnEquip()
    {
        if (SelectedSlotIndex == -1 || itemSlotInfos[SelectedSlotIndex].itemData == null 
            || itemSlotInfos[SelectedSlotIndex].itemData.ItemType == ItemType.Material
            || itemSlotInfos[SelectedSlotIndex].itemData.ItemType == ItemType.Relics)
        {
            return;
        }

        var equipItemData = itemSlotInfos[SelectedSlotIndex].itemData;
        int amount = itemSlotInfos[SelectedSlotIndex].Amount;
        int durability = itemSlotInfos[SelectedSlotIndex].Durability;
        EquipItem();
        equipmentSocketView.OnEquipment(equipItemData.ItemType, equipItemData, amount, durability);
    }

    private void EquipItem()
    {
        itemSlotInfos[SelectedSlotIndex].Amount = 0;

        if (inventoryItemTable.TryGetValue(itemSlotInfos[SelectedSlotIndex].itemData.ID, out var itemInfoList))
        {
            itemInfoList.Remove(itemSlotInfos[SelectedSlotIndex]);
        }

        OnCheckEraseGatherItem(itemSlotInfos[SelectedSlotIndex]);
        itemInfoView.OnSetItemSlotInfo(null);
        itemSlotInfos[SelectedSlotIndex].Empty();
        --useSlotCount;

        UpdateSlot(SelectedSlotIndex);
    }

    private void UseItem()
    {
        itemSlotInfos[SelectedSlotIndex].Amount -= 1;
        playerStats.OnUseItem(itemSlotInfos[SelectedSlotIndex].itemData);

        if (itemSlotInfos[SelectedSlotIndex].Amount == 0)
        {
            if (inventoryItemTable.TryGetValue(itemSlotInfos[SelectedSlotIndex].itemData.ID, out var itemInfoList))
            {
                itemInfoList.Remove(itemSlotInfos[SelectedSlotIndex]);
            }

            itemInfoView.OnSetItemSlotInfo(null);
            itemSlotInfos[SelectedSlotIndex].Empty();
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

                    if(itemList[i].Amount == 0)
                    {
                        inventoryItemTable[id].Remove(itemSlotInfos[itemList[i].index]);
                        itemSlotInfos[itemList[i].index].Empty();
                    }
                    UpdateSlot(itemList[i].index);
                    break;
                }
                else
                {
                    int leftCount = count - itemList[i].Amount;
                    itemList[i].Amount -= count - leftCount;
                    inventoryItemTable[id].Remove(itemList[i]);
                    itemSlotInfos[itemList[i].index].Empty();
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
        if (SelectedSlotIndex == -1 || itemSlotInfos[SelectedSlotIndex].itemData == null)
        {
            return;
        }

        OnCheckEraseGatherItem(itemSlotInfos[SelectedSlotIndex]);
        if (inventoryItemTable.TryGetValue(itemSlotInfos[SelectedSlotIndex].itemData.ID, out var itemInfoList))
        {
            itemInfoList.Remove(itemSlotInfos[SelectedSlotIndex]);
            itemSlotInfos[SelectedSlotIndex].Empty();
            itemInfoView.OnSetItemSlotInfo(null);
            --useSlotCount;
        }


        UpdateSlot(SelectedSlotIndex);
    }

    public void OnDivisionItem(int count)
    {
        if (SelectedSlotIndex == -1 || itemSlotInfos[SelectedSlotIndex].itemData == null)
        {
            return;
        }

        if (inventoryItemTable.TryGetValue(itemSlotInfos[SelectedSlotIndex].itemData.ID, out var itemInfoList))
        {
            itemSlotInfos[SelectedSlotIndex].Amount -= count;

            int slotIndex = FindEmptySlotIndex();

            itemSlotInfos[slotIndex].Amount = count;
            itemSlotInfos[slotIndex].itemData = itemSlotInfos[SelectedSlotIndex].itemData;

            inventoryItemTable[itemSlotInfos[SelectedSlotIndex].itemData.ID].Add(itemSlotInfos[slotIndex]);

            ++useSlotCount;

            if (itemSlotInfos[SelectedSlotIndex].Amount <= 0)
            {
                itemSlotInfos[SelectedSlotIndex].itemData = null;
            }

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

        if (dragSeletedSlotIndex < 0 || dragSeletedSlotIndex >= itemSlotInfos.Length || itemSlotInfos[dragSeletedSlotIndex] == null)
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

        if (sourceSlotIndex < 0 || sourceSlotIndex >= itemSlotInfos.Length || targetSlotIndex == sourceSlotIndex)
        {
            return;
        }

        // string id로 변경
        if (itemSlotInfos[sourceSlotIndex].itemData != null && itemSlotInfos[sourceSlotIndex].itemData.ID == itemSlotInfos[targetSlotIndex].itemData.ID)
        {
            // 같은 아이템일 경우 합치기
            int totalAmount = itemSlotInfos[targetSlotIndex].Amount + itemSlotInfos[sourceSlotIndex].Amount;

            if (totalAmount <= itemSlotInfos[targetSlotIndex].itemData.MaxStack)
            {
                if (inventoryItemTable.TryGetValue(itemSlotInfos[sourceSlotIndex].itemData.ID, out var itemInfoList))
                {
                    itemInfoList.Remove(itemSlotInfos[sourceSlotIndex]);
                }

                itemSlotInfos[sourceSlotIndex].Amount = totalAmount;
                itemSlotInfos[targetSlotIndex].Empty();
            }
            else
            {
                itemSlotInfos[sourceSlotIndex].Amount = itemSlotInfos[sourceSlotIndex].itemData.MaxStack;
                itemSlotInfos[targetSlotIndex].Amount = totalAmount - itemSlotInfos[sourceSlotIndex].itemData.MaxStack;
            }
        }
        else
        {
            (itemSlotInfos[targetSlotIndex], itemSlotInfos[sourceSlotIndex]) = (itemSlotInfos[sourceSlotIndex], itemSlotInfos[targetSlotIndex]);
            itemSlotInfos[targetSlotIndex].index = targetSlotIndex;
            itemSlotInfos[sourceSlotIndex].index = sourceSlotIndex;
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
        if ((int)ItemData.ConvertEquipmentType(itemSlotInfos[dragSeletedSlotIndex].itemData.ID, itemSlotInfos[dragSeletedSlotIndex].itemData.ItemType) != (int)equipmentSocket.EquipmentType)
        {
            OnEquip();
            return;
        }

        SelectedSlotIndex = dragSeletedSlotIndex;
        OnEquip();
        // equipmentSocket.onChangeEquipEvent?.Invoke(equipmentSocket.eq)
    }

    public void OnSetEquipmentItem(ItemData itemData, int amount)
    {
        itemSlotInfos[dragSeletedSlotIndex].itemData = itemData;
        itemSlotInfos[dragSeletedSlotIndex].Amount = amount;

        if (inventoryItemTable.ContainsKey(itemData.ID))
        {
            inventoryItemTable[itemData.ID].Add(itemSlotInfos[dragSeletedSlotIndex]);
        }
        else
        {
            var itemInfoList = new List<ItemSlotInfo>();
            itemInfoList.Add(itemSlotInfos[dragSeletedSlotIndex]);
            inventoryItemTable.Add(itemData.ID, itemInfoList);
        }

        itemSlots[dragSeletedSlotIndex].SetItemData(itemSlotInfos[dragSeletedSlotIndex]);
        ++useSlotCount;
        dragSeletedSlotIndex = -1;
        isOnDrag = false;
    }

    public void OnUseDurability(int index)
    {
        if(itemSlotInfos[index].Durability <= 0)
        {
            if (inventoryItemTable.TryGetValue(itemSlotInfos[index].itemData.ID, out var itemInfoList))
            {
                itemInfoList.Remove(itemSlotInfos[index]);
            }

            OnCheckEraseGatherItem(itemSlotInfos[index]);
            itemInfoView.OnSetItemSlotInfo(null);
            itemSlotInfos[index].Empty();
            --useSlotCount;

            UpdateSlot(index);
        }
    }

    private void CreateItem(DropItemInfo dropItemInfo, int slotIndex)
    {
        if (slotIndex == -1)
        {
            return;
        }

        itemSlotInfos[slotIndex].Amount = dropItemInfo.amount;
        itemSlotInfos[slotIndex].itemData = dropItemInfo.itemData;
        itemSlotInfos[slotIndex].Durability = dropItemInfo.durability;

        if (inventoryItemTable.ContainsKey(dropItemInfo.id))
        {
            inventoryItemTable[dropItemInfo.id].Add(itemSlotInfos[slotIndex]);
        }
        else
        {
            var itemInfoList = new List<ItemSlotInfo>();
            itemInfoList.Add(itemSlotInfos[slotIndex]);
            inventoryItemTable.Add(dropItemInfo.id, itemInfoList);
        }

        OnCheckAddGatherItem(itemSlotInfos[slotIndex]);


        itemSlots[slotIndex].OnUpdateSlot(); 
        ++useSlotCount;
    }

    private void OnCheckEraseGatherItem(ItemSlotInfo itemSlotInfo)
    {
        if (itemSlotInfo.itemData.ItemType == ItemType.Weapon)
        {
            var weaponData = DataTableManager.WeaponTable.Get(itemSlotInfo.itemData.ID);

            if (weaponData.GatherType != 0)
            {
                int index = weaponData.GatherType - 1;

                if (GatherItemSlotInfoList[index] != null)
                {
                    GatherItemSlotInfoList[index].Remove(itemSlotInfo);
                }
            }
        }
    }

    private void OnCheckAddGatherItem(ItemSlotInfo itemSlotInfo)
    {
        if (itemSlotInfo.itemData.ItemType == ItemType.Weapon)
        {
            var weaponData = DataTableManager.WeaponTable.Get(itemSlotInfo.itemData.ID);

            if (weaponData.GatherType != 0)
            {
                int index = weaponData.GatherType - 1;
                GatherItemSlotInfoList[index].Add(itemSlotInfo);
            }
        }
    }

    public bool IsFullInventory()
    {
        return useSlotCount == maxSlot;
    }

    public bool IsFullInventoryAndMaxStack(int id)
    {
        bool canAdd = false;
        if (inventoryItemTable.ContainsKey(id))
        {
            var itemList = inventoryItemTable[id];

            foreach (var item in itemList)
            {
                if (item.Amount == item.itemData.MaxStack)
                {
                    canAdd = false;
                    continue;
                }
                canAdd = true;
            }
        }
        return IsFullInventory() && canAdd;
    }

    public void OnChangeEquimentItem(ItemData itemData, int amount, int durability)
    {
        var unEquimentInfo = new DropItemInfo();
        unEquimentInfo.amount = amount;
        unEquimentInfo.id = itemData.ID;
        unEquimentInfo.itemData = itemData;
        unEquimentInfo.durability = durability;
        ChangeItem(unEquimentInfo);
    }

    public void Save()
    {
        var itemSlotList = SaveLoadManager.Data.ItemSlotInfoSaveDataList;
        itemSlotList.Clear();

        foreach (var itemInfo in itemSlotInfos)
        {
            var itemInfoSaveData = new ItemInfoSaveData();
            itemInfoSaveData.amount = itemInfo.Amount;
            itemInfoSaveData.itemID = itemInfo.itemData == null ? -1 : itemInfo.itemData.ID;
            itemInfoSaveData.index = itemInfo.index;
            itemInfoSaveData.durability = itemInfo.Durability;
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

            itemSlotInfos[i].Amount = itemSlotInfoList[i].amount;
            itemSlotInfos[i].index = itemSlotInfoList[i].index;
            itemSlotInfos[i].Durability = itemSlotInfoList[i].durability;
            itemSlotInfos[i].itemData = DataTableManager.ItemTable.Get(itemSlotInfoList[i].itemID);


            if (itemSlotInfos[i].itemData == null)
            {
                continue;
            }

            if (inventoryItemTable.ContainsKey(itemSlotInfos[i].itemData.ID))
            {
                inventoryItemTable[itemSlotInfos[i].itemData.ID].Add(itemSlotInfos[i]);
            }
            else
            {
                var itemInfoList = new List<ItemSlotInfo>();
                itemInfoList.Add(itemSlotInfos[i]);
                inventoryItemTable.Add(itemSlotInfos[i].itemData.ID, itemInfoList);
            }

            OnCheckAddGatherItem(itemSlotInfos[i]);
            ++useSlotCount;
        }

        UpdateSlots();
    }
}
