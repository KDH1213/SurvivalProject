using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeFarmUI : MonoBehaviour
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
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI beforeOutPut;
    [SerializeField]
    private TextMeshProUGUI beforeProduceTime;
    [SerializeField]
    private TextMeshProUGUI afterOutPut;
    [SerializeField]
    private TextMeshProUGUI afterProduceTime;
    [SerializeField]
    private List<BuildInfoUINeedItem> needItems = new List<BuildInfoUINeedItem>();
    public TestInventory inven;
    private Inventory inventory;

    [SerializeField]
    private Button upgradeButton;

    private int nextStructureId;

    [SerializeField]
    private GameObject deleteArrow;
    [SerializeField]
    private GameObject deleteRight;
    [SerializeField]
    private GameObject deleteItemList;
    [SerializeField]
    private GameObject deleteChangeInfo;
    [SerializeField]
    private GameObject deleteItemListTitle;

    public void SetUIInfo(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {

        ResetInfo();
        var system = selectedObject.uiController.GetComponent<PlacementSystem>();
        if (GameObject.FindWithTag("Player") != null)
        {
            inventory = GameObject.FindWithTag("Player").GetComponent<PlayerFSM>().PlayerInventory;
        }
        
        var data = DataTableManager.StructureTable.Get(objInfo.ID);

        int index = system.Database.objects.FindIndex(data => data.ID == objInfo.NextStructureID);
        beforeImage.sprite = objInfo.Icon;
        beforeName.text = $"{objInfo.Name}";
        if (objInfo.NextStructureID == 0)
        {
            SetMaxUpgrade();
            return;
        }

        PlacementObjectInfo nextLevelInfo = system.Database.objects[index];
        afterImage.sprite = nextLevelInfo.Icon;
        afterName.text = $"{nextLevelInfo.Name}";

        Sprite sprite = DataTableManager.ItemTable.Get(data.ItemToProduce).ItemImage;
        itemImage.sprite = sprite;
        beforeOutPut.text = $"{data.AmountPerProduction} 개";
        beforeProduceTime.text = $"{data.ProductionCycle} 초";

        var nextData = DataTableManager.StructureTable.Get(objInfo.NextStructureID);
        afterOutPut.text = $"{nextData.AmountPerProduction} 개";
        afterProduceTime.text = $"{nextData.ProductionCycle} 초";

        int needItemIndex = 0;

        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in objInfo.NeedItems)
        {
            needItems[needItemIndex].gameObject.SetActive(true);
            if(inventory == null)
            {
                needItems[needItemIndex].SetNeedItem(null, item.Value, inven.inventory[item.Key]);
            }
            else
            {
                needItems[needItemIndex].SetNeedItem(null, item.Value, inventory.GetTotalItem(item.Key));
            }
            needItems.Add(needItems[needItemIndex]);
            needItemIndex++;
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
        deleteItemList.SetActive(false);
        deleteChangeInfo.SetActive(false);
        deleteItemListTitle.SetActive(false);
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

    private void ResetInfo()
    {
        upgradeButton.onClick.RemoveAllListeners();
        beforeImage.sprite = null;
        beforeName.text = null;
        afterImage.sprite = null;
        afterName.text = null;
        itemImage.sprite = null;
        beforeOutPut.text = null;
        beforeProduceTime.text = null;
        afterOutPut.text = null;
        afterProduceTime.text = null;

        deleteArrow.SetActive(true);
        deleteRight.SetActive(true);
        deleteItemList.SetActive(true);
        deleteChangeInfo.SetActive(true);
        deleteItemListTitle.SetActive(true);
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
