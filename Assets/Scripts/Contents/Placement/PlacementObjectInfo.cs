using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlacementObjectInfo
{
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public int MaxBuildCount { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public StructureKind Kind { get; private set; }
    [field: SerializeField]
    public List<PlacementLevelInfo> LevelList  { get; private set; }

}

[Serializable]
public class PlacementLevelInfo
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int DefaultHp { get; private set; }
    [field: SerializeField]
    public GameObject Prefeb { get; private set; }
    [field: SerializeField]
    public SerializedDictionary<string, int> NeedItems { get; private set; }
    [field: SerializeField]
    public Sprite Icon { get; private set; }
}
