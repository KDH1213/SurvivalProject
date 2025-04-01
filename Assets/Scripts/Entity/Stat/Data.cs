using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseData", menuName = "System/Stat/BaseData")]
public class Data : ScriptableObject
{
    public float aggroRange;
    public Vector3 attackSize;
    public Vector3 offset;
}
