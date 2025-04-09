using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct PlayerSaveInfo
{
    public Vector3 position;
    public float hp;

    public float[] survivalStatValues;
}