using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "StageData", menuName = "System/Stage/StageData", order = 0)]
public class StageData : ScriptableObject
{
    [field: SerializeField]
    public Vector2 TileSize { get; private set; }

    [field: SerializeField]
    public int MapWidth { get; private set; }
    [field: SerializeField]
    public int MapHeight { get; private set; }

    [field: SerializeField]
    public GatherTypeMask CreateGatherType {  get; private set; }


    [field: SerializeField]
    public SerializedDictionary<GatherType, float> CreateGatherPercentTable = new SerializedDictionary<GatherType, float>();

    [field: SerializeField]
    public SerializedDictionary<GatherType, GatherData> GatherDataTable = new SerializedDictionary<GatherType, GatherData>();

    [ContextMenu("CreateTable")]
    private void OnCreatePercentTable()
    {
        CreateGatherPercentTable.Clear();
        GatherDataTable.Clear();

        for (int i = (int)GatherType.None + 1; i < (int)GatherType.End; ++i)
        {
            if((1 << i & (uint)CreateGatherType) != 0)
            {
                CreateGatherPercentTable.Add((GatherType)i, 0.1f);
                GatherDataTable.Add((GatherType)i, null);
            }
        }
    }

    public GatherInfo GetRandomGatherInfo(GatherType gatherType)
    {
        return GatherDataTable[gatherType].GetRandomGatherInfo();
    }
}
