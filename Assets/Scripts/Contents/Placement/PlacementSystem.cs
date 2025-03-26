using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private PlacementInput inputManager;
    [SerializeField]
    private PreviewObject preview;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private PlacementObjectList database;
    public int SelectedObjectIndex { get; set; }

    public float gridCellCount;

    public PlacementInput GetInputManager { get; private set; }
    public Grid GetGrid { get; private set; }

    private List<PlacementObject> placedGameObjects = new List<PlacementObject>();
    private GridData gridData;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        SelectedObjectIndex = -1;
        //inputManager = GetComponent<PlacementInput>();
        GetInputManager = inputManager;
        GetGrid = grid;
        gridData = new GridData();
        grid.cellSize = Vector3.one * 10 / gridCellCount;
    }

    private void Update()
    {
        if (!inputManager.IsObjectSelected)
        {
            return;
        }
        // 현재 클릭시 invoke되는 부분과 겹쳐 수정 필요
        if (SelectedObjectIndex < 0)
        {
            return;
        }
        inputManager.GetClickHit();
        Vector3 mousePosition = inputManager.LastPosition;
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (preview.IsPreview)
        {
            if (lastDetectedPosition != gridPosition)
            {
                bool placementValidity = CheckPlacementValidity(gridPosition, SelectedObjectIndex);
                preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
                lastDetectedPosition = gridPosition;
            }
        }
        else
        {

        }


    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        SelectedObjectIndex = database.objects.FindIndex(data => data.ID == ID);
        if (SelectedObjectIndex < 0)
        {
            Debug.LogError($"존재하지 않는 ID : {ID}");
            return;
        }
        preview.StartShowingPlacementPreview(database.objects[SelectedObjectIndex].Prefeb);
    }

    private void StopPlacement()
    {
        SelectedObjectIndex = -1;
        preview.StopShowingPreview();
    }

    public bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        return gridData.CanPlaceObjectAt(gridPosition, database.objects[SelectedObjectIndex].Size);
    }

    public void PlaceStructure()
    {
        Vector3 mousePosition = inputManager.LastPosition;
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, SelectedObjectIndex);
        if (!placementValidity)
        {
            return;
        }

        GameObject newObject = Instantiate(database.objects[SelectedObjectIndex].Prefeb);
        newObject.transform.position = grid.CellToWorld(gridPosition);

        PlacementObject placementObject = newObject.transform.GetChild(0).GetComponent<PlacementObject>();
        placementObject.IsPlaced = true;
        placedGameObjects.Add(placementObject);

        gridData.AddObjectAt(gridPosition, database.objects[SelectedObjectIndex].Size,
            database.objects[SelectedObjectIndex].ID,
            placedGameObjects.Count - 1, placementObject);
        preview.UpdatePosition(grid.CellToWorld(gridPosition), CheckPlacementValidity(gridPosition, SelectedObjectIndex));
    }

    public void SelectStructure(PlacementObject obj)
    {
        if(preview.IsPreview)
        {
            return;
        }
        Vector3 mousePosition = inputManager.LastPosition;
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        int id = gridData.RemoveObjectAt(gridPosition);
        Destroy(obj.transform.parent.gameObject);
        StartPlacement(id);
    }
}
