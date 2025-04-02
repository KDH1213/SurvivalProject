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
    private GameObject needItemList;
    private List<BuildInfoUINeedItem> needItems = new();
    [SerializeField]
    private BuildInfoUINeedItem needItemPrefeb;
    [SerializeField]
    private Button placeButton;
    public TestInventory inven;

    public void SetUIInfo(PlacementObjectInfo objInfo)
    {
        placementObject = objInfo;
        PlacementLevelInfo levelInfo = objInfo.LevelList[0];
        itemImage.sprite = levelInfo.Icon;
        featureInfo.text = $"Name : {levelInfo.Name}\nLevel : {1}\nFeature : {levelInfo.Feature}";
        foreach (var item in needItems)
        {
            Destroy(item.gameObject);
        }
        needItems.Clear();
        foreach (var item in levelInfo.NeedItems)
        {
            BuildInfoUINeedItem needItem = Instantiate(needItemPrefeb, needItemList.transform);
            needItem.SetNeedItem(null, objInfo.Kind.ToString(), item.Value, inven.inventory[item.Key]);
            needItems.Add(needItem);
        }
        SetButtonDisable();
        
    }

    private void SetButtonDisable()
    {
        if (!inven.CheckItemCount(placementObject.LevelList[0].NeedItems))
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
