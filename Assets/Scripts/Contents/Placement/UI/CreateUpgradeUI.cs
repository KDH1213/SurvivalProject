using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateUpgradeUI : MonoBehaviour
{
    [SerializeField]
    private Image beforeImage;
    [SerializeField]
    private TextMeshProUGUI beforeName;
    [SerializeField]
    private TextMeshProUGUI beforeHp;
    [SerializeField]
    private Image afterImage;
    [SerializeField]
    private TextMeshProUGUI afterName;
    [SerializeField]
    private TextMeshProUGUI afterHp;
    [SerializeField]
    private UnlockItemSlot slotPrefab;
    [SerializeField]
    private GameObject unlockContents;
    private List<UnlockItemSlot> unlockItemList = new List<UnlockItemSlot>();
    [SerializeField]
    private List<NeedItem> needItems = new List<NeedItem>();
    [SerializeField]
    private List<NeedPenalty> needPenalties = new List<NeedPenalty>();
    public TestInventory inven;

    private readonly string hpFormat = "HP : {0}";

    [SerializeField]
    private Button upgradeButton;
    private Inventory inventory;

    public void SetUIInfo(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        var system = selectedObject.uiController.GetComponent<PlacementSystem>();
        if (GameObject.FindWithTag(Tags.Player) != null)
        {
            inventory = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerFSM>().PlayerInventory;
        }
        upgradeButton.onClick.RemoveAllListeners();

        int index = 0;
        int penaltyIndex = 0;
        beforeImage.sprite = objInfo.Icon;
        beforeName.text = $"{objInfo.Name}";
        beforeHp.text = string.Format(hpFormat, objInfo.DefaultHp);

        int nextIndex = system.Database.objects.FindIndex(data => data.ID == objInfo.NextStructureID);
        PlacementObjectInfo nextLevelInfo = system.Database.objects[nextIndex];
        afterImage.sprite = nextLevelInfo.Icon;
        afterName.text = $"{nextLevelInfo.Name}";
        afterHp.text = string.Format(hpFormat, nextLevelInfo.DefaultHp);



        var unlockItems = DataTableManager.ItemCreateTable.GetAll();
        var structureData = DataTableManager.StructureTable.Get(nextLevelInfo.ID);
        var data = DataTableManager.ConstructionTable.Get(structureData.PlaceBuildingID);
        var unlockList = unlockItems.Where(
            item => item.Value.RecipeCategory == structureData.BuildingSubType && item.Value.Rank == structureData.Rank)
            .ToList();
        var itemTable = DataTableManager.ItemTable;


        foreach (var item in unlockItemList)
        {
            Destroy(item.gameObject);
        }
        unlockItemList.Clear();
        foreach (var unlock in unlockList)
        {
            var item = Instantiate(slotPrefab, unlockContents.transform);
            item.SetItem(itemTable.Get(unlock.Value.ResultID).ItemImage);
            unlockItemList.Add(item);
        }

        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var penalty in needPenalties)
        {
            penalty.gameObject.SetActive(false);
        }

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
        foreach (var item in data.NeedPenalties)
        {
            needPenalties[penaltyIndex].gameObject.SetActive(true);
            needPenalties[penaltyIndex].SetNeedPenalty(item.Key, item.Value);

            penaltyIndex++;
        }

        SetButtonDisable(objInfo.NeedItems);
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
