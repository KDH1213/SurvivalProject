using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryCheckUI : MonoBehaviour
{
    [SerializeField]
    private List<NeedItem> needItems;
    public void SetUI(int id)
    {
        int index = 0;
        var data = DataTableManager.StructureTable.Get(id);
        foreach (NeedItem needItem in needItems)
        {
            needItem.gameObject.SetActive(false);
        }
        foreach (var item in data.ReturnItemList)
        {
            var itemData = DataTableManager.ItemTable.Get(item.Key);
            needItems[index].gameObject.SetActive(true);
            needItems[index].SetNeedItem(itemData.ItemImage, item.Value, -1);
            index++;
        }
    }
}
