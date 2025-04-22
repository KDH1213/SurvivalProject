using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageStructure : PlacementObject
{
    private int maxStorage;
    private List<ItemSlot> slots = new List<ItemSlot>();
    private Dictionary<ItemType, int> storages;
    public override void SetData()
    {

    }

    public override void Interact(GameObject interactor)
    {

    }
}
