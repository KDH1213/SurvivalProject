using UnityEngine;


public class PlacementSaveData
{
}

[System.Serializable]
public class PlacementSaveInfo : PlacementSaveData
{
    public Vector3Int position;
    public Quaternion rotation;
    public float hp;
    public int id;
    
}

[System.Serializable]
public class FarmPlacementSaveInfo : PlacementSaveData
{
    public Vector3Int position;
    public Quaternion rotation;
    public float hp;
    public float createTime;
    public int id;
    public int outPut;
}
