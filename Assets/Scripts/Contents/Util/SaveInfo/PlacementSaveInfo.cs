using UnityEngine;

[System.Serializable]
public struct PlacementSaveInfo
{
    public Vector3Int position;
    public Quaternion rotation;
    public float hp;
    public int id;
}

[System.Serializable]
public struct FarmPlacementSaveInfo
{
    public Vector3Int position;
    public Quaternion rotation;
    public float hp;
    public float createTime;
    public int id;
    public int outPut;
}