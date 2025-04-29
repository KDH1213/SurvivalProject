using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildInfoUI : MonoBehaviour
{
    [SerializeField]
    private PlacementSystem system;
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI featureInfo;
    private PlacementObjectInfo placementObject;
    [SerializeField]
    private List<NeedItem> needItems;
    [SerializeField]
    private NeedItem needItemPrefeb;
    [SerializeField]
    private Button placeButton;
    public TestInventory inven;
    private Inventory inventory;

    public void SetUIInfo(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            inventory = GameObject.FindWithTag("Player").GetComponent<PlayerFSM>().PlayerInventory;
        }

        //var stringTable = DataTableManager.

        int index = 0;
        placementObject = objInfo;
        itemImage.sprite = objInfo.Icon;
        featureInfo.text = $"Name : {objInfo.Name}\nLevel : {objInfo.Rank}\nFeature : {objInfo.Feature}";
        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }

        var itemTable = DataTableManager.ItemTable;

        if (inventory == null)
        {
            foreach (var item in objInfo.NeedItems)
            {
                needItems[index].gameObject.SetActive(true);
                if (inven.inventory.ContainsKey(item.Key))
                {
                    needItems[index].SetNeedItem(itemTable.Get(item.Key).ItemImage, 
                        item.Value, inven.inventory[item.Key]);
                }
                else
                {
                    needItems[index].SetNeedItem(itemTable.Get(item.Key).ItemImage, 
                        item.Value, inven.inventory[item.Key]);
                }

                needItems.Add(needItems[index]);
                index++;
            }
        }
        else
        {
            foreach (var item in objInfo.NeedItems)
            {
                needItems[index].gameObject.SetActive(true);
                if (inventory.GetTotalItem(item.Key) < item.Value)
                {
                    needItems[index].SetNeedItem(itemTable.Get(item.Key).ItemImage, 
                        item.Value, inventory.GetTotalItem(item.Key) + 
                        system.Storages.Sum(storage => storage.inventory.GetTotalItem(item.Key)));
                }
                else
                {
                    needItems[index].SetNeedItem(itemTable.Get(item.Key).ItemImage, 
                        item.Value, inventory.GetTotalItem(item.Key) +
                        system.Storages.Sum(storage => storage.inventory.GetTotalItem(item.Key)));
                }

                needItems.Add(needItems[index]);
                index++;
            }
        }
        SetButtonDisable();
        
    }

    private void SetButtonDisable()
    {
        if (inventory != null)
        {
            if (CanPlaced())
            {
                placeButton.interactable = true;
                placeButton.onClick.AddListener(OnCloseWindow);
                placeButton.onClick.AddListener(() => system.StartPlacement(placementObject.ID));
            }
            else
            {
                placeButton.interactable = false;
                placeButton.onClick.RemoveAllListeners();
            }
        }
        else 
        { 
            if (!inven.CheckItemCount(placementObject.NeedItems))
            {
                placeButton.interactable = false;
                placeButton.onClick.RemoveAllListeners();
            }
            else
            {
                placeButton.interactable = true;
                placeButton.onClick.AddListener(OnCloseWindow);
                placeButton.onClick.AddListener(() => system.StartPlacement(placementObject.ID));
            }
        }
    }

    public void OnCloseWindow()
    {
        gameObject.SetActive(false);
    }

    private bool CanPlaced()
    {
        foreach (var data in placementObject.NeedItems)
        {
            if (inventory == null)
                break;
            if (inventory.GetTotalItem(data.Key) < data.Value && 
                system.Storages.Sum(storage => storage.inventory.GetTotalItem(data.Key)) == data.Value)
            {
                return false;
            }
        }
        return true;
    }

}
