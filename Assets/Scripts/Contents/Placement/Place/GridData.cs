
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
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

    public Vector3Int SearchSide(Vector3Int gridPos, Vector2Int objectSize)
    {
        var visited = new HashSet<Vector3Int>();
        var queue = new Queue<Vector3Int>();

        queue.Enqueue(gridPos);

        while (queue.Count > 0)
        {
            var currentPos = queue.Dequeue();
            visited.Add(currentPos);
            foreach (var adjacent in CalculateSide(currentPos, objectSize))
            {
                if (!CanPlaceObjectAt(adjacent,objectSize) || visited.Contains(adjacent) || queue.Contains(adjacent))
                    continue;

                return adjacent;
            }

            if(queue.Count == 0)
            {
                queue.Enqueue(new Vector3Int(currentPos.x - 1, 0, currentPos.y));
            }
        }

        return Vector3Int.zero;
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

    private List<Vector3Int> CalculateSide(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        int gridPosX = gridPosition.x;
        int gridPosY = gridPosition.z;

        RectInt rectInt = new RectInt(gridPosX - objectSize.x, gridPosY - objectSize.y, 
            gridPosX + objectSize.x, gridPosY + objectSize.y);

        for (int i = rectInt.x; i <= rectInt.width; i++)
        {
            for (int j = rectInt.y; j <= rectInt.height; j++)
            {
                if(i == rectInt.x || i == rectInt.width || j == rectInt.y || j == rectInt.height)
                {
                    returnVal.Add(new Vector3Int(i, 0, j));
                }
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
        var gridSizeX = gridSize.x / 2;
        var gridSizeY = gridSize.y / 2;

        if (gridPosition.x < -gridSizeX)
        {
            return true;
        }
        if (gridPosition.x > gridSizeX - 1)
        {
            return true;
        }
        if (gridPosition.z < -gridSizeY)
        {
            return true;
        }
        if (gridPosition.z > gridSizeY - 1)
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
