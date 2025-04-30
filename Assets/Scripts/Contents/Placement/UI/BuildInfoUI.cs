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
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI hp; 
    [SerializeField]
    private TextMeshProUGUI size;
    [SerializeField]
    private TextMeshProUGUI featureInfo;
    [SerializeField]
    private TextMeshProUGUI structureDescript;
    private PlacementObjectInfo placementObject;
    [SerializeField]
    private List<NeedItem> needItems;
    [SerializeField]
    private List<NeedPenalty> needPenalties;
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

        var stringTable = DataTableManager.StringTable;
        var itemTable = DataTableManager.ItemTable;
        var data = DataTableManager.ConstructionTable.Get(selectedObject.ID);
        var structureData = DataTableManager.StructureTable.Get(selectedObject.ID);

        int index = 0;
        int penaltyIndex = 0;



        placementObject = objInfo;
        itemImage.sprite = objInfo.Icon;
        itemName.text = objInfo.Name;
        size.text = $"{objInfo.Size.x} X {objInfo.Size.y}";
        hp.text = objInfo.DefaultHp.ToString();
        featureInfo.text = stringTable.Get(structureData.DescriptID);

        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var penalty in needPenalties)
        {
            penalty.gameObject.SetActive(false);
        }
        
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
                index++;
            }
        }

        foreach (var item in data.NeedPenalties)
        {
            needPenalties[penaltyIndex].gameObject.SetActive(true);
            needPenalties[penaltyIndex].SetNeedPenalty(item.Key, item.Value);

            penaltyIndex++;
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

    private void SetStructureDescript(StructureKind kind)
    {
        switch (kind)
        {
            case StructureKind.Other:
                break;
            case StructureKind.Farm:
                break;
            case StructureKind.Turret:
                break;
            case StructureKind.Create:
                break;
            case StructureKind.Storage:
                break;
            case StructureKind.Rest:
                break;
        }
    }

}
