using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSocketView : MonoBehaviour, ISaveLoadData
{
    [SerializeField]
    private EquipmentSocket[] equipmentSockets;

    private PlayerStats playerStats;

    private void Awake()
    {
        if(SaveLoadManager.Data == null || SaveLoadManager.Data.equipmentItemList.Count == 0)
        {
            for (int i = 0; i < equipmentSockets.Length; ++i)
            {
                equipmentSockets[i].InitializeSocket((EquipmentType)i, null);
            }
        }
        else
        {
            Load();
        }

        // �ش� ��ġ�� �̺�Ʈ ����ϱ�
        // ����
        // ����
    }

    public void Load()
    {
        var equipmentItemList = SaveLoadManager.Data.equipmentItemList;

        for (int i = 0; i < equipmentItemList.Count; ++i)
        {
            // �ش� �κ� SaveLoad�� ���� ItemData �� �ٸ��� ����
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
}
