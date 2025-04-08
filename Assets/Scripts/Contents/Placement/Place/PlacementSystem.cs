using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private PlacementInput inputManager;
    [SerializeField]
    private PlacementUIController placementUI;
    [SerializeField]
    private PlacementMode placementMode;
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

    public PlacementObject SelectedObject; // { get; set; }
    private List<PlacementObject> placedGameObjects = new List<PlacementObject>();
    private GridData gridData;

    private void Awake()
    {
        SelectedObjectIndex = -1;
        inputManager = GetComponent<PlacementInput>();
        placementMode = GetComponent<PlacementMode>();
        placementUI = GetComponent<PlacementUIController>();
        GetGrid = grid;
        gridData = new GridData(new Vector2Int(18,18));
        grid.cellSize = Vector3.one * 10 / gridCellCount;

        DataTableManager.Get<StructureTable>("");
    }

    private void Update()
    {
        // 배치된 항목 이동
        if (inputManager.IsPointerOverUi)
        {
            return;
        }
        if (preview.IsPreview && inputManager.IsObjectHoldPress)
        {
            inputManager.UpdatePosition();
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
        preview.StartShowingPlacementPreview(database.objects[SelectedObjectIndex].LevelList[0].Prefeb,
            database.objects[SelectedObjectIndex].Size);
    }

    // 배치 시작 overload
    public void StartPlacement(int ID, PlacementObject obj)
    {
        //StopPlacement();
        SelectedObjectIndex = database.objects.FindIndex(data => data.ID == ID);
        if (SelectedObjectIndex < 0)
        {
            Debug.LogError($"존재하지 않는 ID : {ID}");
            return;
        }
        preview.StartShowingPlacementPreview(database.objects[SelectedObjectIndex].LevelList[0].Prefeb,
            database.objects[SelectedObjectIndex].Size, obj);
    }

    // 배치 멈추기
    private void StopPlacement()
    {
        SelectedObjectIndex = -1;
        SelectedObject = null;
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
        bool invenValidity = inven.CheckItemCount(database.objects[SelectedObjectIndex].LevelList[0].NeedItems);
        if (!invenValidity)
        {
            StopPlacement();
            return;
        }

        if (SelectedObject != null)
        {
            MoveStructure(gridPosition);
            return;
        }
        else
        {
            inven.MinusItem(database.objects[SelectedObjectIndex].LevelList[0].NeedItems);
        }

        GameObject newObject = Instantiate(database.objects[SelectedObjectIndex].LevelList[0].Prefeb);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        newObject.transform.GetChild(0).rotation = preview.PreviewObject.transform.GetChild(0).rotation;

        PlacementObject placementObject = newObject.transform.GetChild(0).GetComponent<PlacementObject>();
        placementObject.IsPlaced = true;
        placementObject.ID = SelectedObjectIndex;
        placementObject.Position = gridPosition;
        placementObject.Rotation = preview.PreviewObject.transform.GetChild(0).rotation;

        placedGameObjects.Add(placementObject);

        gridData.AddObjectAt(gridPosition, database.objects[SelectedObjectIndex].Size,
            database.objects[SelectedObjectIndex].ID,
            placedGameObjects.Count - 1, placementObject);

        int left = placementUI.OnSetObjectListUi(database, placementObject.PlacementData.ID, placedGameObjects);
        if(left <= 0)
        {
            StopPlacement();
            return;
        }

        Vector3Int nextPos = gridData.SearchSide(gridPosition, database.objects[SelectedObjectIndex].Size);
        preview.UpdatePosition(grid.CellToWorld(nextPos),
            CheckPlacementValidity(nextPos, SelectedObjectIndex));
        inputManager.LastPosition = grid.CellToWorld(nextPos);
        //StopPlacement();
    }

    // 배치된 오브젝트 선택
    public bool SelectStructure(PlacementObject obj)
    {
        if(preview.IsPreview)
        {
            return false;
        }

        SelectedObject = obj;

        if (placementMode.CurrentMode == Mode.Place)
        {
            int index = database.objects.FindIndex(data => data.ID == SelectedObject.ID);
            placementUI.OnOpenObjectInfo(database.objects[index]);
        }
        else if(placementMode.CurrentMode == Mode.Edit)
        {
            int id = RemoveStructure(obj);
            StartPlacement(id, obj);
        }
        
        return true;
    }

    private void MoveStructure(Vector3Int gridPos)
    {
        SelectedObject.transform.parent.position = grid.CellToWorld(gridPos);
        SelectedObject.Position = gridPos;
        SelectedObject.transform.rotation = preview.PreviewObject.transform.GetChild(0).rotation;
        SelectedObject.Rotation = SelectedObject.transform.rotation;
        SelectedObject.transform.parent.gameObject.SetActive(true);
        SetPlacementInfo(SelectedObject);
        SelectedObject = null;
        StopPlacement();
    }

    public int RemoveStructure(PlacementObject obj)
    {
        SelectedObject.transform.parent.gameObject.SetActive(false);
        int id = gridData.RemoveObjectAt(obj);
        placedGameObjects.Remove(obj);

        foreach (var item in placedGameObjects)
        {
            item.PlacementData.OrderPlaceObjectIndex(obj.PlacementData.PlaceObjectIndex);
        }

        return id;
    }

    // 설치된 오브젝트 삭제    
    public void DestoryStructure()
    {
        RemoveStructure(SelectedObject);
        placementUI.OnSetObjectListUi(database, SelectedObject.PlacementData.ID, placedGameObjects);
        inven.PlusItem(database.objects[SelectedObject.PlacementData.ID].LevelList[0].NeedItems);
        Destroy(SelectedObject.transform.parent.gameObject);
        //preview.StopShowingPreview();
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
