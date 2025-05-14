using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageStructure : PlacementObject
{

    public StorageInventory inventory;
    public int maxStorage = 20;

    private void Start()
    {
        
    }

    
    public override void SetData()
    {
        inventory = GetComponent<StorageInventory>();
        inventory.SetItemInfos(maxStorage);
        if (ObjectPoolManager.Instance.ObjectPoolTable.TryGetValue(ObjectPoolType.HpBar, out var component))
        {
            var stats = GetComponent<StructureStats>();
            var hpBarObjectPool = component.GetComponent<UIHpBarObjectPool>();
            var hpBar = hpBarObjectPool.GetHpBar();
            hpBar.GetComponent<UITargetFollower>().SetTarget(transform, Vector3.zero);
            hpBar.SetTarget(stats);

            stats.deathEvent.AddListener(() => { hpBar.gameObject.SetActive(false); });
            disableEvent.AddListener(() => { if (hpBar != null) { hpBar.gameObject.SetActive(false); } });
            enableEvent.AddListener(() => { if (hpBar != null) { hpBar.gameObject.SetActive(true); } });
        }
    }

    public override void Upgrade(PlacementObject before)
    {
        var upgrade = before as StorageStructure;

        inventory = upgrade.inventory;
    }

    public override void Load()
    {
        int index = SaveLoadManager.Data.storagePlacementSaveInfo.FindIndex(x => x.position == Position && x.id == ID);
        inventory.Load(index);
        var data = SaveLoadManager.Data.storagePlacementSaveInfo.Find(x => x.position == Position && x.id == ID);
        Hp = data.hp;
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
