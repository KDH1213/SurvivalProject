using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GatherInfo
{
    public Vector3 offsetPosition;
    public GameObject prefab;
}


[CreateAssetMenu(fileName = "StageData", menuName = "System/Stage/GatherData", order = 1)]
public class GatherData : ScriptableObject
{
    [field: SerializeField]
    public GatherType GatherType { get; private set; }

    [field: SerializeField]
    public List<GatherInfo> GatherPrefabList { get; private set; } = new List<GatherInfo>();

    public GatherInfo GetRandomGatherInfo()
    {
        return GatherPrefabList[Random.Range(0, GatherPrefabList.Count)];
    }
}
