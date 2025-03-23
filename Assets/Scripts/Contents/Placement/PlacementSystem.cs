using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int selectedObjectIndex = 0;

    public float gridCellCount;

    private List<GameObject> placedGameObjects = new List<GameObject>();
    private void Start()
    {
        grid.cellSize = Vector3.one * 10 / gridCellCount;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            preview.StartShowingPlacementPreview(database.objects[0].Prefeb);
            SetEvent();
        }
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUi())
        {
            return;
        }
        Vector3 mousePosition = inputManager.LastPosition;
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        GameObject newObject = Instantiate(database.objects[selectedObjectIndex].Prefeb);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObjects.Add(newObject);

        Debug.Log(gridPosition);
    }

    private void PlacePreview()
    {
        if (inputManager.IsPointerOverUi())
        {
            return;
        }
        Vector3 mousePosition = inputManager.LastPosition;
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        preview.UpdatePosition(grid.CellToWorld(gridPosition),true);

        Debug.Log(gridPosition);
    }

    public void SetEvent()
    {
        if (preview.IsPreview)
        {
            inputManager.OnClickPlace += PlacePreview;
        }
        else
        {
            inputManager.OnClickPlace -= PlacePreview;
        }
    }

}
