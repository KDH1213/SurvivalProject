using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSocketView : MonoBehaviour, ISaveLoadData
{
    [SerializeField]
    private EquipmentSocket[] equipmentSockets;
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private PlayerStats playerStats;
    [SerializeField]
    private ItemInfoView itemInfoView;


    [SerializeField]
    private TextMeshProUGUI attackPowerText;
    [SerializeField]
    private TextMeshProUGUI attackSpeedText;
    [SerializeField]
    private TextMeshProUGUI moveSpeedText;
    [SerializeField]
    private TextMeshProUGUI defenseText;

    private int seleteSocket = -1;
    private bool isOnDrag = false;

    public void Initialize()
    {
        if (SaveLoadManager.Data == null || SaveLoadManager.Data.equipmentItemIDList.Count == 0)
        {
            for (int i = 0; i < equipmentSockets.Length; ++i)
            {
                SetInitializeEvent(i);
                equipmentSockets[i].InitializeSocket((EquipmentType)i, null, 0);
            }
        }
        else
        {
            Load();
        }
    }

    private void SetInitializeEvent(int index)
    {
        equipmentSockets[index].onClickEvent.AddListener(OnSeleteSocket);
        equipmentSockets[index].onUnEquipEvent.AddListener(OnUnEquipSocket);
        equipmentSockets[index].onChangeEquipEvent.AddListener(OnChangeEquipment);
        equipmentSockets[index].onDragEnter.AddListener(() => 
        { 
            isOnDrag = true; seleteSocket = (int)equipmentSockets[index].EquipmentType;

            if (inventory.PrivewIcon != null && equipmentSockets[index].ItemData != null)
            {
                inventory.PrivewIcon.SetActive(true);
                inventory.PrivewIcon.GetComponent<Image>().sprite = equipmentSockets[index].ItemData.ItemImage;
            }
        });
        equipmentSockets[index].onDragEvent.AddListener((position) => { if (inventory.PrivewIcon != null) inventory.PrivewIcon.transform.position = position; });
        equipmentSockets[index].onDragExit.AddListener(OnEndDrag);
    }

    private void Start()
    {
        var statTable = playerStats.CurrentStatTable;
        statTable[StatType.BasicAttackPower].OnChangeValueAction((value) => attackPowerText.text = value.ToString());
        statTable[StatType.AttackSpeed].OnChangeValueAction((value) => attackSpeedText.text = value.ToString());
        statTable[StatType.MovementSpeed].OnChangeValueAction((value) => moveSpeedText.text = value.ToString());
        statTable[StatType.Defense].OnChangeValueAction((value) => defenseText.text = value.ToString());

        statTable[StatType.BasicAttackPower].OnActionChangeValue();
        statTable[StatType.AttackSpeed].OnActionChangeValue();
        statTable[StatType.MovementSpeed].OnActionChangeValue();
        statTable[StatType.Defense].OnActionChangeValue();
    }

    public void OnEquipment(ItemType itemType, ItemData itemData, int amount)
    {
        var equipmentType = ItemData.ConvertEquipmentType(itemData.ID, itemType);
        equipmentSockets[(int)equipmentType].OnEquipment(equipmentType, itemData, amount);
        playerStats.OnEquipmentItem(itemData);
    }

    public void OnUnEquipSocket()
    {
        if(inventory.IsFullInventory() || seleteSocket == -1 || equipmentSockets[seleteSocket].ItemData == null)
        {
            return;
        }

        DropItemInfo dropItemInfo = new DropItemInfo();
        dropItemInfo.amount = equipmentSockets[seleteSocket].Amount;
        dropItemInfo.ItemName = equipmentSockets[seleteSocket].ItemData.ItemName;
        dropItemInfo.id = equipmentSockets[seleteSocket].ItemData.ID;
        dropItemInfo.itemData = equipmentSockets[seleteSocket].ItemData;
        inventory.AddItem(dropItemInfo);

        equipmentSockets[seleteSocket].OnUnEquipment();
        playerStats.OnUnEquipmentItem(dropItemInfo.itemData);
        itemInfoView.OnSetEquipmentItemInfo(null);

        seleteSocket = -1;
    }

    public void OnSeleteSocket(EquipmentType equipmentType)
    {
        seleteSocket = (int)equipmentType;
        itemInfoView.OnSetEquipmentItemInfo(equipmentSockets[seleteSocket].ItemData);
    }

    public void OnChangeEquipment(EquipmentType equipmentType, int amount)
    {
        if(equipmentType != EquipmentType.Consumable)
        {
            playerStats.OnUnEquipmentItem(equipmentSockets[(int)equipmentType].ItemData);
        }

        inventory.OnChangeEquimentItem(equipmentSockets[(int)equipmentType].ItemData, amount); 
        itemInfoView.OnSetEquipmentItemInfo(null);
        // equipmentSockets[seleteSocket].
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (inventory.PrivewIcon != null)
        {
            inventory.PrivewIcon.SetActive(false);
        }

        if (seleteSocket == -1 || equipmentSockets[seleteSocket].ItemData == null)
        {
            isOnDrag = false;
            seleteSocket = -1;
            itemInfoView.OnSetEquipmentItemInfo(null);
            return;
        }

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            var itemSlot = result.gameObject.GetComponent<ItemSlot>();

            if (itemSlot == null)
            {
                continue;
            }

            itemSlot.onDragEnter?.Invoke();
            if (itemSlot.ItemData == null)
            {
                inventory.OnSetEquipmentItem(equipmentSockets[seleteSocket].ItemData, equipmentSockets[seleteSocket].Amount);
                equipmentSockets[seleteSocket].OnUnEquipment();
            }
            else
            {
                inventory.OnEndDragTargetToEquipmentSocket(equipmentSockets[seleteSocket]);
            }
            break;
        }

        seleteSocket = -1;
        isOnDrag = false;
    }

    public void Load()
    {
        var equipmentItemList = SaveLoadManager.Data.equipmentItemIDList;

        for (int i = 0; i < equipmentItemList.Count; ++i)
        {
            SetInitializeEvent(i);
            if (equipmentItemList[i] == -1)
            {
                equipmentSockets[i].InitializeSocket((EquipmentType)i, null, 0);
                continue;
            }

            // 해당 부분 SaveLoad에 따라 ItemData 값 다르게 세팅
            var itemData = DataTableManager.ItemTable.Get(equipmentItemList[i]);
            if((EquipmentType)i == EquipmentType.Consumable)
            {
                equipmentSockets[i].InitializeSocket((EquipmentType)i, itemData, SaveLoadManager.Data.equipmentConsumableCount);
            }
            else
            {
                equipmentSockets[i].InitializeSocket((EquipmentType)i, itemData, 1);
            }
        }
    }

    public void Save()
    {
        var equipmentItemList = SaveLoadManager.Data.equipmentItemIDList;
        SaveLoadManager.Data.equipmentItemIDList.Clear();
        for (int i = 0; i < equipmentSockets.Length; ++i)
        {
            if(equipmentSockets[i].ItemData == null)
            {
                SaveLoadManager.Data.equipmentItemIDList.Add(-1);
            }
            else
            {
                SaveLoadManager.Data.equipmentItemIDList.Add(equipmentSockets[i].ItemData.ID);
            }
        }

        if (equipmentSockets[(int)EquipmentType.Consumable].ItemData != null)
        {
            SaveLoadManager.Data.equipmentConsumableCount = equipmentSockets[(int)EquipmentType.Consumable].Amount;
        }
    }


}
