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
}
