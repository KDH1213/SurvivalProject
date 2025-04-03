using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WoodProduceStructure : PlacementObject, IProduceStructure
{
    public float ProduceTime { get; set; }
    public float CurrentTime { get; set; }
    public string Kind { get; set; }
    public int OutPut { get; set; }

    public int testOutPut;

    private void Awake()
    {
        ProduceTime = 10f;
    }
    private void Update()
    {
        Produce();
    }
    public void Produce()
    {
        if(!IsPlaced)
        { 
            return; 
        }

        CurrentTime += Time.deltaTime;
        if (CurrentTime >= ProduceTime)
        {
            OutPut++;
            testOutPut++;
            CurrentTime = 0f;
        }
    }

    public override void Instruct()
    {
        throw new System.NotImplementedException();
    }
}
