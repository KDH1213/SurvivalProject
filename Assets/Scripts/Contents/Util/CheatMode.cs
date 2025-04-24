using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheatMode : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField idField;

    [SerializeField]
    private Inventory playerInentory;

    public void OnCreateItem()
    {
        if(int.TryParse(idField.text, out var id))
        {
            var itemData = DataTableManager.ItemTable.Get(id);

            if(itemData != null)
            {
                DropItemInfo dropItemInfo = new DropItemInfo();
                dropItemInfo.id = id;
                dropItemInfo.itemData = itemData;
                dropItemInfo.amount = 1;

                playerInentory.AddItem(dropItemInfo);
            }
        }
    }
}
