
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridData
{
    private Dictionary<Vector3Int, PlacementObject> placedObject = new Dictionary<Vector3Int, PlacementObject>();
    private Vector2Int gridSize;

    public Vector3 pos;
    public Vector3 localScale;
    public GridData(Vector2Int gridSize)
    {
        this.gridSize = gridSize;
    }


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
                throw new System.Exception($"Dictionary already contains this cell position {pos}");
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
            if(CheckObjectInArea(pos))
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

        pos = test;
        localScale = new Vector3(magX * objectSize.x, 0f, magY * objectSize.y);

        Collider[] testCollider = Physics.OverlapBox(test,
            new Vector3(magX * objectSize.x, 1f, magY * objectSize.y), grid.transform.rotation);

        foreach(var obj in testCollider)
        {
            if (obj.tag.Equals("Monster") || obj.tag.Equals("Player") 
                || obj.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckObjectInArea(Vector3Int gridPosition)
    {
        if(gridPosition.x < -gridSize.x / 2 )
        {
            return true;
        }
        if (gridPosition.x > gridSize.x / 2 - 1)
        {
            return true;
        }
        if (gridPosition.z < -gridSize.y / 2)
        {
            return true;
        }
        if (gridPosition.z > gridSize.y / 2 - 1)
        {
            return true;
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
