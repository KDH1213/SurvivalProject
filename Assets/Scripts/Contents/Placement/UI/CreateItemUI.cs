using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    private CreateStructure currentObject;
    private GameObject target;

    public UnityEvent<int, int> onCreateItemEvent;

    private void Awake()
    {
        var player = GameObject.FindWithTag(Tags.Player);
        if (player != null)
        {
            inventory = player.GetComponent<PlayerFSM>().PlayerInventory;
        }

        var questSystem = GameObject.FindWithTag(Tags.QuestSystem);
        if(questSystem != null)
        {
            onCreateItemEvent.AddListener(questSystem.GetComponent<QuestSystem>().OnCreateItem);
        }
    }

    private void OnDisable()
    {
        currentObject = null;
    }

    [ContextMenu("openCreate")]
    public void SetUI(GameObject target, CreateStructure structure)
    {
        currentObject = structure;
        this.target = target;
        int index = 0;
        var data = DataTableManager.ItemCreateTable.GetAll();
        foreach (var item in createList)
        {
            Destroy(item.gameObject);
        }
        createList.Clear();
        foreach (var itemData in data)
        {
            if(itemData.Value.Rank > structure.Rank || DataTableManager.ItemTable.Get(itemData.Key) == null)
            {
                continue;
            }
            CreateItemSlot item = Instantiate(createItemPrefab, createListContents.transform);
            item.index = index;
            item.SetItemSlot(itemData.Key);
            item.GetComponent<Button>().onClick.AddListener(() => UpdateInfo(item.index));
            createList.Add(item);
            index++;
        }
        currentObject.closeUI.AddListener(() => gameObject.SetActive(false));
    }

    private void UpdateInfo(int index)
    {
        createButton.onClick.RemoveAllListeners();
        selectIndex = index;
        var info = createList[index].ItemInfo;
        var data = DataTableManager.ItemCreateTable.Get(info.id);
        itemImage.sprite = info.itemImage;
        itemName.text = info.data.ItemName;
        itemDescript.text = DataTableManager.StringTable.Get(info.data.DescriptID);
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

        SetButtonDisable(data.NeedItemList);
        createButton.onClick.AddListener(() => CreateItem(data, info.data));
    }


    private void CreateItem(ItemCreateTable.Data createData, ItemData data)
    {
        var createItem = new DropItemInfo();
        createItem.id = data.ID;  
        createItem.ItemName = data.ItemName;
        createItem.itemData = data;
        createItem.amount = createData.ResultValue;
        if (inventory != null)
        {
            ConsumItem(createData.NeedItemList);
            inventory.AddItem(createItem);
            onCreateItemEvent?.Invoke(createItem.id, createItem.amount);
        }
        else
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            dict.Add(createItem.id, createItem.amount);
            inven.MinusItem(createData.NeedItemList);
            inven.PlusItem(dict);
        }

        UpdateInfo(selectIndex);
    }

    private void SetButtonDisable(Dictionary<int, int> needItems)
    {
        if (inventory != null)
        {
            if (CanPlaced(needItems))
            {
                createButton.interactable = true;
            }
            else
            {
                createButton.interactable = false;
            }
        }
        else
        {
            if (!inven.CheckItemCount(needItems))
            {
                createButton.interactable = false;
            }
            else
            {
                createButton.interactable = true;
            }
        }
        
    }

    private bool CanPlaced(Dictionary<int, int> needItems)
    {
        foreach (var data in needItems)
        {
            if (inventory == null)
                break;
            if (inventory.GetTotalItem(data.Key) < data.Value)
            {
                return false;
            }
        }
        return true;
    }

    private void ConsumItem(Dictionary<int, int> needItems)
    {
        foreach (var data in needItems)
        {
            if (inventory == null)
                break;
            inventory.ConsumeItem(data.Key, data.Value);
        }
    }
}
