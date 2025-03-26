using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    private Dictionary<Vector3Int, PlacementData> placedObject = new Dictionary<Vector3Int, PlacementData>();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placeObjectIndex)
    {
        Debug.Log(gridPosition);
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placeObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if (placedObject.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }
            placedObject[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int i = 0; i < objectSize.x; i++)
        {
            for (int j = 0; j < objectSize.y; j++)
            {
                returnVal.Add(gridPosition + new Vector3Int(i, 0, j));
            }
        }

        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObject.ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    }
}
