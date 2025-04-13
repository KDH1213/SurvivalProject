using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlacementObject : MonoBehaviour, ISaveLoadData, IInteractable
{
    public PlacementData PlacementData { get; set; }
    public Vector3Int Position { get; set; }
    public Quaternion Rotation { get; set; }
    public bool IsPlaced { get; set; } = false;
    public bool IsCollision { get; set; } = false;
    public int Level { get; set; } = 1;
    public int ID { get; set; }
    public float Hp { get; set; }
    public PlacementSystem system { get; set; }

    public InteractType InteractType => InteractType.Placement;

    public bool IsInteractable => true;

    public UnityEvent<GameObject> OnEndInteractEvent => throw new System.NotImplementedException();

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
        SaveLoadManager.Data.StageSaveData.placementSaveInfoList.Add(saveInfo);
    }

    public virtual void Load()
    {
    }

    public virtual void Interact(GameObject interactor)
    {
        DropItemInfo itemInfo = new DropItemInfo();
        //itemInfo.amount = 
        //interactor.GetComponent<PlayerFSM>().PlayerInventory.AddItem(DropItemInfo)
    }
}

