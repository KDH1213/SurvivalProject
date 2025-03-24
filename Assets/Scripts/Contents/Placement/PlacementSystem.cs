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

    private List<GameObject> placedGameObjects = new List<GameObject>();
    private GridData gridData;

    private void Start()
    {
        GetInputManager = inputManager;
        GetGrid = grid;
        gridData = new GridData();
        grid.cellSize = Vector3.one * 10 / gridCellCount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedObjectIndex = 0;
            preview.StartShowingPlacementPreview(database.objects[SelectedObjectIndex].Prefeb);
            preview.UpdatePosition(Vector3.zero, true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectedObjectIndex = 1;
            preview.StartShowingPlacementPreview(database.objects[SelectedObjectIndex].Prefeb);
            preview.UpdatePosition(Vector3.zero, true);
        }
    }

    public bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        Debug.Log(gridPosition);
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
        placedGameObjects.Add(newObject);

        gridData.AddObjectAt(gridPosition, database.objects[SelectedObjectIndex].Size,
            database.objects[SelectedObjectIndex].ID,
            placedGameObjects.Count - 1);
        //preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    
}
