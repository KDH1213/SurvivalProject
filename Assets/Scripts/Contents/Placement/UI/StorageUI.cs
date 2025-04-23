using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    private GameObject target;

    [field: SerializeField]
    public GameObject PrivewIcon { get; private set; }
    [SerializeField]
    private ItemSlot slotPrefab;
    [SerializeField]
    private Transform inventorySlotParent; 
    [SerializeField]
    private Transform storageSlotParent;

    [SerializeField]
    private StorageItemInfo itemInfoView;

    private List<ItemSlot> inventoryItemSlots = new List<ItemSlot>();
    private List<ItemSlot> storageItemSlots = new List<ItemSlot>();

    private ItemInfo[] inventoryItemInfos;
    private ItemInfo[] storageItemInfos;

    private StorageStructure currentStructure;

    public int SelectedSlotIndex { get; private set; } = -1;
    private int dragSeletedSlotIndex;
    private bool isOnDrag = false;
    public void SetUI(GameObject target, StorageStructure structure)
    {
        this.target = target;
        currentStructure = structure;
        inventoryItemInfos = target.GetComponent<PlayerFSM>().PlayerInventory.ItemInfos;
        storageItemInfos = structure.inventory.ItemInfos;
        SetInventory(inventoryItemInfos, inventoryItemSlots, inventorySlotParent);
        SetInventory(storageItemInfos, storageItemSlots, storageSlotParent);
    }

    private void SetInventory(ItemInfo[] itemInfos, List<ItemSlot> itemSlots, Transform parent)
    {
        itemSlots = new List<ItemSlot>();
        for (int i = 0; i < itemInfos.Length; i++) 
        {
            var slot = Instantiate(slotPrefab, parent);
            slot.SlotIndex = i;
            slot.button.onClick.AddListener(() =>
            {
                SelectedSlotIndex = slot.SlotIndex;
                slot.OnClickSlot();
                itemInfoView.OnSetItemSlotInfo(itemInfos[SelectedSlotIndex].itemData);
            });

            slot.OnDragEvent.AddListener((position) => { if (PrivewIcon != null) PrivewIcon.transform.position = position; });

            slot.onDragEnter.AddListener(() =>
            {
                isOnDrag = true;
                dragSeletedSlotIndex = slot.SlotIndex;

                if (PrivewIcon != null && slot.ItemData != null)
                {
                    PrivewIcon.SetActive(true);
                    PrivewIcon.GetComponent<Image>().sprite = slot.ItemData.ItemImage;
                }
            });

            if (parent == inventorySlotParent)
            {
                slot.onDragExit.AddListener(OnEndDrag);
            }
            else
            {
                slot.onDragExit.AddListener(OnEndDragStorage);
            }
            //slot.onDoubleClickEvent.AddListener(OnEquip);
            
            itemSlots.Add(slot);
        }
        UpdateSlots(itemInfos, itemSlots);

        if (parent == inventorySlotParent)
        {
            inventoryItemSlots = itemSlots;
        }
        else
        {
            storageItemSlots = itemSlots;
        }
        
    }

    private void UpdateSlots(ItemInfo[] items, List<ItemSlot> itemSlots)
    {
        for (int i = 0; i < items.Length; ++i)
        {
            itemSlots[i].SetItemData(items[i]);
        }
    }

    public void UpdateSlot(List<ItemSlot> itemSlots,int slotIndex)
    {
        itemSlots[slotIndex].OnUpdateSlot();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (PrivewIcon != null)
        {
            PrivewIcon.SetActive(false);
        }

        if (dragSeletedSlotIndex < 0 || dragSeletedSlotIndex >= inventoryItemInfos.Length || inventoryItemInfos[dragSeletedSlotIndex] == null)
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

            if (uiItemSlot == null)
            {
                continue;
            }

            if (uiItemSlot.transform.parent == storageSlotParent)
            {
                OnEndDragTargetToOtherSlot(inventoryItemInfos, storageItemInfos, inventoryItemSlots,
                    storageItemSlots, uiItemSlot);
                continue;
            }

            if (uiItemSlot != null)
            {
                OnEndDragTargetToItemSlot(uiItemSlot);
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

        if (sourceSlotIndex < 0 || sourceSlotIndex >= inventoryItemInfos.Length || targetSlotIndex == sourceSlotIndex)
        {
            return;
        }

        // string id로 변경
        if (inventoryItemInfos[sourceSlotIndex].itemData != null 
            && inventoryItemInfos[sourceSlotIndex].itemData.ID == inventoryItemInfos[targetSlotIndex].itemData.ID)
        {
            // 같은 아이템일 경우 합치기
            int totalAmount = inventoryItemInfos[targetSlotIndex].Amount + inventoryItemInfos[sourceSlotIndex].Amount;

            if (totalAmount <= inventoryItemInfos[targetSlotIndex].itemData.MaxStack)
            {
                var table = target.GetComponent<PlayerFSM>().PlayerInventory.InventroyItemTable;
                if (table.TryGetValue(inventoryItemInfos[sourceSlotIndex].itemData.ID, out var itemInfoList))
                {
                    itemInfoList.Remove(inventoryItemInfos[sourceSlotIndex]);
                }

                inventoryItemInfos[sourceSlotIndex].Amount = totalAmount;
                inventoryItemInfos[targetSlotIndex].Empty();
            }
            else
            {
                inventoryItemInfos[sourceSlotIndex].Amount = inventoryItemInfos[sourceSlotIndex].itemData.MaxStack;
                inventoryItemInfos[targetSlotIndex].Amount = totalAmount - inventoryItemInfos[sourceSlotIndex].itemData.MaxStack;
            }
        }
        else
        {
            (inventoryItemInfos[targetSlotIndex], inventoryItemInfos[sourceSlotIndex]) = (inventoryItemInfos[sourceSlotIndex], inventoryItemInfos[targetSlotIndex]);
            inventoryItemInfos[targetSlotIndex].index = targetSlotIndex;
            inventoryItemInfos[sourceSlotIndex].index = sourceSlotIndex;
            inventoryItemSlots[targetSlotIndex].OnSwapItemInfo(inventoryItemSlots[sourceSlotIndex]);
            // itemInfos[targetSlotIndex].OnSwapItemInfo(itemInfos[sourceSlotIndex]);
        }

        UpdateSlot(inventoryItemSlots, targetSlotIndex);
        UpdateSlot(inventoryItemSlots, sourceSlotIndex);
        isOnDrag = false;
        dragSeletedSlotIndex = -1;
    }

    public void OnEndDragStorage(PointerEventData eventData)
    {
        if (PrivewIcon != null)
        {
            PrivewIcon.SetActive(false);
        }

        if (dragSeletedSlotIndex < 0 || dragSeletedSlotIndex >= storageItemInfos.Length || storageItemInfos[dragSeletedSlotIndex] == null)
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

            if (uiItemSlot == null)
            {
                continue;
            }

            if (uiItemSlot.transform.parent == inventorySlotParent)
            {
                OnEndDragTargetToOtherSlot(storageItemInfos, inventoryItemInfos, storageItemSlots,
                    inventoryItemSlots, uiItemSlot);
                continue;
            }

            if (uiItemSlot != null)
            {
                OnEndDragTargetToStorageItemSlot(uiItemSlot);
            }

            break;
        }

        // 드래그 해제 후에도 아이템이 제대로 이동되지 않았다면 초기화
        isOnDrag = false;

        SelectedSlotIndex = -1;
        dragSeletedSlotIndex = -1;
    }

    private void OnEndDragTargetToStorageItemSlot(ItemSlot itemSlot)
    {
        int targetSlotIndex = dragSeletedSlotIndex;
        int sourceSlotIndex = itemSlot.SlotIndex;

        if (sourceSlotIndex < 0 || sourceSlotIndex >= storageItemInfos.Length || targetSlotIndex == sourceSlotIndex)
        {
            return;
        }

        // string id로 변경
        if (storageItemInfos[sourceSlotIndex].itemData != null
            && storageItemInfos[sourceSlotIndex].itemData.ID == storageItemInfos[targetSlotIndex].itemData.ID)
        {
            // 같은 아이템일 경우 합치기
            int totalAmount = storageItemInfos[targetSlotIndex].Amount + storageItemInfos[sourceSlotIndex].Amount;

            if (totalAmount <= storageItemInfos[targetSlotIndex].itemData.MaxStack)
            {
                var table = target.GetComponent<PlayerFSM>().PlayerInventory.InventroyItemTable;
                if (table.TryGetValue(storageItemInfos[sourceSlotIndex].itemData.ID, out var itemInfoList))
                {
                    itemInfoList.Remove(storageItemInfos[sourceSlotIndex]);
                }

                storageItemInfos[sourceSlotIndex].Amount = totalAmount;
                storageItemInfos[targetSlotIndex].Empty();
            }
            else
            {
                storageItemInfos[sourceSlotIndex].Amount = storageItemInfos[sourceSlotIndex].itemData.MaxStack;
                storageItemInfos[targetSlotIndex].Amount = totalAmount - storageItemInfos[sourceSlotIndex].itemData.MaxStack;
            }
        }
        else
        {
            (storageItemInfos[targetSlotIndex], storageItemInfos[sourceSlotIndex]) = (storageItemInfos[sourceSlotIndex], inventoryItemInfos[targetSlotIndex]);
            storageItemInfos[targetSlotIndex].index = targetSlotIndex;
            storageItemInfos[sourceSlotIndex].index = sourceSlotIndex;
            storageItemSlots[targetSlotIndex].OnSwapItemInfo(storageItemSlots[sourceSlotIndex]);
            // itemInfos[targetSlotIndex].OnSwapItemInfo(itemInfos[sourceSlotIndex]);
        }

        UpdateSlot(storageItemSlots, targetSlotIndex);
        UpdateSlot(storageItemSlots, sourceSlotIndex);
        isOnDrag = false;
        dragSeletedSlotIndex = -1;
    }

    private void OnEndDragTargetToOtherSlot(ItemInfo[] sourceInfo, ItemInfo[] targetInfo, 
        List<ItemSlot> sourceSlots, List<ItemSlot> targetSlots, ItemSlot itemSlot)
    {
        int sourceSlotIndex = dragSeletedSlotIndex;
        int targetSlotIndex = itemSlot.SlotIndex;

        if (sourceSlotIndex < 0 || sourceSlotIndex >= sourceInfo.Length )
        {
            return;
        }

        // string id로 변경
        if (sourceInfo[sourceSlotIndex].itemData != null && targetInfo[targetSlotIndex].itemData != null 
            && sourceInfo[sourceSlotIndex].itemData.ID == targetInfo[targetSlotIndex].itemData.ID)
        {
            // 같은 아이템일 경우 합치기
            int totalAmount = targetInfo[targetSlotIndex].Amount + sourceInfo[sourceSlotIndex].Amount;

            if (totalAmount <= targetInfo[targetSlotIndex].itemData.MaxStack)
            {
                var table = target.GetComponent<PlayerFSM>().PlayerInventory.InventroyItemTable;
                if (table.TryGetValue(sourceInfo[sourceSlotIndex].itemData.ID, out var itemInfoList))
                {
                    itemInfoList.Remove(sourceInfo[sourceSlotIndex]);
                }

                sourceInfo[sourceSlotIndex].Amount = totalAmount;
                targetInfo[targetSlotIndex].Empty();
            }
            else
            {
                sourceInfo[sourceSlotIndex].Amount = sourceInfo[sourceSlotIndex].itemData.MaxStack;
                targetInfo[targetSlotIndex].Amount = totalAmount - sourceInfo[sourceSlotIndex].itemData.MaxStack;
            }
        }
        else
        {
            (targetInfo[targetSlotIndex], sourceInfo[sourceSlotIndex]) = (sourceInfo[sourceSlotIndex], targetInfo[targetSlotIndex]);
            targetInfo[targetSlotIndex].index = targetSlotIndex;
            sourceInfo[sourceSlotIndex].index = sourceSlotIndex;
            // targetSlots[targetSlotIndex].OnSwapItemInfo(sourceSlots[sourceSlotIndex]);
            // itemInfos[targetSlotIndex].OnSwapItemInfo(itemInfos[sourceSlotIndex]);
        }

        UpdateSlots(targetInfo, targetSlots);
        UpdateSlots(sourceInfo, sourceSlots);
        isOnDrag = false;
        dragSeletedSlotIndex = -1;
    }

}
