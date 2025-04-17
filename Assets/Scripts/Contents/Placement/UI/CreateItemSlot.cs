using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct CreateItemInfo
{
    public int id;
    public Sprite itemImage;
    public Dictionary<int, int> needItemList;
    public ItemData data;
}

public class CreateItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image slotImage;
    [SerializeField]
    private Image disableImage;

    
    public int index;
    public CreateItemInfo ItemInfo { get; private set; }

    private void SetButtonDisable()
    {
        GetComponent<Button>().enabled = false;
        disableImage.gameObject.SetActive(true);
        slotImage.gameObject.SetActive(false);
    }

    public void SetItemSlot(int id)
    {
        
        var data = DataTableManager.ItemCreateTable.Get(id);
        var itemData = DataTableManager.ItemTable.Get(id);
        var itemInfo = new CreateItemInfo();
        itemInfo.id = id;
        itemInfo.itemImage = itemData.ItemImage;
        itemInfo.needItemList = new Dictionary<int, int>();
        foreach (var item in data.NeedItemList)
        {
            itemInfo.needItemList.Add(item.Key, item.Value);
        }
        itemInfo.data = itemData;

        ItemInfo = itemInfo;

        slotImage.sprite = itemData.ItemImage;

        disableImage.gameObject.SetActive(false);
        disableImage.color = new Color(1, 1, 1, 1);
        slotImage.sprite = itemData.ItemImage;
        slotImage.color = new Color(1, 1, 1, 1);

    }

    public void OnClickSlot()
    {

    }
}
