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

        PlacementObjectInfo nextLevelInfo = system.Database.objects[index];
        afterImage.sprite = nextLevelInfo.Icon;
        afterName.text = $"{nextLevelInfo.Name}";

        Sprite sprite = DataTableManager.ItemTable.Get(data.ItemToProduce).ItemImage;
        itemImage.sprite = sprite;
        beforeOutPut.text = $"{data.AmountPerProduction} 개";
        beforeProduceTime.text = $"{data.ProductionCycle} 초";

        var nextData = DataTableManager.StructureTable.Get(data.UpgradeID);
        afterOutPut.text = $"{nextData.AmountPerProduction} 개";
        afterProduceTime.text = $"{nextData.ProductionCycle} 초";

        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in objInfo.NeedItems)
        {
            needItems[index].gameObject.SetActive(true);
            if(inventory == null)
            {
                needItems[index].SetNeedItem(null, item.Key, item.Value, inven.inventory[item.Key]);
            }
            else
            {
                needItems[index].SetNeedItem(null, item.Key, item.Value, inventory.GetTotalItem(item.Key));
            }
            needItems.Add(needItems[index]);
            index++;
        }

        upgradeButton.onClick.AddListener(
            () => ConsumItem(system.Database.objects[index].NeedItems));
        upgradeButton.onClick.AddListener(
            () => system.UpgradeStructure(selectedObject, objInfo.NextStructureID));
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
}
}
