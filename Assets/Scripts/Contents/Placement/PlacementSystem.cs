using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    public PlacementObject SelectedObject { get; set; }
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
        if (inputManager.IsPointerOverUi)
        {
            return;
        }
        if (preview.IsPreview && inputManager.IsObjectSelected)
        {
            PlacementObject hit = inputManager.GetClickHit()?.gameObject.GetComponent<PlacementObject>();
            
            Vector3 mousePosition = inputManager.LastPosition;
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        
            bool placementValidity = CheckPlacementValidity(gridPosition, SelectedObjectIndex);
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastDetectedPosition = gridPosition;
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

    public void StartPlacement(int ID, PlacementObject obj = null)
    {
        StopPlacement();
        SelectedObjectIndex = database.objects.FindIndex(data => data.ID == ID);
        if (SelectedObjectIndex < 0)
        {
            Debug.LogError($"존재하지 않는 ID : {ID}");
            return;
        }
        preview.StartShowingPlacementPreview(database.objects[SelectedObjectIndex].Prefeb, obj);
    }

    private void StopPlacement()
    {
        SelectedObjectIndex = -1;
        preview.StopShowingPreview();
    }

    public bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        return gridData.CanPlaceObjectAt(gridPosition, database.objects[selectedObjectIndex].Size);
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
        if(SelectedObject != null)
        {
            Destroy(SelectedObject.transform.parent.gameObject);
            SelectedObject = null;
        }

        GameObject newObject = Instantiate(database.objects[SelectedObjectIndex].Prefeb);
        newObject.transform.position = grid.CellToWorld(gridPosition);

        PlacementObject placementObject = newObject.transform.GetChild(0).GetComponent<PlacementObject>();
        placementObject.IsPlaced = true;
        placementObject.Position = gridPosition;
        placedGameObjects.Add(placementObject);

        gridData.AddObjectAt(gridPosition, database.objects[SelectedObjectIndex].Size,
            database.objects[SelectedObjectIndex].ID,
            placedGameObjects.Count - 1, placementObject);
        //preview.UpdatePosition(grid.CellToWorld(gridPosition), CheckPlacementValidity(gridPosition, SelectedObjectIndex));
        preview.StopShowingPreview();
    }

    public bool SelectStructure(PlacementObject obj)
    {
        if(preview.IsPreview)
        {
            return false;
        }
        Vector3 position = obj.Position;
        Vector3Int gridPosition = grid.WorldToCell(position);

        int id = gridData.RemoveObjectAt(obj);
        placedGameObjects.Remove(obj);

        foreach(var item in placedGameObjects)
        {
            item.PlacementData.OrderPlaceObjectIndex(obj.PlacementData.PlaceObjectIndex);
        }
        SelectedObject = obj;
        SelectedObject.transform.parent.gameObject.SetActive(false);
        
        StartPlacement(id, obj);
        return true;
    }

    public void DestoryStructure()
    {
        Destroy(SelectedObject.transform.parent.gameObject);
        preview.StopShowingPreview();
    }
}
