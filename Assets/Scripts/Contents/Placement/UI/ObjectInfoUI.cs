using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoUI : MonoBehaviour
{
    [SerializeField]
    private Image objectImage;
    [SerializeField]
    private TextMeshProUGUI objectInfo;
    [SerializeField]
    private List<BuildInfoUINeedItem> needItems = new List<BuildInfoUINeedItem>();
    [SerializeField]
    private BuildInfoUINeedItem needItemPrefeb;
    [SerializeField]
    private GameObject debuffContents;
    public TestInventory inven;

    public void SetUIInfo(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        int index = 0;
        PlacementLevelInfo levelInfo = objInfo.LevelList[selectedObject.Level - 1];
        objectImage.sprite = levelInfo.Icon;
        objectInfo.text = $"Name : {levelInfo.Name}\nLevel : {selectedObject.Level}\nFeature : {levelInfo.Feature}";
        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in levelInfo.NeedItems)
        {
            needItems[index].gameObject.SetActive(true);
            needItems[index].SetNeedItem(null, item.Key, item.Value, inven.inventory[item.Key]);
            needItems.Add(needItems[index]);
            index++;
        }
    }

    public void OnCloseWindow()
    {
        gameObject.SetActive(false);
    }
}
