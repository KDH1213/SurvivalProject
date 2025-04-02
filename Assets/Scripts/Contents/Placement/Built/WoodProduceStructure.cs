using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodProduceStructure : MonoBehaviour, IProduceStructure
{
    public float ProduceSpeed { get; set; }
    public Vector3Int Position { get; set; }
    public bool IsPlaced { get; set; }
    public bool IsCollision { get; set; }
    public int Level { get; set; }
    public int ID { get; set; }
    public float Hp { get; set; }

    private int output;

    public void Instruct()
    {
        throw new System.NotImplementedException();
    }

    public void OnDestruction(GameObject attacker)
    {
        throw new System.NotImplementedException();
    }

    public void Produce()
    {
        throw new System.NotImplementedException();
    }
}
