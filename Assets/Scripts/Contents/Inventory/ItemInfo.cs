using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo
{
    public ItemData itemData;
    public int index;
    public int Amount { get; set; }

    public void Empty()
    {
        itemData = null;
        Amount = 0;
    }
}
