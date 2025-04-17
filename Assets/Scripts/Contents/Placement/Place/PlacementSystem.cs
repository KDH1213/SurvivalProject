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
    private Inventory inventory;
    [SerializeField]
    private Grid grid;

    //[SerializeField]
    public PlacementObjectList Database { get; set; }
    public int SelectedObjectIndex { get; set; }
    public float gridCellCount;

    public MaxBuildCount buildCount;
    
    private GameObject testGameObject;
    public PlacementInput GetInputManager { get; private set; }
    public Grid GetGrid { get; private set; }

    public PlacementObject SelectedObject { get; set; }
    public List<PlacementObject> PlacedGameObjects { get; private set; } = new List<PlacementObject>();
    private GridData gridData;

    private bool isInit = false;

    private void Awake()
    {
        if(!isInit)
        {
            Initialized();
        }
    }

    public void Initialized()
    {
        isInit = true;
        SelectedObjectIndex = -1;
        inputManager = GetComponent<PlacementInput>();
        placementMode = GetComponent<PlacementMode>();
        placementUI = GetComponent<PlacementUIController>();
        GetGrid = grid;
        gridData = new GridData(new Vector2Int(20, 20));
        grid.cellSize = Vector3.one * 10 / gridCellCount;

        Database = new PlacementObjectList();
        Database.SetObjects();

        var stageManager = GameObject.FindGameObjectWithTag("StageManager");
        if (stageManager != null)
        {
            stageManager.GetComponent<StageManager>().onSaveEvent += Save;
        }

        if (SaveLoadManager.Data != null)
        {
            SaveLoadManager.Load();

            Load();
        }
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
        SelectedObjectIndex = Database.objects.FindIndex(data => data.ID == ID);
        if (SelectedObjectIndex < 0)
        {
            Debug.LogError($"존재하지 않는 ID : {ID}");
            return;
        }
        preview.StartShowingPlacementPreview(Database.objects[SelectedObjectIndex].Prefeb,
            Database.objects[SelectedObjectIndex].Size);
    }

    // 배치 시작 overload
    public void StartPlacement(int ID, PlacementObject obj)
    {
        //StopPlacement();
        SelectedObjectIndex = Database.objects.FindIndex(data => data.ID == ID);
        if (SelectedObjectIndex < 0)
        {
            Debug.LogError($"존재하지 않는 ID : {ID}");
            return;
        }
        preview.StartShowingPlacementPreview(Database.objects[SelectedObjectIndex].Prefeb,
            Database.objects[SelectedObjectIndex].Size, obj);
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
        Vector2Int size = Database.objects[selectedObjectIndex].Size;
        return gridData.CanPlaceObjectAt(gridPosition, size) &&
            !gridData.CheckCollideOther(grid, gridPosition, size);
    }

    

    // 오브젝트 배치
    public void PlaceStructure()
    {
        Vector3 mousePosition = inputManager.LastPosition;
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        var objData = Database.objects[SelectedObjectIndex];

        bool placementValidity = CheckPlacementValidity(gridPosition, SelectedObjectIndex);
        if (!placementValidity)
        {
            return;
        }
        

        if (SelectedObject != null)
        {
            MoveStructure(gridPosition);
            return;
        }
        else
        {
            inven.MinusItem(objData.NeedItems);
            
            foreach(var data in objData.NeedItems)
            {
                if (inventory == null)
                    break;
                inventory.ConsumeItem(data.Key, data.Value);
            }
            
        }

        GameObject newObject = Instantiate(objData.Prefeb);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        newObject.transform.GetChild(0).rotation = preview.PreviewObject.transform.GetChild(0).rotation;

        PlacementObject placementObject = newObject.transform.GetChild(0).GetComponent<PlacementObject>();
        placementObject.IsPlaced = true;
        placementObject.Hp = objData.DefaultHp;
        placementObject.ID = objData.ID;
        placementObject.Rank = objData.Rank;
        placementObject.Position = gridPosition;
        placementObject.Rotation = preview.PreviewObject.transform.GetChild(0).rotation;
        placementObject.uiController = placementUI;
        placementObject.SetData();

        PlacedGameObjects.Add(placementObject);

        gridData.AddObjectAt(gridPosition, objData.Size,
            objData.ID,
            PlacedGameObjects.Count - 1, placementObject);

        int left = placementUI.OnSetObjectListUi(Database, placementObject.PlacementData.ID, PlacedGameObjects);
        if(left <= 0)
        {
            StopPlacement();
            return;
        }

        bool invenValidity = inven.CheckItemCount(objData.NeedItems);
        foreach (var data in objData.NeedItems)
        {
            if (inventory == null)
                break;
            if (inventory.GetTotalItem(data.Key) < data.Value)
            {
                
                invenValidity = false;
            }
        }
        if (!invenValidity)
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
        StopPlacement();
    }

    public int RemoveStructure(PlacementObject obj)
    {
        if(SelectedObject != null)
        {
            SelectedObject.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            obj.transform.parent.gameObject.SetActive(false);
        }
        
        int id = gridData.RemoveObjectAt(obj);
        PlacedGameObjects.Remove(obj);

        foreach (var item in PlacedGameObjects)
        {
            item.PlacementData.OrderPlaceObjectIndex(obj.PlacementData.PlaceObjectIndex);
        }

        return id;
    }

    public void UpgradeStructure(PlacementObject before, int afterId)
    {
        int index = Database.objects.FindIndex(x => x.ID == afterId);
        PlacementObjectInfo placeObjInfo = Database.objects[index];

        GameObject newObject = Instantiate(placeObjInfo.Prefeb);
        newObject.transform.position = grid.CellToWorld(before.Position);
        newObject.transform.GetChild(0).rotation = before.Rotation;

        PlacementObject placementObject = newObject.transform.GetChild(0).GetComponent<PlacementObject>();
        placementObject.IsPlaced = true;
        placementObject.ID = placeObjInfo.ID;
        placementObject.Rank = placeObjInfo.Rank;
        placementObject.Position = before.Position;
        placementObject.Rotation = before.Rotation;
        placementObject.uiController = placementUI;
        placementObject.SetData();
        RemoveStructure(before);

        PlacedGameObjects.Add(placementObject);

        gridData.AddObjectAt(placementObject.Position, placeObjInfo.Size,
            placementObject.ID,
            PlacedGameObjects.Count - 1, placementObject);

        Destroy(before.transform.parent.gameObject);
    }

    // 설치된 오브젝트 삭제    
    public void DestoryStructure()
    {
        if(SelectedObject == null || !preview.IsPreview)
        {
            return;
        }

        int index = Database.objects.FindIndex(x => x.ID == SelectedObject.PlacementData.ID);
        RemoveStructure(SelectedObject);
        placementUI.OnSetObjectListUi(Database, SelectedObject.PlacementData.ID, PlacedGameObjects);
        Destroy(SelectedObject.transform.parent.gameObject);
        preview.StopShowingPreview();
    }

    public void DestoryStructure(PlacementObject obj)
    {
        int index = Database.objects.FindIndex(x => x.ID == obj.ID);
        RemoveStructure(obj);
        placementUI.OnSetObjectListUi(Database, obj.ID, PlacedGameObjects);
        // Destroy(obj.transform.parent.gameObject);
    }

    // 오브젝트 원래 위치로 배치
    public void SetPlacementInfo(PlacementObject obj)
    {
        obj.IsPlaced = true;
        PlacedGameObjects.Add(obj);
        gridData.AddObjectAt(obj.Position, Database.objects[SelectedObjectIndex].Size,
            Database.objects[SelectedObjectIndex].ID,
            PlacedGameObjects.Count - 1, obj);
    }

    public void Save()
    {
        if(SaveLoadManager.Data == null)
        {
            return;
        }

        SaveLoadManager.Data.farmPlacementSaveInfos.Clear();
        SaveLoadManager.Data.placementSaveInfoList.Clear();

        foreach (var placedGameObject in PlacedGameObjects)
        {
            placedGameObject.Save();
        }

    }

    public void Load()
    {
        PlacedGameObjects.Clear();

        var placementSaveInfoList = SaveLoadManager.Data.farmPlacementSaveInfos;
        foreach (var placement in placementSaveInfoList)
        {
            // TODO :: 배치 오브젝트 생성 코드

            int index = Database.objects.FindIndex(x => x.ID == placement.id);
            PlacementObjectInfo placeObjInfo = Database.objects[index];

            GameObject newObject = Instantiate(placeObjInfo.Prefeb);
            newObject.transform.position = grid.CellToWorld(placement.position);
            newObject.transform.GetChild(0).rotation = placement.rotation;

            PlacementObject placementObject = newObject.transform.GetChild(0).GetComponent<PlacementObject>();
            placementObject.IsPlaced = true;
            placementObject.ID = placeObjInfo.ID;
            placementObject.Hp = placeObjInfo.DefaultHp;
            placementObject.Rank = placeObjInfo.Rank;
            placementObject.Position = placement.position;
            placementObject.Rotation = placement.rotation;
            placementObject.uiController = placementUI;
            placementObject.SetData();
            placementObject.Load();

            PlacedGameObjects.Add(placementObject);

            gridData.AddObjectAt(placement.position, placeObjInfo.Size,
                placement.id,
                PlacedGameObjects.Count - 1, placementObject);

        }

        var placementSaveInfoList2 = SaveLoadManager.Data.placementSaveInfoList;
        foreach (var placement in placementSaveInfoList2)
        {
            // TODO :: 배치 오브젝트 생성 코드

            int index = Database.objects.FindIndex(x => x.ID == placement.id);
            PlacementObjectInfo placeObjInfo = Database.objects[index];

            GameObject newObject = Instantiate(placeObjInfo.Prefeb);
            newObject.transform.position = grid.CellToWorld(placement.position);
            newObject.transform.GetChild(0).rotation = placement.rotation;

            PlacementObject placementObject = newObject.transform.GetChild(0).GetComponent<PlacementObject>();
            placementObject.IsPlaced = true;
            placementObject.ID = placeObjInfo.ID;
            placementObject.Rank = placeObjInfo.Rank;
            placementObject.Hp = placeObjInfo.DefaultHp;
            placementObject.Position = placement.position;
            placementObject.Rotation = placement.rotation;
            placementObject.uiController = placementUI;
            placementObject.SetData();
            placementObject.Load();

            PlacedGameObjects.Add(placementObject);

            gridData.AddObjectAt(placement.position, placeObjInfo.Size,
                placement.id,
                PlacedGameObjects.Count - 1, placementObject);

        }

    }

    /*private void OnApplicationQuit()
    {
        Save();

        SaveLoadManager.Save(1);
    }*/
}
