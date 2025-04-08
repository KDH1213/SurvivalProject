using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField]
    private Image beforeImage;
    [SerializeField]
    private TextMeshProUGUI beforeName;
    [SerializeField]
    private Image afterImage;
    [SerializeField]
    private TextMeshProUGUI afterName;
    [SerializeField]
    private List<BuildInfoUINeedItem> needItems = new List<BuildInfoUINeedItem>();
    public TestInventory inven;

    public void SetUIInfo(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        int index = 0;
        PlacementLevelInfo levelInfo = objInfo.LevelList[selectedObject.Level - 1];
        beforeImage.sprite = levelInfo.Icon;
        beforeName.text = $"{levelInfo.Name}";

        PlacementLevelInfo nextLevelInfo = objInfo.LevelList[selectedObject.Level];
        afterImage.sprite = nextLevelInfo.Icon;
        afterName.text = $"{nextLevelInfo.Name}";

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

}
