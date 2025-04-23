using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageStructure : PlacementObject
{
    
    private Dictionary<ItemType, int> storages;
    [SerializeField]
    private PlacementUIController uicon;

    public StorageInventory inventory;
    public int maxStorage = 20;

    private void Start()
    {
        SetData();
    }
    public override void SetData()
    {
        inventory = new StorageInventory();
        inventory.SetItemInfos(maxStorage);
    }

    public override void Interact(GameObject interactor)
    {
        uicon.OnOpenStorageUI(interactor, this);
    }
}
