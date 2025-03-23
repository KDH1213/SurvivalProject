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
    public GatherTypeMask CreateGatherType { get; private set; }

    [field: SerializeField]
    public float ResetTime { get; private set; }

    [field: SerializeField]
    public bool UseCreatePercent { get; private set; }

    [field: SerializeField]
    public List<GameObject> TilePrefabList { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public SerializedDictionary<GatherType, FloatMinMax> CreateGatherPercentTable = new SerializedDictionary<GatherType, FloatMinMax>();

    [field: SerializeField]
    public SerializedDictionary<GatherType, IntMinMax> CreateGatherCountTable = new SerializedDictionary<GatherType, IntMinMax>();

    [field: SerializeField]
    public SerializedDictionary<GatherType, GatherData> GatherDataTable = new SerializedDictionary<GatherType, GatherData>();

    [ContextMenu("CreateTable")]
    private void OnCreateTable()
    {
        CreateGatherPercentTable.Clear();
        CreateGatherCountTable.Clear();

        var gatherDataTable = new SerializedDictionary<GatherType, GatherData>();

        for (int i = (int)GatherType.None + 1; i < (int)GatherType.End; ++i)
        {
            if ((1 << i & (uint)CreateGatherType) != 0)
            {
                CreateGatherPercentTable.Add((GatherType)i, FloatMinMax.GetValue(0.1f, 0.1f));
                CreateGatherCountTable.Add((GatherType)i, IntMinMax.GetValue(10, 15));

                gatherDataTable.Add((GatherType)i, null);

                if (GatherDataTable.ContainsKey((GatherType)i) && GatherDataTable.TryGetValue((GatherType)i, out var data))
                {
                    gatherDataTable[(GatherType)i] = data;
                }
            }
        }

        GatherDataTable.Clear();
        GatherDataTable = gatherDataTable;
    }

    public GatherInfo GetRandomGatherInfo(GatherType gatherType)
    {
        return GatherDataTable[gatherType].GetRandomGatherInfo();
    }

    public GameObject GetRandomTile()
    {
        return TilePrefabList[Random.Range(0, TilePrefabList.Count)];
    }
}
