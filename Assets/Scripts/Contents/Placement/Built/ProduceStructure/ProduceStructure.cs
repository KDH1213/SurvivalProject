using System.Collections;
using System.Collections.Generic;
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

    public int testOutPut;

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
        if (!IsPlaced)
        {
            return;
        }

        currentTime += Time.deltaTime;
        if (currentTime >= produceTime)
        {
            outPut++;
            testOutPut++;
            currentTime = 0f;
        }
    }

    public void SetProduceStructure(float produceTime, string kind, int outPut = 0)
    {
        this.produceTime = produceTime;
        this.kind = kind;
        this.outPut = outPut;
        testOutPut = outPut;
    }

    public override void Instruct()
    {
        throw new System.NotImplementedException();
    }
}
