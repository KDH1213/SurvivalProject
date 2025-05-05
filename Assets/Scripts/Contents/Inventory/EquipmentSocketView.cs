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
    [SerializeField]
    private TextMeshProUGUI heatResistanceText;
    [SerializeField]
    private TextMeshProUGUI coldResistanceText;

    private int seleteSocket = -1;
    private bool isOnDrag = false;

    public void Initialize()
    {
        if (SaveLoadManager.Data == null || SaveLoadManager.Data.equipmentItemIDList.Count == 0)
        {
            InitializeSockets();
        }
        else
        {
            Load();
        }

        for (int i = 0; i < (int)EquipmentType.Weapon; ++i)
        {
            playerStats.damegedEvent.AddListener(equipmentSockets[i].OnUseDurability);
            equipmentSockets[i].onBreakItemEvent.AddListener(OnBreakItem);
        }
        playerStats.onAttackEvent.AddListener(equipmentSockets[(int)EquipmentType.Weapon].OnUseDurability);
        equipmentSockets[(int)EquipmentType.Weapon].onBreakItemEvent.AddListener(OnBreakItem);
    }


    private void InitializeSockets()
    {
        for (int i = 0; i < equipmentSockets.Length; ++i)
        {
            SetInitializeEvent(i);
            equipmentSockets[i].InitializeSocket((EquipmentType)i, null, 0, 0);
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
        playerStats.onChangDamageValue += (value) => attackPowerText.text = value.ToString();
        playerStats.onChangeAttackSpeedValue += (value) => attackSpeedText.text = value.ToString();
        playerStats.onChangeSpeedValue +=(value) => moveSpeedText.text = value.ToString();
        playerStats.onChangeDefenceValue +=(value) => defenseText.text = value.ToString();
        // playerStats.onChangeColdResistanceValue +=(value) => coldResistanceText.text = value.ToString();
        // playerStats.onChangeHeatResistanceValue +=(value) => heatResistanceText.text = value.ToString();

        statTable[StatType.BasicAttackPower].OnActionChangeValue();
        statTable[StatType.AttackSpeed].OnActionChangeValue();
        statTable[StatType.MovementSpeed].OnActionChangeValue();
        statTable[StatType.Defense].OnActionChangeValue();
        var coldReistanceValue = statTable[StatType.ColdResistance];
        coldReistanceValue.OnChangeValue += (value) => coldResistanceText.text = value.ToString();
        coldResistanceText.text = coldReistanceValue.Value.ToString();

        var heatReistanceValue = statTable[StatType.HeatResistance];
        heatReistanceValue.OnChangeValue +=(value) => heatResistanceText.text = value.ToString();
        heatResistanceText.text = heatReistanceValue.Value.ToString();
    }

    public void OnEquipment(ItemType itemType, ItemData itemData, int amount, int durability)
    {
        var equipmentType = ItemData.ConvertEquipmentType(itemData.ID, itemType);
        equipmentSockets[(int)equipmentType].OnEquipment(equipmentType, itemData, amount, durability);
        playerStats.OnEquipmentItem(itemData);
    }

    public void OnUnEquipSocket()
    {
        if(inventory.IsFullInventory() || seleteSocket == -1 || equipmentSockets[seleteSocket].ItemData == null )
        {
            return;
        }

        DropItemInfo dropItemInfo = new DropItemInfo(equipmentSockets[seleteSocket].ItemInfo);
       inventory.AddItem(dropItemInfo);

        equipmentSockets[seleteSocket].OnUnEquipment();
        playerStats.OnUnEquipmentItem(dropItemInfo.itemData);
        itemInfoView.OnSetEquipmentItemInfo(null);

        seleteSocket = -1;
    }

    public void OnBreakItem(int socket)
    {
        playerStats.OnUnEquipmentItem(equipmentSockets[socket].ItemData);
        equipmentSockets[socket].OnUnEquipment();
        itemInfoView.OnSetEquipmentItemInfo(null);
    }

    public void OnSeleteSocket(EquipmentType equipmentType)
    {
        seleteSocket = (int)equipmentType;
        itemInfoView.OnSetEquipmentItemInfo(equipmentSockets[seleteSocket].ItemData);
    }

    public void OnChangeEquipment(EquipmentType equipmentType, int amount, int durability)
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
        if (SaveLoadManager.Data.isRestart)
        {
            InitializeSockets();
            return;
        }

        var equipmentItemList = SaveLoadManager.Data.equipmentItemIDList;
        var equipmentItemInfoSaveDataList = SaveLoadManager.Data.equipmentItemInfoSaveDataList;

        for (int i = 0; i < equipmentItemList.Count; ++i)
        {
            SetInitializeEvent(i);
            if (equipmentItemList[i] == -1)
            {
                equipmentSockets[i].InitializeSocket((EquipmentType)i, null, 0, 0);
                continue;
            }

            // 해당 부분 SaveLoad에 따라 ItemData 값 다르게 세팅
            var itemData = DataTableManager.ItemTable.Get(equipmentItemList[i]);
            if((EquipmentType)i == EquipmentType.Consumable)
            {
                equipmentSockets[i].InitializeSocket((EquipmentType)i, itemData, SaveLoadManager.Data.equipmentConsumableCount, 0);
            }
            else
            {
                // TODO
                equipmentSockets[i].InitializeSocket((EquipmentType)i, itemData, 1, itemData.Durability);
            }
        }
    }

    public void Save()
    {
        var equipmentItemList = SaveLoadManager.Data.equipmentItemIDList;
        var equipmentItemInfoSaveDataList = SaveLoadManager.Data.equipmentItemInfoSaveDataList;

        SaveLoadManager.Data.equipmentItemIDList.Clear();
        equipmentItemInfoSaveDataList.Clear();
        for (int i = 0; i < equipmentSockets.Length; ++i)
        {
            if(equipmentSockets[i].ItemData == null)
            {
                SaveLoadManager.Data.equipmentItemIDList.Add(-1);
                equipmentItemInfoSaveDataList.Add(new EquipmentItemInfoSaveData(-1, 0));
            }
            else
            {
                SaveLoadManager.Data.equipmentItemIDList.Add(equipmentSockets[i].ItemData.ID);
                equipmentItemInfoSaveDataList.Add(new EquipmentItemInfoSaveData(equipmentSockets[i].ItemData.ID, equipmentSockets[i].ItemInfo.Durability));
            }
        }

        if (equipmentSockets[(int)EquipmentType.Consumable].ItemData != null)
        {
            SaveLoadManager.Data.equipmentConsumableCount = equipmentSockets[(int)EquipmentType.Consumable].Amount;
        }
    }


}
