using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StorageInventory : MonoBehaviour
{
    private ItemSlotInfo[] itemInfos;
    private Dictionary<int, List<ItemSlotInfo>> inventoryItemTable = new Dictionary<int, List<ItemSlotInfo>>();

    public Dictionary<int, List<ItemSlotInfo>> InventroyItemTable => inventoryItemTable;
    public ItemSlotInfo[] ItemInfos => itemInfos;
    private int useSlotCount = 0;
    public void Initialize()
    {
        /*if (SaveLoadManager.Data != null)
        {
            Load();
        }*/

    }

    public void SetItemInfos(int count)
    {
        
        itemInfos = new ItemSlotInfo[count];
        for (int i = 0; i < count; i++) 
        {
            itemInfos[i] = new ItemSlotInfo();
            itemInfos[i].index = i;
        }
    }

    public int GetTotalItem(int id)
    {
        int count = 0;
        if (inventoryItemTable.ContainsKey(id))
        {
            var itemList = inventoryItemTable[id];
            foreach (var item in itemList)
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

    public void OnEraseItem(int selectedIndex)
    {
        if (selectedIndex == -1 || itemInfos[selectedIndex].itemData == null)
        {
            return;
        }

        if (inventoryItemTable.TryGetValue(itemInfos[selectedIndex].itemData.ID, out var itemInfoList))
        {
            itemInfoList.Remove(itemInfos[selectedIndex]);
            itemInfos[selectedIndex].Empty();
            --useSlotCount;
        }
    }

    private int FindEmptySlotIndex()
    {
        foreach (var info in itemInfos)
        {
            if (info.itemData == null)
            {
                return info.index;
            }
        }

        return -1;
    }

    // string으로 찾는 경우 추 후 아이템 id로 변경
    public void AddItem(DropItemInfo dropItemInfo)
    {
        if (inventoryItemTable.ContainsKey(dropItemInfo.id))
        {
            var itemList = inventoryItemTable[dropItemInfo.id];

            foreach (var item in itemList)
            {
                int addUseCount = (item.itemData.MaxStack - item.Amount);
                if (dropItemInfo.amount <= addUseCount)
                {
                    item.Amount += dropItemInfo.amount;
                    dropItemInfo.amount = 0;
                    break;
                }
                else
                {
                    item.Amount = item.itemData.MaxStack;
                    dropItemInfo.amount -= addUseCount;
                }

            }

            if (dropItemInfo.amount > 0 && useSlotCount < ItemInfos.Length)
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

            if (slotIndex == -1)
            {
                return;
            }
            CreateItem(dropItemInfo, slotIndex);
        }
    }
    public void ConsumeItem(int id, int count)
    {
        if (inventoryItemTable.ContainsKey(id))
        {
            var itemList = inventoryItemTable[id].OrderBy(item => item.Amount).
                ThenByDescending(item => item.index).ToList();
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].Amount >= count)
                {
                    itemList[i].Amount -= count;
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
                }

            }
        }
        else
        {
            new KeyNotFoundException($"키를 찾을 수 없습니다. {id}");
        }
    }
    private void CreateItem(DropItemInfo dropItemInfo, int slotIndex)
    {
        var itemInfo = new ItemSlotInfo();
        itemInfo.index = slotIndex;
        itemInfo.Amount = dropItemInfo.amount;
        // TODO :: 임시 코드
        itemInfo.itemData = dropItemInfo.itemData;

        if (inventoryItemTable.ContainsKey(dropItemInfo.id))
        {
            inventoryItemTable[dropItemInfo.id].Add(itemInfo);
        }
        else
        {
            var itemInfoList = new List<ItemSlotInfo>();
            itemInfoList.Add(itemInfo);
            inventoryItemTable.Add(dropItemInfo.id, itemInfoList);
        }

        if (slotIndex == -1)
        {
            return;
        }

        itemInfos[slotIndex] = itemInfo;
        ++useSlotCount;
    }

    public bool IsFullInventory()
    {
        return useSlotCount == itemInfos.Length;
    }

    public void Save(int index)
    {
        List<ItemInfoSaveData> itemSlotList = new List<ItemInfoSaveData>();

        foreach (var itemInfo in itemInfos)
        {
            var itemInfoSaveData = new ItemInfoSaveData();
            itemInfoSaveData.amount = itemInfo.Amount;
            itemInfoSaveData.itemID = itemInfo.itemData == null ? -1 : itemInfo.itemData.ID;
            itemInfoSaveData.index = itemInfo.index;
            itemInfoSaveData.durability = itemInfo.Durability;
            itemSlotList.Add(itemInfoSaveData);
        }

        SaveLoadManager.Data.storagePlacementSaveInfo[index].itemDatas = itemSlotList;
    }

    public void Load(int id)
    {
        if (SaveLoadManager.Data.isRestart)
        {
            return;
        }

        var itemSlotInfoList = SaveLoadManager.Data.storagePlacementSaveInfo[id].itemDatas;
        if(itemSlotInfoList == null)
        {
            return;
        }

        for (var i = 0; i < itemSlotInfoList.Count; ++i)
        {
            if (itemSlotInfoList[i].itemID == -1)
            {
                continue;
            }

            itemInfos[i].Amount = itemSlotInfoList[i].amount;
            itemInfos[i].index = itemSlotInfoList[i].index;
            itemInfos[i].Durability = itemSlotInfoList[i].durability;
            itemInfos[i].itemData = DataTableManager.ItemTable.Get(itemSlotInfoList[i].itemID);


            if (itemInfos[i].itemData == null)
            {
                continue;
            }

            if (inventoryItemTable.ContainsKey(itemInfos[i].itemData.ID))
            {
                inventoryItemTable[itemInfos[i].itemData.ID].Add(itemInfos[i]);
            }
            else
            {
                var itemInfoList = new List<ItemSlotInfo>();
                itemInfoList.Add(itemInfos[i]);
                inventoryItemTable.Add(itemInfos[i].itemData.ID, itemInfoList);
            }

            ++useSlotCount;
        }

    }
}
