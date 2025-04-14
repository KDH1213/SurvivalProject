using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmUI : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI featureInfo;
    [SerializeField]
    private List<BuildInfoUINeedItem> needItems;
    [SerializeField]
    private Button interactButton;
    private GameObject target;

    private ProduceInfo produceInfo;

    public void SetUIInfo(PlacementSystem system, GameObject target, int id, ProduceInfo produceInfo)
    {
        if (system == null)
        {
            return;
        }

        this.produceInfo = produceInfo;
        this.target = target;
        int index = system.Database.objects.FindIndex(x => x.ID == id);
        PlacementObjectInfo data = system.Database.objects[index];
        itemImage.sprite = data.Icon;
        featureInfo.text = $"Name : {data.Name}\nLevel : {1}\nKind : {data.Kind}";
        foreach (var item in needItems)
        {
            item.gameObject.SetActive(false);
        }

        needItems[0].gameObject.SetActive(true);
        needItems[0].SetNeedItem(null, produceInfo.id, produceInfo.outPut, produceInfo.maxOutPut);
        needItems.Add(needItems[index]);
    }

    private void SetButtonDisable()
    {
        /*if (!inven.CheckItemCount(placementObject.NeedItems))
        {
            interactButton.interactable = false;
            interactButton.onClick.RemoveAllListeners();
        }
        else
        {
            interactButton.interactable = true;
            interactButton.onClick.AddListener(OnCloseWindow);
            interactButton.onClick.AddListener(() => system.StartPlacement(placementObject.ID));
        }*/
    }

    public void onInteract()
    {
        var testItem = DataTableManager.ItemTable.Get(1201001); // kind...?
        var inventroy = target.GetComponent<PlayerFSM>().PlayerInventory;

        int totalOutPut = produceInfo.outPut;
        int slotCount = totalOutPut / testItem.MaxAmount;
        int leftItems = totalOutPut % testItem.MaxAmount;

        var test = new DropItemInfo();
        test.id = 1201001;
        test.ItemName = testItem.ItemName;
        test.itemData = testItem;

        for (int i = 0; i < slotCount; i++)
        {
            test.amount = testItem.MaxAmount;

            if (inventroy.IsFullInventory())
            {
                var itemList = inventroy.InventroyItemTable[test.id];
                foreach (var item in itemList)
                {
                    int addUseCount = (item.itemData.MaxAmount - item.Amount);
                    if (addUseCount > 0)
                    {
                        test.amount = addUseCount;
                        totalOutPut -= addUseCount;
                        inventroy.AddItem(test);
                    }
                }
                produceInfo.structure.ReturnOutPut(totalOutPut);
                return;
            }

            totalOutPut -= test.amount;
            inventroy.AddItem(test);
        }

        test.amount = leftItems;
        
        if (inventroy.IsFullInventory())
        {
            var itemList = inventroy.InventroyItemTable[test.id];
            foreach (var item in itemList)
            {
                int addUseCount = (item.itemData.MaxAmount - item.Amount);
                if (addUseCount > 0)
                {
                    test.amount = addUseCount;
                    totalOutPut -= addUseCount;
                    inventroy.AddItem(test);
                }
            }
            produceInfo.structure.ReturnOutPut(totalOutPut);
            return;
        }
        totalOutPut -= leftItems;
        inventroy.AddItem(test);
        produceInfo.structure.ReturnOutPut(totalOutPut);
    }

    public void OnCloseWindow()
    {
        gameObject.SetActive(false);
    }

}
