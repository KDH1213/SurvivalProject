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

    [SerializeField]
    private Button upgradeButton;

    private int nextStructureId;

    public void SetUIInfo(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        upgradeButton.onClick.RemoveAllListeners();

        int index = 0;
        beforeImage.sprite = objInfo.Icon;
        beforeName.text = $"{objInfo.Name}";

        PlacementObjectInfo nextLevelInfo = objInfo;
        afterImage.sprite = nextLevelInfo.Icon;
        afterName.text = $"{nextLevelInfo.Name}";

        nextStructureId = objInfo.NextStructureID;

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
        var system = selectedObject.uiController.GetComponent<PlacementSystem>();
        upgradeButton.onClick.AddListener(
            () => system.UpgradeStructure(selectedObject, 1602002/*objInfo.NextStructureID*/));
    }

    

}
