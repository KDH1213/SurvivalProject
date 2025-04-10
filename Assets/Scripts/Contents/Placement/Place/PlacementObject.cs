using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlacementObject : MonoBehaviour, ISaveLoadData
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

    public virtual void Save()
    {
        if(SaveLoadManager.Data == null)
        {
            return;
        }

        var saveInfo = new PlacementSaveInfo();
        saveInfo.hp = Hp;
        saveInfo.position = Position;
        saveInfo.rotation = Rotation;
        saveInfo.id = ID;
        SaveLoadManager.Data.placementSaveInfoList.Add(saveInfo);
    }

    public virtual void Load()
    {
    }
}

