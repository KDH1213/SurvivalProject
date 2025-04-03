using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoUI : MonoBehaviour
{
    [SerializeField]
    private Image objectImage;
    [SerializeField]
    private TextMeshProUGUI objectInfo;
    [SerializeField]
    private GameObject needItemContents;
    private List<BuildInfoUINeedItem> needItems = new List<BuildInfoUINeedItem>();
    [SerializeField]
    private BuildInfoUINeedItem needItemPrefeb;
    [SerializeField]
    private GameObject debuffContents;
    public TestInventory inven;

    public void SetUIInfo(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        PlacementLevelInfo levelInfo = objInfo.LevelList[selectedObject.Level - 1];
        objectImage.sprite = levelInfo.Icon;
        objectInfo.text = $"Name : {levelInfo.Name}\nLevel : {selectedObject.Level}\nFeature : {levelInfo.Feature}";
        foreach (var item in needItems)
        {
            Destroy(item.gameObject);
        }
        needItems.Clear();
        foreach (var item in levelInfo.NeedItems)
        {
            BuildInfoUINeedItem needItem = Instantiate(needItemPrefeb, needItemContents.transform);
            needItem.SetNeedItem(null, objInfo.Kind.ToString(), item.Value, inven.inventory[item.Key]);
            needItems.Add(needItem);
        }
    }

    public void OnCloseWindow()
    {
        gameObject.SetActive(false);
    }
}
