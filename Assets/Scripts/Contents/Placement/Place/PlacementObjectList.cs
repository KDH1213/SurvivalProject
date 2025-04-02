using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StructureKind
{
    Farm,
    Turret,
    Other

}

[CreateAssetMenu(fileName = "Placement", menuName = "Placement/PlacementObjectData")]
public class PlacementObjectList : ScriptableObject 
{
    public List<PlacementObjectInfo> objects;
}
