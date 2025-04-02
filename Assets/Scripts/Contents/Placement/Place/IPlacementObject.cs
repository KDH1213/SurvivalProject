using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlacementObject : IDestructible
{
    public Vector3Int Position { get; set; }
    public bool IsPlaced { get; set; }
    public bool IsCollision { get; set; }
    public int Level { get; set; }
    public int ID { get; set; }
    public float Hp { get; set; }
    public void Instruct();
}

public interface IProduceStructure : IPlacementObject
{
    public float ProduceSpeed { get; set; }
    public void Produce(/* �ڿ��� ���� �Ű����� */);
}

public interface IFenceStructure : IPlacementObject
{
    
}
