using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmUI : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI featureInfo;
    [SerializeField]
    private List<BuildInfoUINeedItem> needItems;
    [SerializeField]
    private Button interactButton;

    public void SetUIInfo(PlacementSystem system, int id, string kind, int outPut, int maxOutPut)
    {
        int index = system.Database.objects.FindIndex(x => x.ID == id);
        PlacementObjectInfo data = system.Database.objects[index];
        itemImage.sprite = data.Icon;
        featureInfo.text = $"Name : {data.Name}\nLevel : {1}\nKind : {data.Kind}";
        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }

        needItems[0].gameObject.SetActive(true);
        needItems[0].SetNeedItem(null, kind, outPut, maxOutPut);
        needItems.Add(needItems[index]);
        
    }

    private void SetButtonDisable()
    {
        /*if (!inven.CheckItemCount(placementObject.NeedItems))
        {
            interactButton.interactable = false;
            interactButton.onClick.RemoveAllListeners();
        }
        else
        {
            interactButton.interactable = true;
            interactButton.onClick.AddListener(OnCloseWindow);
            interactButton.onClick.AddListener(() => system.StartPlacement(placementObject.ID));
        }*/
    }

    public void OnCloseWindow()
    {
        gameObject.SetActive(false);
    }

}
