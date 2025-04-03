using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Portion_", menuName = "Inventory System/Item Data/Portion")]
public class ToolData : ItemData
{
    /// <summary> 효과량(회복량 등) </summary>
    public float Value => value;
    public float Damage => damage;

    [SerializeField]
    private float value;
    [SerializeField]
    private float damage;
}
