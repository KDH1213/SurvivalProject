using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoUI : MonoBehaviour
{
    [SerializeField]
    private PlacementUIController uiController;
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
    [SerializeField]
    private Button upgrade;
    public TestInventory inven;

    public void SetUIInfo(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        int index = 0;
        objectImage.sprite = objInfo.Icon;
        objectInfo.text = $"Name : {objInfo.Name}\nLevel : {selectedObject.Level}\nFeature : {objInfo.Feature}";
        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in objInfo.NeedItems)
        {
            needItems[index].gameObject.SetActive(true);
            needItems[index].SetNeedItem(null, item.Value, inven.inventory[item.Key]);
            needItems.Add(needItems[index]);
            index++;
        }

        upgrade.onClick.AddListener(() => gameObject.SetActive(false));

        

        upgrade.onClick.AddListener(() => uiController.OnOpenUpgradeInfo(objInfo));
    }

    public void OnCloseWindow()
    {
        gameObject.SetActive(false);
    }
}
