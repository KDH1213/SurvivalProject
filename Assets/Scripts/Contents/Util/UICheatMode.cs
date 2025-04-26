using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICheatMode : MonoBehaviour
{
    [SerializeField]
    private UICheatModeItemData itemPanelPrefab;

    [SerializeField]
    private Transform createPoint;

    [SerializeField]
    private Inventory playerInentory;

    private void Awake()
    {
        var itemValues = DataTableManager.ItemTable.ItemDataTable.Values;

        foreach (var item in itemValues)
        {
            var uICheatModeItemData = Instantiate(itemPanelPrefab, createPoint);
            uICheatModeItemData.SetItemData(item);
            uICheatModeItemData.onCreateAction += OnCreateItem;
        }
    }

    public void OnCreateItem(DropItemInfo dropItemInfo)
    {
        playerInentory.AddItem(dropItemInfo);
    }

    //public void OnCreateItem()
    //{
    //    if(int.TryParse(idField.text, out var id))
    //    {
    //        var itemData = DataTableManager.ItemTable.Get(id);

    //        if(itemData != null)
    //        {
    //            DropItemInfo dropItemInfo = new DropItemInfo();
    //            dropItemInfo.id = id;
    //            dropItemInfo.itemData = itemData;
    //            dropItemInfo.amount = 1;

    //            playerInentory.AddItem(dropItemInfo);
    //        }
    //    }
    //}
}
