using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

[Serializable]
public class PlacementObjectInfo
{
    [field: SerializeField]
    public int ID { get; set; }
    [field: SerializeField]
    public int MaxBuildCount { get; set; }
    [field: SerializeField]
    public Vector2Int Size { get; set; } = Vector2Int.one;
    [field: SerializeField]
    public StructureKind Kind { get; set; }
    [field: SerializeField]
    public string Name { get; set; }
    [field: SerializeField]
    public float DefaultHp { get; set; }
    [field: SerializeField]
    public GameObject Prefeb { get; set; }
    [field: SerializeField]
    public SerializedDictionary<string, int> NeedItems { get; set; } = new SerializedDictionary<string, int>();
    [field: SerializeField]
    public Sprite Icon { get; set; }
    [field: SerializeField]
    public string Feature { get; set; }
    /*[field: SerializeField]
    public List<PlacementLevelInfo> LevelList  { get; set; }*/

}

[Serializable]
public class PlacementLevelInfo
{
    [field: SerializeField]
    public string Name { get; set; }
    [field: SerializeField]
    public float DefaultHp { get; set; }
    [field: SerializeField]
    public GameObject Prefeb { get; set; }
    [field: SerializeField]
    public SerializedDictionary<string, int> NeedItems { get; set; }
    [field: SerializeField]
    public Sprite Icon { get; set; }
    [field: SerializeField]
    public string Feature { get; set; }
}
