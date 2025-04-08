using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSocketView : MonoBehaviour, ISaveLoadData
{
    [SerializeField]
    private EquipmentSocket[] equipmentSockets;
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private PlayerStats playerStats;

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

        // 해당 위치에 이벤트 등록하기
        // 장착
        // 해제
    }

    public void Load()
    {
        var equipmentItemList = SaveLoadManager.Data.equipmentItemList;

        for (int i = 0; i < equipmentItemList.Count; ++i)
        {
            // 해당 부분 SaveLoad에 따라 ItemData 값 다르게 세팅
            equipmentSockets[i].InitializeSocket((EquipmentType)i, equipmentItemList[i]);
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

    public void OnEquipment(ItemType itemType, ItemData itemData)
    {
        equipmentSockets[(int)itemType].OnEquipment((EquipmentType)itemType, itemData);
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

        seleteSocket = -1;
    }

    public void OnSeleteSocket(EquipmentType equipmentType)
    {
        seleteSocket = (int)equipmentType;
    }

    public void OnChangeEquipment(EquipmentType equipmentType)
    {
        inventory.OnChangeEquimentItem(equipmentSockets[(int)equipmentType].ItemData);
        // equipmentSockets[seleteSocket].
    }
}
