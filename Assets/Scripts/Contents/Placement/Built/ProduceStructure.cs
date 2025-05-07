using System;
using UnityEngine;

[Serializable]
public struct ProduceInfo
{
    public ProduceStructure structure;
    public int id;
    [SerializeField]
    public int outPut;
    [SerializeField]
    public int maxOutPut;
    [SerializeField]
    public int outPutValue;
}

public class ProduceStructure : PlacementObject
{
    [SerializeField]
    public float produceTime;
    [SerializeField]
    public float currentTime;
    [SerializeField]
    public ProduceInfo produceInfo;


    private void Awake()
    {

    }
    private void Update()
    {
        Produce();
    }
    private void Produce()
    {
        if (!IsPlaced)
        {
            return;
        }
        if(produceInfo.outPut >= produceInfo.maxOutPut)
        {
            produceInfo.outPut = produceInfo.maxOutPut;
            return;
        }

        if (Time.time >= currentTime)
        {
            produceInfo.outPut += produceInfo.outPutValue;
            currentTime = Time.time + produceTime;
        }
    }

    public override void SetData()
    {
        StructureTable.Data data = DataTableManager.StructureTable.Get(ID);
        produceTime = data.ProductionCycle;
        produceInfo.structure = this;
        produceInfo.id = data.ItemToProduce;
        produceInfo.maxOutPut = data.MaxStorageCapacity;
        produceInfo.outPutValue = data.AmountPerProduction;
        currentTime = Time.time + produceTime;
    }

    public override void Upgrade(PlacementObject before)
    {
        var upgrade = before as ProduceStructure;

        produceInfo.outPut = upgrade.produceInfo.outPut;
        currentTime = upgrade.currentTime;
    }

    public override void Save()
    {
        if (SaveLoadManager.Data == null)
        {
            return;
        }

        var saveInfo = new FarmPlacementSaveInfo();
        saveInfo.hp = Hp;
        saveInfo.position = Position;
        saveInfo.rotation = Rotation;
        saveInfo.id = ID;
        saveInfo.createTime = currentTime - Time.time;
        saveInfo.outPut = produceInfo.outPut;

        SaveLoadManager.Data.farmPlacementSaveInfos.Add(saveInfo);
    }

    public override void Load()
    {
        var data = SaveLoadManager.Data.farmPlacementSaveInfos.Find(x => x.position == Position && x.id == ID);
        produceInfo.outPut = data.outPut;
        Hp = data.hp;
        currentTime = Time.time + data.createTime;
    }

    public override void Interact(GameObject interactor)
    {
        Debug.Log("»£√‚");
        uiController.OnOpenFarmInfo(interactor, ID, this);
    }

    public void ReturnOutPut(int outPut)
    {
        produceInfo.outPut = outPut;
    }
}
