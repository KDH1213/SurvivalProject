using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
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

    public bool CheckCollideOther(Grid grid, Vector3Int gridPosition, Vector2Int objectSize)
    {
        Vector3 test = GetPlaceObjectCenter(grid, gridPosition, objectSize);
        Vector3Int nextPosX = new Vector3Int(gridPosition.x + 1, 0, gridPosition.z);
        Vector3Int nextPosY = new Vector3Int(gridPosition.x, 0, gridPosition.z + 1);
        float magX = Mathf.Abs(grid.CellToWorld(nextPosX).x - grid.CellToWorld(gridPosition).x);
        float magY = Mathf.Abs(grid.CellToWorld(nextPosY).z - grid.CellToWorld(gridPosition).z);

        Collider[] testCollider = Physics.OverlapBox(new Vector3(test.x, 0f, test.z),
            new Vector3(magX * objectSize.x, 0f, magY * objectSize.y), grid.transform.rotation);

        foreach(var obj in testCollider)
        {
            if(obj.tag.Equals("Monster"))
            {
                return true;
            }
        }
        return false;
    }

    public Vector3 GetPlaceObjectCenter(Grid grid, Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        Vector3 Center = (grid.CellToWorld(gridPosition) + 
            grid.CellToWorld(new Vector3Int(gridPosition.x + objectSize.x, 0, gridPosition.z + objectSize.y))) / 2;

        return Center;
    }
}
