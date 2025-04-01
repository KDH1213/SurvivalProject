using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private PlacementInput inputManager;
    [SerializeField]
    private PlacementUIController placementUI;
    [SerializeField]
    private PlacementPreview preview;
    [SerializeField]
    private TestInventory inven;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private PlacementObjectList database;
    public int SelectedObjectIndex { get; set; }
    public float gridCellCount;
    
    private GameObject testGameObject;
    public PlacementInput GetInputManager { get; private set; }
    public Grid GetGrid { get; private set; }

    public PlacementObject SelectedObject { get; set; }
    private List<PlacementObject> placedGameObjects = new List<PlacementObject>();
    private GridData gridData;

    private void Awake()
    {
        SelectedObjectIndex = -1;
        inputManager = GetComponent<PlacementInput>();
        placementUI = GetComponent<PlacementUIController>();
        GetGrid = grid;
        gridData = new GridData();
        grid.cellSize = Vector3.one * 10 / gridCellCount;
    }

    private void Update()
    {
        // 배치된 항목 이동
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
        }

    }

    // 배치시작
    public void StartPlacement(int ID)
    {
        preview.RePlaceObject();
        StopPlacement();
        SelectedObjectIndex = database.objects.FindIndex(data => data.ID == ID);
        if (SelectedObjectIndex < 0)
        {
            Debug.LogError($"존재하지 않는 ID : {ID}");
            return;
        }
        preview.StartShowingPlacementPreview(database.objects[SelectedObjectIndex].Prefeb);
    }

    // 배치 시작 overload
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

    // 배치 멈추기
    private void StopPlacement()
    {
        SelectedObjectIndex = -1;
        preview.StopShowingPreview();
    }

    // 배치 가능한지 검사
    public bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        Vector2Int size = database.objects[selectedObjectIndex].Size;
        return gridData.CanPlaceObjectAt(gridPosition, size) &&
            !gridData.CheckCollideOther(grid, gridPosition, size);
    }

    

    // 오브젝트 배치
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

        bool invenValidity = inven.CheckItemCount(database.objects[SelectedObjectIndex].NeedItems);
        if (!invenValidity)
        {
            StopPlacement();
            return;
        }

        GameObject newObject = Instantiate(database.objects[SelectedObjectIndex].Prefeb);
        newObject.transform.position = grid.CellToWorld(gridPosition);

        PlacementObject placementObject = newObject.transform.GetChild(0).GetComponent<PlacementObject>();
        placementObject.IsPlaced = true;
        placementObject.Position = gridPosition;
        placedGameObjects.Add(placementObject);

        inven.MinusItem(database.objects[SelectedObjectIndex].NeedItems);

        gridData.AddObjectAt(gridPosition, database.objects[SelectedObjectIndex].Size,
            database.objects[SelectedObjectIndex].ID,
            placedGameObjects.Count - 1, placementObject);
        placementUI.OnSetObjectListUi(database, placementObject.PlacementData.ID, placedGameObjects);
        //preview.UpdatePosition(grid.CellToWorld(gridPosition), CheckPlacementValidity(gridPosition, SelectedObjectIndex));
        StopPlacement();
    }

    // 배치된 오브젝트 선택
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

    // 설치된 오브젝트 삭제    
    public void DestoryStructure()
    {
        placementUI.OnSetObjectListUi(database, SelectedObject.PlacementData.ID, placedGameObjects);
        inven.PlusItem(database.objects[SelectedObjectIndex].NeedItems);
        Destroy(SelectedObject.transform.parent.gameObject);
        preview.StopShowingPreview();
    }

    // 오브젝트 원래 위치로 배치
    public void SetPlacementInfo(PlacementObject obj)
    {
        obj.IsPlaced = true;
        placedGameObjects.Add(obj);

        gridData.AddObjectAt(obj.Position, database.objects[SelectedObjectIndex].Size,
            database.objects[SelectedObjectIndex].ID,
            placedGameObjects.Count - 1, obj);
    }
}
