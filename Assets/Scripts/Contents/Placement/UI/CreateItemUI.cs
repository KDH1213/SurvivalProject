using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateItemUI : MonoBehaviour
{
    private readonly string armorFormat = "방어력 : {0}\n이동 속도 : {1}\n";
    private readonly string weaponFormat = "공격력 : {0}\n공격 속도 : {1}\n";
    private readonly string consumableFormat = "체력 : {0}\n포만감 : {1}\n수분 : {2}\n피로도 : {3}\n";

    [SerializeField]
    private GameObject createListContents;
    private List<CreateItemSlot> createList = new List<CreateItemSlot>();
    [SerializeField]
    private CreateItemSlot createItemPrefab;
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private List<BuildInfoUINeedItem> needItemList = new List<BuildInfoUINeedItem>();
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI itemDescript;
    [SerializeField]
    private Button createButton;

    private int selectIndex;
    private Inventory inventory;

    public TestInventory inven;

    private GameObject target;

    private void Awake()
    {
        if(GameObject.FindWithTag("Player") != null)
            inventory = GameObject.FindWithTag("Player").GetComponent<PlayerFSM>().PlayerInventory;
    }

    [ContextMenu("openCreate")]
    public void SetUI(GameObject target)
    {
        this.target = target;
        int index = 0;
        var data = DataTableManager.ItemCreateTable.GetAll();
        foreach (var itemData in data)
        {
            CreateItemSlot item = Instantiate(createItemPrefab, createListContents.transform);
            item.index = index;
            item.SetItemSlot(itemData.Key);
            item.GetComponent<Button>().onClick.AddListener(() => UpdateInfo(item.ItemInfo));
            createList.Add(item);
            index++;
        }
    }

    private void UpdateInfo(CreateItemInfo info)
    {
        var data = DataTableManager.ItemCreateTable.Get(info.id);
        itemImage.sprite = info.itemImage;
        itemName.text = info.itemName;
        itemDescript.text = info.itemDescription;
        int itemIdx = 0;

        foreach (var item in needItemList)
        {
            item.gameObject.SetActive(false);
        }

        if (inventory != null)
        {
            foreach (var itemData in data.NeedItemList)
            {
                var item = DataTableManager.ItemTable.Get(itemData.Key);
                needItemList[itemIdx].gameObject.SetActive(true);
                needItemList[itemIdx].SetNeedItem(item.ItemImage, itemData.Value, inventory.GetTotalItem(itemData.Key));
                itemIdx++;
            }
        }
        else
        {
            foreach (var itemData in data.NeedItemList)
            {
                var item = DataTableManager.ItemTable.Get(itemData.Key);
                needItemList[itemIdx].gameObject.SetActive(true);
                needItemList[itemIdx].SetNeedItem(item.ItemImage, itemData.Value, inven.inventory[itemData.Key]);
                itemIdx++;
            }
        }
        
    }

    private void onClickList()
    {
    }
}
