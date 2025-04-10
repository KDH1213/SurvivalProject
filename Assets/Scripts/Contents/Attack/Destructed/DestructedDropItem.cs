using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedDropItem : MonoBehaviour, IDestructible
{ 
    // TODO :: 임시 코드
    [SerializeField]
    private int count;

    [SerializeField]
    private int id;

    private DropItemInfo dropItemInfo = new DropItemInfo();

    private void Awake()
    {
        dropItemInfo.itemData = DataTableManager.ItemTable.Get(id);
        dropItemInfo.ItemName = dropItemInfo.itemData.ItemName;
        dropItemInfo.amount = count;
    }
    public void OnDestruction(GameObject attacker)
    {
        var player = attacker.GetComponent<PlayerFSM>();
        if (player != null)
        {
            player.OnDropItem(dropItemInfo);
        }
    }
}