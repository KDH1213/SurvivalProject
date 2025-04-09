using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlacementObject : MonoBehaviour, IAttackable
{
    public PlacementData PlacementData { get; set; }
    public Vector3Int Position { get; set; }
    public Quaternion Rotation { get; set; }
    public bool IsPlaced { get; set; } = false;
    public bool IsCollision { get; set; } = false;
    public int Level { get; set; } = 1;
    public int ID { get; set; }
    public float Hp { get; set; }
    public abstract void Instruct();
    public abstract void SetData();

    public void OnAttack(GameObject attacker, DamageInfo damageInfo)
    {
        throw new System.NotImplementedException();
    }
}

