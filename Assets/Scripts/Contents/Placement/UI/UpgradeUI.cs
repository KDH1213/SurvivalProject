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
    private TextMeshProUGUI beforeHp;
    [SerializeField]
    private Image afterImage;
    [SerializeField]
    private TextMeshProUGUI afterName;
    [SerializeField]
    private TextMeshProUGUI afterHp;
    [SerializeField]
    private List<ChnageText> changeTextList = new List<ChnageText>();
    [SerializeField]
    private List<NeedItem> needItems = new List<NeedItem>();
    [SerializeField]
    private List<NeedPenalty> needPenalties = new List<NeedPenalty>();
    public TestInventory inven;

    [SerializeField]
    private Button upgradeButton;
    private Inventory inventory;

    private readonly string hpFormat = "HP : {0}";

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

        var structureData = DataTableManager.StructureTable.Get(nextLevelInfo.ID);
        var data = DataTableManager.ConstructionTable.Get(structureData.PlaceBuildingID);

        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var penalty in needPenalties)
        {
            penalty.gameObject.SetActive(false);
        }
        foreach (var text in changeTextList)
        {
            text.gameObject.SetActive(false);
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

        foreach (var item in data.NeedPenalties)
        {
            needPenalties[penaltyIndex].gameObject.SetActive(true);
            needPenalties[penaltyIndex].SetNeedPenalty(item.Key, item.Value);

            penaltyIndex++;
        }

        SetTextChange(objInfo, nextLevelInfo);

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

    private void SetTextChange(PlacementObjectInfo beforeInfo, PlacementObjectInfo afterInfo)
    {
        var beforeData = DataTableManager.StructureTable.Get(beforeInfo.ID);
        var afterData = DataTableManager.StructureTable.Get(afterInfo.ID);

        var kind = beforeInfo.Kind;
        switch (kind)
        {
            case StructureKind.Other:
                break;
            case StructureKind.Farm:
                changeTextList[0].gameObject.SetActive(true);
                changeTextList[1].gameObject.SetActive(true);
                changeTextList[2].gameObject.SetActive(true);
                changeTextList[0].SetText("생산 주기", $"{beforeData.ProductionCycle}"
                    , $"{afterData.ProductionCycle}");
                changeTextList[1].SetText("1회 생산량", $"{beforeData.AmountPerProduction}"
                    , $"{afterData.AmountPerProduction}");
                changeTextList[2].SetText("최대 생산량", $"{beforeData.MaxStorageCapacity}"
                    , $"{afterData.MaxStorageCapacity}");
                break;
            case StructureKind.Turret:
                var beforeTurret = beforeInfo.Prefeb.transform.GetChild(0).GetComponent<TurretStructure>();
                var afterTurret = afterInfo.Prefeb.transform.GetChild(0).GetComponent<TurretStructure>();
                changeTextList[0].gameObject.SetActive(true);
                changeTextList[1].gameObject.SetActive(true);
                changeTextList[0].SetText("공격력", $"{beforeTurret.damage}", $"{afterTurret.damage}");
                changeTextList[1].SetText("공격 속도", $"{beforeTurret.attackTerm}", $"{afterTurret.attackTerm}");
                break;
            case StructureKind.Storage:
                changeTextList[0].gameObject.SetActive(true);
                changeTextList[0].SetText("인벤토리 칸 수", $"{beforeData.BoxInventorySlot}"
                    , $"{afterData.BoxInventorySlot}");
                break;
            case StructureKind.Rest:
                changeTextList[0].gameObject.SetActive(true);
                changeTextList[0].SetText("분당 체력 회복량", $"{beforeData.FatigueReductionPerMinute}"
                    , $"{afterData.FatigueReductionPerMinute}");
                break;
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
