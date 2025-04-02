using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodProduceStructure : PlacementObject, IProduceStructure
{
    public float ProduceSpeed { get; set; }
    private int output;
    public void Produce()
    {
        throw new System.NotImplementedException();
    }

    public override void Instruct()
    {
        throw new System.NotImplementedException();
    }
}
