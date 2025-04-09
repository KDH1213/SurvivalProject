using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour, ISaveLoadData
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

    //[SerializeField]
    public PlacementObjectList Database { get; set; }
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

        Database = new PlacementObjectList();
        Database.SetObjects();

        var stageManager = GameObject.FindGameObjectWithTag("StageManager");
        if (stageManager != null)
        {
            stageManager.GetComponent<StageManager>().onSaveEvent += Save;
        }
    }

    private void Update()
    {
        // ��ġ�� �׸� �̵�
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

    // ��ġ����
    public void StartPlacement(int ID)
    {
        preview.RePlaceObject();
        StopPlacement();
        SelectedObjectIndex = Database.objects.FindIndex(data => data.ID == ID);
        if (SelectedObjectIndex < 0)
        {
            Debug.LogError($"�������� �ʴ� ID : {ID}");
            return;
        }
        preview.StartShowingPlacementPreview(Database.objects[SelectedObjectIndex].Prefeb,
            Database.objects[SelectedObjectIndex].Size);
    }

    // ��ġ ���� overload
    public void StartPlacement(int ID, PlacementObject obj)
    {
        //StopPlacement();
        SelectedObjectIndex = Database.objects.FindIndex(data => data.ID == ID);
        if (SelectedObjectIndex < 0)
        {
            Debug.LogError($"�������� �ʴ� ID : {ID}");
            return;
        }
        preview.StartShowingPlacementPreview(Database.objects[SelectedObjectIndex].Prefeb,
            Database.objects[SelectedObjectIndex].Size, obj);
    }

    // ��ġ ���߱�
    private void StopPlacement()
    {
        SelectedObjectIndex = -1;
        SelectedObject = null;
        preview.StopShowingPreview();
    }

    // ��ġ �������� �˻�
    public bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        Vector2Int size = Database.objects[selectedObjectIndex].Size;
        return gridData.CanPlaceObjectAt(gridPosition, size) &&
            !gridData.CheckCollideOther(grid, gridPosition, size);
    }

    

    // ������Ʈ ��ġ
    public void PlaceStructure()
    {
        Vector3 mousePosition = inputManager.LastPosition;
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, SelectedObjectIndex);
        if (!placementValidity)
        {
            return;
        }
        bool invenValidity = inven.CheckItemCount(Database.objects[SelectedObjectIndex].NeedItems);
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
            inven.MinusItem(Database.objects[SelectedObjectIndex].NeedItems);
        }

        GameObject newObject = Instantiate(Database.objects[SelectedObjectIndex].Prefeb);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        newObject.transform.GetChild(0).rotation = preview.PreviewObject.transform.GetChild(0).rotation;

        PlacementObject placementObject = newObject.transform.GetChild(0).GetComponent<PlacementObject>();
        placementObject.IsPlaced = true;
        placementObject.ID = Database.objects[SelectedObjectIndex].ID;
        placementObject.Position = gridPosition;
        placementObject.Rotation = preview.PreviewObject.transform.GetChild(0).rotation;
        placementObject.SetData();

        placedGameObjects.Add(placementObject);

        gridData.AddObjectAt(gridPosition, Database.objects[SelectedObjectIndex].Size,
            Database.objects[SelectedObjectIndex].ID,
            placedGameObjects.Count - 1, placementObject);

        int left = placementUI.OnSetObjectListUi(Database, placementObject.PlacementData.ID, placedGameObjects);
        if(left <= 0)
        {
            StopPlacement();
            return;
        }

        Vector3Int nextPos = gridData.SearchSide(gridPosition, Database.objects[SelectedObjectIndex].Size);
        preview.UpdatePosition(grid.CellToWorld(nextPos),
            CheckPlacementValidity(nextPos, SelectedObjectIndex));
        inputManager.LastPosition = grid.CellToWorld(nextPos);
        //StopPlacement();
    }

    // ��ġ�� ������Ʈ ����
    public bool SelectStructure(PlacementObject obj)
    {
        if(preview.IsPreview)
        {
            return false;
        }

        SelectedObject = obj;

        if (placementMode.CurrentMode == Mode.Place)
        {
            int index = Database.objects.FindIndex(data => data.ID == SelectedObject.ID);
            placementUI.OnOpenObjectInfo(Database.objects[index]);
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

    // ��ġ�� ������Ʈ ����    
    public void DestoryStructure()
    {
        RemoveStructure(SelectedObject);
        placementUI.OnSetObjectListUi(Database, SelectedObject.PlacementData.ID, placedGameObjects);
        inven.PlusItem(Database.objects[SelectedObject.PlacementData.ID].NeedItems);
        Destroy(SelectedObject.transform.parent.gameObject);
        //preview.StopShowingPreview();
    }

    // ������Ʈ ���� ��ġ�� ��ġ
    public void SetPlacementInfo(PlacementObject obj)
    {
        obj.IsPlaced = true;
        placedGameObjects.Add(obj);
        gridData.AddObjectAt(obj.Position, Database.objects[SelectedObjectIndex].Size,
            Database.objects[SelectedObjectIndex].ID,
            placedGameObjects.Count - 1, obj);
    }

    public void Save()
    {
        if(SaveLoadManager.Data == null)
        {
            return;
        }

        foreach (var placedGameObject in placedGameObjects)
        {
            placedGameObject.Save();
        }
        
    }

    public void Load()
    {
        if (SaveLoadManager.Data == null)
        {
            return;
        }

        var placementSaveInfoList = SaveLoadManager.Data.placementSaveInfoList;
        foreach (var placement in placementSaveInfoList)
        {
            // TODO :: ��ġ ������Ʈ ���� �ڵ�
        }


    }
}
