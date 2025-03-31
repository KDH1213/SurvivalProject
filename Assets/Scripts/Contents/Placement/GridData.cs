using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridData
{
    private Dictionary<Vector3Int, PlacementObject> placedObject = new Dictionary<Vector3Int, PlacementObject>();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placeObjectIndex, PlacementObject obj)
    {
        Debug.Log(gridPosition);
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placeObjectIndex);
        obj.PlacementData = data;
        foreach (var pos in positionToOccupy)
        {
            if (placedObject.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }
            placedObject[pos] = obj;
        }
    }

    public int RemoveObjectAt(PlacementObject obj)
    {
        int placeObjectIndex = obj.PlacementData.PlaceObjectIndex;
        int id = obj.PlacementData.ID;
        var objList = placedObject.Where(v => v.Value.PlacementData.PlaceObjectIndex == placeObjectIndex).ToList();
        foreach (var item in objList)
        {
            placedObject.Remove(item.Key);
        }

        return id;
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

    public Vector2Int GetPlaceObjectCenter(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        int yCenter = (gridPosition.z + positionToOccupy[objectSize.y - 1].z + 1) / 2;
        int xCenter = (gridPosition.x + positionToOccupy[objectSize.x + objectSize.y - 1].x + 1) / 2; 
        return new Vector2Int(xCenter ,yCenter);
    }
}
