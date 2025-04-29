using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedDropItem : MonoBehaviour, IDestructible
{ 
    // TODO :: 임시 코드
    [SerializeField]
    private int[] counts;

    [SerializeField]
    private int[] ids;

    private List<DropItemInfo> dropItemInfoList = new List<DropItemInfo>();

    private void Awake()
    {
        for (int i = 0; i < ids.Length; ++i)
        {
            var dropItemInfo = new DropItemInfo();
            dropItemInfo.itemData = DataTableManager.ItemTable.Get(ids[i]);
            dropItemInfo.id = ids[i];
            dropItemInfo.amount = counts[i];
            dropItemInfoList.Add(dropItemInfo);
        }
    }
    public void OnDestruction(GameObject attacker)
    {
        var player = attacker.GetComponent<PlayerFSM>();
        if (player != null)
        {
            //var dropItemList = DataTableManager.DropTable.Get(GetComponent<IDropable>().DropID).GetDropItemList();

            //foreach (var dropItem in dropItemList)
            //{
            //    player.OnDropItem(dropItemInfoList[i]);
            //}

            for (int i = 0; i < dropItemInfoList.Count; ++i)
            {
                player.OnDropItem(dropItemInfoList[i]);
            }

        }
    }
}