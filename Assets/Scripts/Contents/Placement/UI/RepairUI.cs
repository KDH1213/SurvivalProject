using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairUI : MonoBehaviour
{
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
    public TestInventory inven;
    private Inventory inventory;

    public void SetUI(PlacementObjectInfo objInfo, PlacementObject selectedObject)
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            inventory = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerFSM>().PlayerInventory;
        }

        structureImage.sprite = objInfo.Icon;
        structureName.text = objInfo.Name;
        beforeHp.text = selectedObject.Hp.ToString();
        afterHp.text = objInfo.DefaultHp.ToString();

        int index = 0;
        var itemTable = DataTableManager.ItemTable;
        if (inventory == null)
        {
            foreach (var item in objInfo.NeedItems)
            {
                needItems[index].gameObject.SetActive(true);
                if (inven.inventory.ContainsKey(item.Key))
                {
                    needItems[index].SetNeedItem(itemTable.Get(item.Key).ItemImage,
                        item.Value, inven.inventory[item.Key]);
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
                        item.Value, inventory.GetTotalItem(item.Key));
                }
                else
                {
                    needItems[index].SetNeedItem(itemTable.Get(item.Key).ItemImage,
                        item.Value, inventory.GetTotalItem(item.Key));
                }

                needItems.Add(needItems[index]);
                index++;
            }
        }

    }
}
