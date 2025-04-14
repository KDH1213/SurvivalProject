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
    private float produceTime;
    [SerializeField]
    private float currentTime;
    [SerializeField]
    private ProduceInfo produceInfo;


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
        }

        if (Time.time >= currentTime)
        {
            produceInfo.outPut += produceInfo.outPutValue;
            currentTime = Time.time + produceTime;
        }
    }

    public override void SetData()
    {
        FarmTable.Data data = DataTableManager.FarmTable.Get(ID);
        produceTime = data.outputTime;
        produceInfo.structure = this;
        produceInfo.id = data.itemID;
        produceInfo.maxOutPut = data.maxStorage;
        produceInfo.outPutValue = data.outputValue;
        currentTime = Time.time + produceTime;
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
        currentTime = Time.time + data.createTime;
    }

    public override void Interact(GameObject interactor)
    {
        uiController.OnOpenFarmInfo(interactor, ID, produceInfo);
    }

    public void ReturnOutPut(int outPut)
    {
        produceInfo.outPut = outPut;
    }
}
