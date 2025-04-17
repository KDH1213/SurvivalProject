using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class FarmInfoUi : MonoBehaviour
{
    [SerializeField]
    private Image objectImage;
    [SerializeField]
    private Image produceInfoImage;
    [SerializeField]
    private Image currentProduceImage;
    [SerializeField]
    private TextMeshProUGUI objectLevel;
    [SerializeField]
    private TextMeshProUGUI objectName;
    [SerializeField]
    private TextMeshProUGUI produceTerm;
    [SerializeField]
    private TextMeshProUGUI produceOutPut;
    [SerializeField]
    private TextMeshProUGUI produceMax;
    [SerializeField]
    private TextMeshProUGUI currentTime;
    [SerializeField]
    private TextMeshProUGUI currentProduce;
    [SerializeField]
    private Button interact;
    public TestInventory inven;

    private ProduceStructure currentObject;
    private GameObject target;


    private void Awake()
    {
        interact.onClick.AddListener(() => onInteract());
    }
    private void OnDisable()
    {
        currentObject = null;
    }
    private void Update()
    {
        produceTerm.text = $"{currentObject.produceTime} 초";
        currentTime.text = $"{(currentObject.currentTime - Time.time):N0} 초";
        currentProduce.text = $"{currentObject.produceInfo.outPut} 개";
    }
    public void SetUIInfo(PlacementObjectInfo objInfo, GameObject target, ProduceStructure selectedObject)
    {
        ResetInfo();

        objectName.text = objInfo.Name;
        objectImage.sprite = objInfo.Icon;
        objectLevel.text = $"Level : {selectedObject.Rank}";

        currentObject = selectedObject;

        Sprite sprite = DataTableManager.ItemTable.Get(selectedObject.produceInfo.id).ItemImage;
        produceInfoImage.sprite = sprite;
        produceTerm.text = $"{selectedObject.produceTime} 초";
        produceOutPut.text = $"{selectedObject.produceInfo.outPutValue} 개";
        produceMax.text = $"{selectedObject.produceInfo.maxOutPut} 개";

        currentProduceImage.sprite = sprite;
        currentTime.text = $"{selectedObject.currentTime - Time.time:N0} 초";
        currentProduce.text = $"{selectedObject.produceInfo.outPut} 개";

        this.target = target;

        interact.onClick.AddListener(() => onInteract());
    }


    public void onInteract()
    {
        var testItem = DataTableManager.ItemTable.Get(currentObject.produceInfo.id);
        var inventroy = target.GetComponent<PlayerFSM>().PlayerInventory;

        int totalOutPut = currentObject.produceInfo.outPut;
        int slotCount = totalOutPut / testItem.MaxStack;
        int leftItems = totalOutPut % testItem.MaxStack;

        var produceItem = new DropItemInfo();
        produceItem.id = currentObject.produceInfo.id;  // testItem.ID
        produceItem.ItemName = testItem.ItemName;
        produceItem.itemData = testItem;

        for (int i = 0; i < slotCount; i++)
        {
            produceItem.amount = testItem.MaxStack;

            if (inventroy.IsFullInventory())
            {
                var itemList = inventroy.InventroyItemTable[produceItem.id];
                foreach (var item in itemList)
                {
                    int addUseCount = (item.itemData.MaxStack - item.Amount);
                    if (addUseCount > 0)
                    {
                        produceItem.amount = addUseCount;
                        totalOutPut -= addUseCount;
                        inventroy.AddItem(produceItem);
                    }
                }
                currentObject.produceInfo.structure.ReturnOutPut(totalOutPut);
                return;
            }

            totalOutPut -= produceItem.amount;
            inventroy.AddItem(produceItem);
        }

        produceItem.amount = leftItems;

        if (inventroy.IsFullInventory())
        {
            var itemList = inventroy.InventroyItemTable[produceItem.id];
            foreach (var item in itemList)
            {
                int addUseCount = (item.itemData.MaxStack - item.Amount);
                if (addUseCount > 0)
                {
                    produceItem.amount = addUseCount;
                    totalOutPut -= addUseCount;
                    inventroy.AddItem(produceItem);
                }
            }
            currentObject.produceInfo.structure.ReturnOutPut(totalOutPut);
            return;
        }
        totalOutPut -= leftItems;
        inventroy.AddItem(produceItem);
        currentObject.produceInfo.structure.ReturnOutPut(totalOutPut);

    }

    private void ResetInfo()
    {
        interact.onClick.RemoveAllListeners();

        objectName.text = null;
        objectImage.sprite = null;
        objectLevel.text = null;

        currentObject = null;

        produceInfoImage.sprite = null;
        produceTerm.text = null;
        produceOutPut.text = null;
        produceMax.text = null;

        currentProduceImage.sprite = null;
        currentTime.text = null;
        currentProduce.text = null;
    }
}
