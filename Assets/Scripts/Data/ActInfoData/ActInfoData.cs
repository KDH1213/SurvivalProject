using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActInfoData", menuName = "System/Act/ActInfoData", order = 1)]
public class ActInfoData : ScriptableObject
{
    [SerializedDictionary, SerializeField]
    public SerializedDictionary<ActType, ActData> actDataTable = new SerializedDictionary<ActType, ActData>();

    [ContextMenu("ActSetting")]
    public void OnInitialize()
    {
        actDataTable.Clear();

        for (int i = 0; i < (int)ActType.End; ++i)
        {
            actDataTable.Add((ActType)i, new ActData());
        }
    }
}
