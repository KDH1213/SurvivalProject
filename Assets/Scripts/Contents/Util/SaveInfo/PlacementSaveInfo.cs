using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlacementSaveInfo
{
    public Vector3Int position;
    public Quaternion rotation;
    public float hp;
    public int id;
    
}

[System.Serializable]
public class FarmPlacementSaveInfo
{
    public Vector3Int position;
    public Quaternion rotation;
    public float hp;
    public float createTime;
    public int id;
    public int outPut;
}

public class StoragePlacementSaveInfo
{
    public Vector3Int position;
    public Quaternion rotation;
    public float hp;
    public int id;
    public List<ItemInfoSaveData> itemDatas;
}
