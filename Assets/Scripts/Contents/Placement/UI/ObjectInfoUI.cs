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

    private Inventory inventory;

    private void Awake()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            inventory = GameObject.FindWithTag("Player").GetComponent<PlayerFSM>().PlayerInventory;
        }
    }
    public void SetUIInfo(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        int index = 0;
        objectImage.sprite = objInfo.Icon;
        objectInfo.text = $"Name : {objInfo.Name}\nLevel : {selectedObject.Level}\nFeature : {objInfo.Feature}";
        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }

        if (inventory != null)
        {
            foreach (var itemData in objInfo.NeedItems)
            {
                var item = DataTableManager.ItemTable.Get(itemData.Key);
                needItems[index].gameObject.SetActive(true);
                needItems[index].SetNeedItem(item.ItemImage, itemData.Value, inventory.GetTotalItem(itemData.Key));
                index++;
            }
        }
        else
        {
            foreach (var itemData in objInfo.NeedItems)
            {
                var item = DataTableManager.ItemTable.Get(itemData.Key);
                needItems[index].gameObject.SetActive(true);
                needItems[index].SetNeedItem(item.ItemImage, itemData.Value, inven.inventory[itemData.Key]);
                index++;
            }

        }

            upgrade.onClick.AddListener(() => gameObject.SetActive(false));

        

        upgrade.onClick.AddListener(() => uiController.OnOpenUpgradeInfo(objInfo));
    }

    public void OnCloseWindow()
    {
        gameObject.SetActive(false);
    }
}
