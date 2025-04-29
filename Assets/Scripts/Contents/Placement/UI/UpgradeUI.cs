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
    private List<NeedItem> needItems = new List<NeedItem>();
    public TestInventory inven;

    [SerializeField]
    private GameObject deleteArrow;
    [SerializeField]
    private GameObject deleteRight;

    [SerializeField]
    private Button upgradeButton;
    private Inventory inventory;

    public void SetUIInfo(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        var system = selectedObject.uiController.GetComponent<PlacementSystem>();
        if (GameObject.FindWithTag("Player") != null)
        {
            inventory = GameObject.FindWithTag("Player").GetComponent<PlayerFSM>().PlayerInventory;
        }
        upgradeButton.onClick.RemoveAllListeners();

        int index = 0;
        beforeImage.sprite = objInfo.Icon;
        beforeName.text = $"{objInfo.Name}";

        if(objInfo.NextStructureID == 0)
        {
            SetMaxUpgrade();
            return;
        }

        int nextIndex = system.Database.objects.FindIndex(data => data.ID == objInfo.NextStructureID);
        PlacementObjectInfo nextLevelInfo = system.Database.objects[nextIndex];
        afterImage.sprite = nextLevelInfo.Icon;
        afterName.text = $"{nextLevelInfo.Name}";

        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }

        var itemTable = DataTableManager.ItemTable;
        foreach (var item in objInfo.NeedItems)
        {
            needItems[index].gameObject.SetActive(true);
            if (inventory == null)
            {
                needItems[index].SetNeedItem(itemTable.Get(item.Key).ItemImage,
                    item.Value, inven.inventory[item.Key]);
            }
            else
            {
                needItems[index].SetNeedItem(itemTable.Get(item.Key).ItemImage,
                    item.Value, inventory.GetTotalItem(item.Key));
            }
            needItems.Add(needItems[index]);
            index++;
        }

        SetButtonDisable(objInfo.NeedItems);
        upgradeButton.onClick.AddListener(
            () => ConsumItem(system.Database.objects[index].NeedItems));
        upgradeButton.onClick.AddListener(
            () => system.UpgradeStructure(selectedObject, objInfo.NextStructureID));
    }

    private void SetMaxUpgrade()
    {
        deleteArrow.SetActive(false);
        deleteRight.SetActive(false);
        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }
        upgradeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Max";
        upgradeButton.interactable = false;
    }

    private void ConsumItem(Dictionary<int, int> needItems)
    {
        foreach (var data in needItems)
        {
            if (inventory == null)
                break;
            inventory.ConsumeItem(data.Key, data.Value);
        }
    }

    private void ResetUpgrade()
    {
        deleteArrow.SetActive(true);
        deleteRight.SetActive(true);
        upgradeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Upgrade";
        upgradeButton.interactable = true;
    }

    private void SetButtonDisable(Dictionary<int, int> needItems)
    {
        if (inventory != null)
        {
            if (CanUpgrade(needItems))
            {
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeButton.interactable = false;
            }
        }
        else
        {
            if (!inven.CheckItemCount(needItems))
            {
                upgradeButton.interactable = false;
            }
            else
            {
                upgradeButton.interactable = true;
            }
        }

    }

    private bool CanUpgrade(Dictionary<int, int> needItems)
    {
        foreach (var data in needItems)
        {
            if (inventory == null)
                break;
            if (inventory.GetTotalItem(data.Key) < data.Value)
            {
                return false;
            }
        }
        return true;
    }
}
