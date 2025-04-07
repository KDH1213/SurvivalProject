using UnityEngine;

public class ProduceStructure : PlacementObject
{
    private float produceTime;
    private float currentTime;
    private string kind;
    private int outPut;
    private int maxOutPut;

    private void Awake()
    {
        produceTime = 5f;
    }
    private void Update()
    {
        Produce();
    }
    private void Produce()
    {
        if (!IsPlaced || outPut >=maxOutPut)
        {
            return;
        }

        if (Time.time >= currentTime)
        {
            outPut++;
            currentTime = Time.time + produceTime;
        }
    }

    public void SetProduceStructure(float produceTime, string kind, int maxOutPut, int outPut = 0)
    {
        this.produceTime = produceTime;
        this.kind = kind;
        this.outPut = outPut;
        this.maxOutPut = maxOutPut;
        currentTime = Time.time + produceTime;
    }

    public override void Instruct()
    {
        throw new System.NotImplementedException();
    }
}
