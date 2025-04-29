using System;
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
    private Image fillImage;
    [SerializeField]
    private TextMeshProUGUI objectName;
    [SerializeField]
    private TextMeshProUGUI objectSize;
    [SerializeField]
    private TextMeshProUGUI objectHp;
    [SerializeField]
    private TextMeshProUGUI objectDescription;
    [SerializeField]
    private TextMeshProUGUI objectInfo;
    [SerializeField]
    private TextMeshProUGUI produceOutPut;
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
        
    }
    public void SetUIInfo(PlacementObjectInfo objInfo, GameObject target, ProduceStructure selectedObject)
    {
        ResetInfo();

        var data = DataTableManager.StructureTable.Get(selectedObject.ID);
        var stringTable = DataTableManager.StringTable;
        var itemData = DataTableManager.ItemTable.Get(selectedObject.produceInfo.id);

        objectName.text = objInfo.Name;
        objectImage.sprite = objInfo.Icon;
        objectSize.text = $"{objInfo.Size.x} X {objInfo.Size.y}";
        objectHp.text = $"HP : {selectedObject.Hp}";

        objectDescription.text = stringTable.Get(data.DescriptID);
        objectDescription.text = $"{data.ProductionCycle}초 마다 {itemData.ItemName} {data.AmountPerProduction}개 생성";

        currentObject = selectedObject;

        Sprite sprite = itemData.ItemImage;
        produceInfoImage.sprite = sprite;
        produceOutPut.text = $"{selectedObject.produceInfo.outPutValue} / {data.MaxStorageCapacity}";

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

        currentObject = null;

        produceInfoImage.sprite = null;
        produceOutPut.text = null;
    }
}
