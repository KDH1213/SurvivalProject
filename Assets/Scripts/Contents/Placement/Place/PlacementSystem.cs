using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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
    public PlaceMode PlaceMode => placementMode.CurrentMode;

    public MaxBuildCount buildCount;
    
    private GameObject testGameObject;
    public PlacementInput GetInputManager { get; private set; }
    public Grid GetGrid { get; private set; }

    public PlacementObject SelectedObject { get; set; }
    public List<PlacementObject> PlacedGameObjects { get; private set; } = new List<PlacementObject>();
    public Dictionary<int, int> PlacedGameObjectTable { get; private set; } = new Dictionary<int, int>();

    public UnityEvent<int, int> onChangeBuildingCountEvnet;

    private GridData gridData;
    public List<StorageStructure> Storages { get; private set; } = new List<StorageStructure>();

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

        var questSystemObject = GameObject.FindWithTag(Tags.QuestSystem);
        if (questSystemObject != null)
        {
            var questSystem = questSystemObject.GetComponent<QuestSystem>();
            onChangeBuildingCountEvnet.AddListener(questSystem.OnCreateBuilding);
            questSystem.SetPlacementSystem(this);
        }

        if (SaveLoadManager.Data != null)
        {
            SaveLoadManager.Load();

            Load();
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

    private void ConsumeItem(int key, int value)
    {
        int inventoryItems = inventory.GetTotalItem(key);
        if(inventoryItems > value)
        {
            inventory.ConsumeItem(key, value);
        }
        else
        {
            inventory.ConsumeItem(key, inventoryItems);
            int leftItems = value - inventoryItems;
            var storageList = Storages
                .OrderBy(structure => structure.inventory.GetTotalItem(key));

            foreach(var structure in storageList)
            {
                var storageItems = structure.inventory.GetTotalItem(key);
                if(storageItems == 0)
                {
                    continue;
                }
                if(storageItems <= leftItems)
                {
                    leftItems -= storageItems;
                }
                else
                {
                    structure.inventory.ConsumeItem(key, leftItems);
                    leftItems = 0;
                }
                

                if (leftItems <= 0)
                {
                    break;
                }
            }
            
        }
        
    }
    
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
                ConsumeItem(data.Key, data.Value);
            }
            
        }

        GameObject newObject = Instantiate(objData.Prefeb);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        newObject.transform.GetChild(0).rotation = preview.PreviewObject.transform.GetChild(0).rotation;

        preview.SetCellIndicator(newObject, objData.Size);

        PlacementObject placementObject = newObject.transform.GetChild(0).GetComponent<PlacementObject>();
        placementObject.IsPlaced = true;
        placementObject.Hp = objData.DefaultHp;
        placementObject.ID = objData.ID;
        placementObject.Rank = objData.Rank;
        placementObject.Position = gridPosition;
        placementObject.Rotation = preview.PreviewObject.transform.GetChild(0).rotation;
        placementObject.uiController = placementUI;
        placementObject.SetData();
        placementObject.CreateActInfo();

        if (GameObject.FindWithTag(Tags.Player) != null)
        {
            var target = GameObject.FindWithTag(Tags.Player).GetComponent<PenaltyController>();
            target.OnPlayAct(placementObject);

            PlacedGameObjects.Add(placementObject);
        }

        if (PlacedGameObjectTable.ContainsKey(placementObject.ID))
        {
            ++PlacedGameObjectTable[placementObject.ID];
        }
        else
        {
            PlacedGameObjectTable.Add(placementObject.ID, 1);
        }

        onChangeBuildingCountEvnet?.Invoke(placementObject.ID, PlacedGameObjectTable[placementObject.ID]);

        gridData.AddObjectAt(gridPosition, objData.Size,
            objData.ID,
            PlacedGameObjects.Count - 1, placementObject);
        if(placementObject is StorageStructure)
        {
            Storages.Add(placementObject as StorageStructure);
        }

        int left = placementUI.OnSetObjectListUi(Database, placementObject.PlacementData.ID, PlacedGameObjects);
        if(left <= 0)
        {
            StopPlacement();
            return;
        }

        bool invenValidity = true;
        if (inventory == null)
        {
            invenValidity = inven.CheckItemCount(objData.NeedItems);
        }
        else
        {
            foreach (var data in objData.NeedItems)
            {
                int leftResource = inventory.GetTotalItem(data.Key) + Storages.Sum(storage => storage.inventory.GetTotalItem(data.Key));
                if (leftResource < data.Value)
                {
                    invenValidity = false;
                }
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

    // ��ġ�� ������Ʈ ����
    public bool SelectStructure(PlacementObject obj)
    {
        if(preview.IsPreview)
        {
            return false;
        }

        SelectedObject = obj;

        if (placementMode.CurrentMode == PlaceMode.Place)
        {
            int index = Database.objects.FindIndex(data => data.ID == SelectedObject.ID);
            placementUI.OnOpenObjectInfo(Database.objects[index]);
        }
        else if(placementMode.CurrentMode == PlaceMode.Edit)
        {
            placementUI.OnShowEditUI(true);
        }
        
        return true;
    }

    public void OnMoveEditPlacement()
    {
        int id = RemoveStructure(SelectedObject);
        StartPlacement(id, SelectedObject);
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

        if (PlacedGameObjectTable.ContainsKey(id))
        {
            --PlacedGameObjectTable[id];
            onChangeBuildingCountEvnet?.Invoke(id, PlacedGameObjectTable[id]);
        }

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
        placementObject.Upgrade(before);
        RemoveStructure(before);

        preview.SetCellIndicator(newObject, placeObjInfo.Size);

        PlacedGameObjects.Add(placementObject);

        if (PlacedGameObjectTable.ContainsKey(placementObject.ID))
        {
            ++PlacedGameObjectTable[placementObject.ID];
        }
        else
        {
            PlacedGameObjectTable.Add(placementObject.ID, 1);
        }

        onChangeBuildingCountEvnet?.Invoke(placementObject.ID, PlacedGameObjectTable[placementObject.ID]);
        gridData.AddObjectAt(placementObject.Position, placeObjInfo.Size,
            placementObject.ID,
            PlacedGameObjects.Count - 1, placementObject);

        Destroy(before.transform.parent.gameObject);
    }

    public void DestoryStructure()
    {
        if(SelectedObject == null)
        {
            return;
        }

        var placementID = SelectedObject.PlacementData.ID;
        var data = DataTableManager.StructureTable.Get(placementID);
        
        int index = Database.objects.FindIndex(x => x.ID == placementID);
        RemoveStructure(SelectedObject);
        placementUI.OnSetObjectListUi(Database, placementID, PlacedGameObjects);
        Destroy(SelectedObject.transform.parent.gameObject);

        foreach (var item in data.ReturnItemList)
        {
            if(inventory == null)
            {
                break;
            }
            var itemData = DataTableManager.ItemTable.Get(item.Key);
            var returnItem = new DropItemInfo();
            returnItem.id = itemData.ID;  // testItem.ID
            returnItem.itemData = itemData;
            returnItem.amount = item.Value;
            inventory.AddItem(returnItem);
        }
        
        preview.StopShowingPreview();
    }

    public void DestoryStructure(PlacementObject obj)
    {
        int index = Database.objects.FindIndex(x => x.ID == obj.ID);
        RemoveStructure(obj);
        placementUI.OnSetObjectListUi(Database, obj.ID, PlacedGameObjects);
        // Destroy(obj.transform.parent.gameObject);
    }

    // ������Ʈ ���� ��ġ�� ��ġ
    public void SetPlacementInfo(PlacementObject obj)
    {
        obj.IsPlaced = true;
        PlacedGameObjects.Add(obj);

        if (PlacedGameObjectTable.ContainsKey(obj.ID))
        {
            ++PlacedGameObjectTable[obj.ID];
        }
        else
        {
            PlacedGameObjectTable.Add(obj.ID, 1);
        }

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
        SaveLoadManager.Data.storagePlacementSaveInfo.Clear();
        SaveLoadManager.Data.placementSaveInfoList.Clear();

        foreach (var placedGameObject in PlacedGameObjects)
        {
            placedGameObject.Save();
        }

    }

    public void Load()
    {
        PlacedGameObjects.Clear();
        PlacedGameObjectTable.Clear();

        LoadFarm();
        LoadStorage();
        LoadStructure();

        var goes = GameObject.FindGameObjectsWithTag("CellIndicator");
        foreach (var go in goes)
        {
            go.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    /*private void OnApplicationQuit()
    {
        Save();

        SaveLoadManager.Save(1);
    }*/


    private void LoadFarm()
    {
        var placementSaveInfoList = SaveLoadManager.Data.farmPlacementSaveInfos;
        foreach (var placement in placementSaveInfoList)
        {
            // TODO :: ��ġ ������Ʈ ���� �ڵ�

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

            if (PlacedGameObjectTable.ContainsKey(placementObject.ID))
            {
                ++PlacedGameObjectTable[placementObject.ID];
            }
            else
            {
                PlacedGameObjectTable.Add(placementObject.ID, 1);
            }

            preview.SetCellIndicator(newObject, placeObjInfo.Size);

            PlacedGameObjects.Add(placementObject);

            gridData.AddObjectAt(placement.position, placeObjInfo.Size,
                placement.id,
                PlacedGameObjects.Count - 1, placementObject);

        }
    }

    private void LoadStorage()
    {
        var placementSaveInfoList = SaveLoadManager.Data.storagePlacementSaveInfo;
        foreach (var placement in placementSaveInfoList)
        {
            // TODO :: ��ġ ������Ʈ ���� �ڵ�

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

            if (PlacedGameObjectTable.ContainsKey(placementObject.ID))
            {
                ++PlacedGameObjectTable[placementObject.ID];
            }
            else
            {
                PlacedGameObjectTable.Add(placementObject.ID, 1);
            }

            preview.SetCellIndicator(newObject, placeObjInfo.Size);

            PlacedGameObjects.Add(placementObject);
            Storages.Add(placementObject as StorageStructure);
            gridData.AddObjectAt(placement.position, placeObjInfo.Size,
                placement.id,
                PlacedGameObjects.Count - 1, placementObject);

        }
    }

    private void LoadStructure()
    {
        var placementSaveInfoList = SaveLoadManager.Data.placementSaveInfoList;
        foreach (var placement in placementSaveInfoList)
        {
            // TODO :: ��ġ ������Ʈ ���� �ڵ�

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

            if (PlacedGameObjectTable.ContainsKey(placementObject.ID))
            {
                ++PlacedGameObjectTable[placementObject.ID];
            }
            else
            {
                PlacedGameObjectTable.Add(placementObject.ID, 1);
            }

            preview.SetCellIndicator(newObject, placeObjInfo.Size);
            PlacedGameObjects.Add(placementObject);

            gridData.AddObjectAt(placement.position, placeObjInfo.Size,
                placement.id,
                PlacedGameObjects.Count - 1, placementObject);

        }
    }
}
