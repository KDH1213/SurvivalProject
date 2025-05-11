using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairUI : MonoBehaviour
{
    [SerializeField]
    private PlacementSystem system;
    [SerializeField]
    private Image structureImage;
    [SerializeField]
    private TextMeshProUGUI structureName;
    [SerializeField]
    private TextMeshProUGUI beforeHp;
    [SerializeField]
    private TextMeshProUGUI afterHp;
    [SerializeField]
    private List<NeedItem> needItems;
    [SerializeField]
    private List<NeedPenalty> needPenalties;
    [SerializeField]
    private Button repairButton;
    public TestInventory inven;
    private Inventory inventory;

    private Dictionary<int, int> consumeItem = new();
    private Dictionary<SurvivalStatType, int> consumePenalty = new();

    private float hpPercent;

    public void SetUI(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            inventory = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerFSM>().PlayerInventory;
        }

        consumeItem.Clear();
        consumePenalty.Clear();

        structureImage.sprite = objInfo.Icon;
        structureName.text = objInfo.Name;
        hpPercent = selectedObject.Hp / objInfo.DefaultHp;
        beforeHp.text = selectedObject.Hp.ToString();
        afterHp.text = objInfo.DefaultHp.ToString();
        foreach (NeedItem needItem in needItems)
        {
            needItem.gameObject.SetActive(false);
        }
        foreach (var needPenalty in needPenalties)
        {
            needPenalty.gameObject.SetActive(false);
        }
        int index = 0;
        int penaltyIndex = 0;
        var itemTable = DataTableManager.ItemTable;
        var structureData = DataTableManager.StructureTable.Get(selectedObject.ID);
        var data = DataTableManager.ConstructionTable.Get(structureData.PlaceBuildingID);
        float percent = 0;
        if (hpPercent <= 0.4)
        {
            percent = 0.6f;
        }
        else if (hpPercent <= 0.7)
        {
            percent = 0.4f;
        }
        else if (hpPercent <= 1)
        {
            percent = 0.2f;
        }
        if (inventory == null)
        {
            foreach (var item in objInfo.NeedItems)
            {
                needItems[index].gameObject.SetActive(true);
                if (inven.inventory.ContainsKey(item.Key))
                {
                    needItems[index].SetNeedItem(itemTable.Get(item.Key).ItemImage,
                        Mathf.FloorToInt(item.Value * percent), inven.inventory[item.Key]);
                    consumeItem.Add(item.Key, Mathf.FloorToInt(item.Value * percent));
                }
                else
                {
                    needItems[index].SetNeedItem(itemTable.Get(item.Key).ItemImage,
                        item.Value, inven.inventory[item.Key]);
                }

                needItems.Add(needItems[index]);
                index++;
            }
        }
        else
        {
            foreach (var item in objInfo.NeedItems)
            {
                needItems[index].gameObject.SetActive(true);
                if (inventory.GetTotalItem(item.Key) < item.Value)
                {
                    needItems[index].SetNeedItem(itemTable.Get(item.Key).ItemImage,
                        Mathf.FloorToInt(item.Value * percent), inventory.GetTotalItem(item.Key));
                }
                else
                {
                    needItems[index].SetNeedItem(itemTable.Get(item.Key).ItemImage,
                        Mathf.FloorToInt(item.Value * percent), inventory.GetTotalItem(item.Key));
                }
                consumeItem.Add(item.Key, Mathf.FloorToInt(item.Value * percent));
                needItems.Add(needItems[index]);
                index++;
            }
        }

        foreach (var item in data.NeedPenalties)
        {
            needPenalties[penaltyIndex].gameObject.SetActive(true);
            needPenalties[penaltyIndex].SetNeedPenalty(item.Key, Mathf.FloorToInt(item.Value * percent));
            consumePenalty.Add(item.Key, Mathf.FloorToInt(item.Value * percent));
            penaltyIndex++;
        }

        repairButton.onClick.AddListener(() => Repair(consumeItem, consumePenalty));
    }

    public void Repair(Dictionary<int, int> needItems, Dictionary<SurvivalStatType, int> needPenalties)
    {
        system.Repair(needItems, needPenalties);
    }
}
