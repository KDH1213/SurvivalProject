using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageStructure : PlacementObject
{

    public StorageInventory inventory;
    public int maxStorage = 20;

    private void Start()
    {
        SetData();
    }

    
    public override void SetData()
    {
        inventory = GetComponent<StorageInventory>();
        inventory.SetItemInfos(maxStorage);
    }

    public override void Load()
    {
        int index = SaveLoadManager.Data.storagePlacementSaveInfo.FindIndex(x => x.position == Position && x.id == ID);
        inventory.Load(index);
    }

    public override void Save()
    {
        if (SaveLoadManager.Data == null)
        {
            return;
        }

        var saveInfo = new StoragePlacementSaveInfo();
        saveInfo.hp = Hp;
        saveInfo.position = Position;
        saveInfo.rotation = Rotation;
        saveInfo.id = ID;
        SaveLoadManager.Data.storagePlacementSaveInfo.Add(saveInfo);
        int index = SaveLoadManager.Data.storagePlacementSaveInfo.FindIndex(x => x.position == Position && x.id == ID);
        inventory.Save(index);
    }

    public override void Interact(GameObject interactor)
    {
        uiController.OnOpenStorageUI(interactor, this);
    }
}
