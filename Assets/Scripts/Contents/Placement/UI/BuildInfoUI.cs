using System;
using System.Collections;
using System.Collections.Generic;
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
    private List<BuildInfoUINeedItem> needItems;
    [SerializeField]
    private BuildInfoUINeedItem needItemPrefeb;
    [SerializeField]
    private Button placeButton;
    public TestInventory inven;

    public void SetUIInfo(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        int index = 0;
        placementObject = objInfo;
        itemImage.sprite = objInfo.Icon;
        featureInfo.text = $"Name : {objInfo.Name}\nLevel : {1}\nFeature : {objInfo.Feature}";
        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in objInfo.NeedItems)
        {
            needItems[index].gameObject.SetActive(true);
            if(inven.inventory.ContainsKey(item.Key))
            {
                needItems[index].SetNeedItem(null, item.Key, item.Value, inven.inventory[item.Key]);
            }
            else
            {
                needItems[index].SetNeedItem(null, item.Key, item.Value, 0);
            }
            
            needItems.Add(needItems[index]);
            index++;
        }
        SetButtonDisable();
        
    }

    private void SetButtonDisable()
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

    public void OnCloseWindow()
    {
        gameObject.SetActive(false);
    }

}
