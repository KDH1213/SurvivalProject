using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlaceObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placeObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlaceObjectIndex = placeObjectIndex;
    }
}
