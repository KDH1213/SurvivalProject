using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlacementObject : MonoBehaviour 
{
    public PlacementData PlacementData { get; set; }
    public Vector3Int Position { get; set; }
    public bool IsPlaced { get; set; } = false;
    public bool IsCollision { get; set; } = false;
    public int Level { get; set; } = 1;
    public int ID { get; set; }
    public float Hp { get; set; }
    public abstract void Instruct();
}

public interface IProduceStructure
{
    public string Kind { get; set; }
    public float ProduceTime { get; set; }
    public float CurrentTime { get; set; }
    public int OutPut { get; set; }
    public void Produce(/* 자원에 대한 매개변수 */);
}

public interface IFenceStructure
{
    
}
