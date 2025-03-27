using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlaceObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int ID, int placeObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        this.ID = ID;
        PlaceObjectIndex = placeObjectIndex;
    }

    public void OrderPlaceObjectIndex(int removeIndex)
    {
        if(PlaceObjectIndex > removeIndex)
        {
            PlaceObjectIndex--;
        }
    }
}
