using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

public class PlacementObjectInfo
{
    public int ID { get; set; }
    public int NextStructureID { get; set; }
    public int Rank { get; set; }
    public Vector2Int Size { get; set; } = Vector2Int.one;
    public StructureKind Kind { get; set; }
    public int SubType { get; set; }
    public string Name { get; set; }
    public int DefaultHp { get; set; }
    public GameObject Prefeb { get; set; }
    public SerializedDictionary<int, int> NeedItems { get; set; } = new SerializedDictionary<int, int>();
    public Sprite Icon { get; set; }
    public string Feature { get; set; }

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
