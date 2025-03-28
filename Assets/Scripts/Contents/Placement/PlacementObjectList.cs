using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placement", menuName = "Placement/PlacementObjectData")]
public class PlacementObjectList : ScriptableObject 
{
    public List<PlacementObjectInfo> objects;
}
