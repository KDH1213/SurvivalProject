using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private void Awake()
    {
        if(SaveLoadManager.Data == null || SaveLoadManager.Data.equipmentItemList.Count == 0)
        {
            for (int i = 0; i < equipmentSockets.Length; ++i)
            {
                equipmentSockets[i].InitializeSocket((EquipmentType)i, null);
                equipmentSockets[i].onClickEvent.AddListener(OnSeleteSocket);
                equipmentSockets[i].onUnEquipEvent.AddListener(OnUnEquipSocket);
                equipmentSockets[i].onChangeEquipEvent.AddListener(OnChangeEquipment);
            }
        }
        else
        {
            Load();
        }
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

    public void OnEquipment(ItemType itemType, ItemData itemData)
    {
        equipmentSockets[(int)itemType].OnEquipment((EquipmentType)itemType, itemData);
        playerStats.OnEquipmentItem(itemData);
    }

    public void OnUnEquipSocket()
    {
        if(inventory.IsFullInventory() || equipmentSockets[seleteSocket].ItemData == null)
        {
            return;
        }

        DropItemInfo dropItemInfo = new DropItemInfo();
        dropItemInfo.amount = 1;
        dropItemInfo.ItemName = equipmentSockets[seleteSocket].ItemData.ItemName;
        dropItemInfo.id = equipmentSockets[seleteSocket].ItemData.ID;
        dropItemInfo.itemData = equipmentSockets[seleteSocket].ItemData;
        inventory.AddItem(dropItemInfo);

        equipmentSockets[seleteSocket].OnUnEquipment();
        playerStats.OnUnEquipmentItem(dropItemInfo.itemData);
        itemInfoView.OnSetItemInfo(null);

        seleteSocket = -1;
    }

    public void OnSeleteSocket(EquipmentType equipmentType)
    {
        seleteSocket = (int)equipmentType;
        itemInfoView.OnSetItemInfo(equipmentSockets[seleteSocket].ItemData);
    }

    public void OnChangeEquipment(EquipmentType equipmentType)
    {
        playerStats.OnUnEquipmentItem(equipmentSockets[(int)equipmentType].ItemData);
        inventory.OnChangeEquimentItem(equipmentSockets[(int)equipmentType].ItemData); 
        itemInfoView.OnSetItemInfo(null);
        // equipmentSockets[seleteSocket].
    }


    public void Load()
    {
        var equipmentItemList = SaveLoadManager.Data.equipmentItemList;

        for (int i = 0; i < equipmentItemList.Count; ++i)
        {
            // 해당 부분 SaveLoad에 따라 ItemData 값 다르게 세팅
            equipmentSockets[i].InitializeSocket((EquipmentType)i, equipmentItemList[i]);
            equipmentSockets[i].onClickEvent.AddListener(OnSeleteSocket);
            equipmentSockets[i].onUnEquipEvent.AddListener(OnUnEquipSocket);
            equipmentSockets[i].onChangeEquipEvent.AddListener(OnChangeEquipment);
        }
    }

    public void Save()
    {
        var equipmentItemList = SaveLoadManager.Data.equipmentItemList;

        for (int i = 0; i < equipmentSockets.Length; ++i)
        {
            SaveLoadManager.Data.equipmentItemList.Add(equipmentSockets[i].ItemData);
        }
    }


}
