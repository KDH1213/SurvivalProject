using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStructure : PlacementObject
{

    public override void SetData()
    {
        if (ObjectPoolManager.Instance.ObjectPoolTable.TryGetValue(ObjectPoolType.HpBar, out var component))
        {
            var stats = GetComponent<StructureStats>();
            var hpBarObjectPool = component.GetComponent<UIHpBarObjectPool>();
            var hpBar = hpBarObjectPool.GetHpBar();
            hpBar.GetComponent<UITargetFollower>().SetTarget(transform, Vector3.zero);
            hpBar.SetTarget(stats);

            stats.deathEvent.AddListener(() => { hpBar.gameObject.SetActive(false); });
            disableEvent.AddListener(() => { hpBar.gameObject.SetActive(false); });
        }
    }

    public override void Load()
    {
        var data = SaveLoadManager.Data.placementSaveInfoList.Find(x => x.position == Position && x.id == ID);
        Hp = data.hp;
    }

    public override void Interact(GameObject interactor)
    {
        uiController.OnOpenCreateItemUI(interactor, this);
    }
}
