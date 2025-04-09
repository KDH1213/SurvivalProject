using UnityEngine;

public class ProduceStructure : PlacementObject
{
    [SerializeField]
    private float produceTime;
    [SerializeField]
    private float currentTime;
    [SerializeField]
    private string kind;
    [SerializeField]
    private int outPut;
    [SerializeField]
    private int maxOutPut;
    [SerializeField]
    private int outPutValue;

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
        if(outPut >= maxOutPut)
        {
            outPut = maxOutPut;
        }

        if (Time.time >= currentTime)
        {
            outPut += outPutValue;
            currentTime = Time.time + produceTime;
        }
    }

    public override void Instruct()
    {
        throw new System.NotImplementedException();
    }

    public override void SetData()
    {
        FarmTable.Data data = DataTableManager.FarmTable.Get(ID);
        produceTime = data.outputTime;
        kind = data.category.ToString();
        maxOutPut = data.maxStorage;
        outPutValue = data.outputValue;
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
        saveInfo.outPut = outPut;

        SaveLoadManager.Data.farmPlacementSaveInfos.Add(saveInfo);
    }

}
