using UnityEngine;
using UnityEngine.UIElements;

public class PlacementPreview : MonoBehaviour
{

    [SerializeField]
    private PlacementSystem placementSystem;
    [SerializeField]
    private PlacementUIController placementUI;
    private PlacementInput inputManager;
    

    [SerializeField]
    private float previewYOffset = 0.06f;
    public bool IsPreview { get; private set; }
    [SerializeField]
    private GameObject cellIndicator;
    [SerializeField]
    public GameObject PreviewObject { get; private set; }
    [SerializeField]
    private Material previewMaterialsPrefeb;
    private Material previewMaterialsInstance;

    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        inputManager = placementSystem.GetComponent<PlacementInput>();
        placementUI = placementSystem.GetComponent<PlacementUIController>();
        previewMaterialsInstance = new Material(previewMaterialsPrefeb);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    // 프리뷰 시작
    public void StartShowingPlacementPreview(GameObject prefeb, Vector2Int size, PlacementObject obj = null)
    {
        PreviewObject = Instantiate(prefeb);
        PreviewObject.transform.GetChild(0).gameObject.layer = GetLayer.Preview;
        if (obj != null)
        {
            PreviewObject.transform.GetChild(0).transform.rotation = obj.Rotation;
            PlacePreview(obj.Position);
        }
        else
        {
            PlacePreview();
        }
        PreparePreview(PreviewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
        placementUI.OnShowPlaceUI(true);
        IsPreview = true;
        inputManager.OnClickPlace += PlacePreview;
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(0.5f * size.x, 0.5f,
                0.5f * size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }

    // 오브젝트 랜더러 설정
    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialsInstance;
            }
            renderer.materials = materials;
        }
    }

    // 프리뷰 중지
    public void StopShowingPreview()
    {
        Destroy(PreviewObject);
        cellIndicator.SetActive(false);
        IsPreview = false;
        placementUI.OnShowPlaceUI(false);
        inputManager.OnClickPlace -= PlacePreview;
    }

    public void StopShowingPreviewToButton()
    {
        RePlaceObject();
        StopShowingPreview();
    }

    // 이미 설치된 오브젝트 재배치
    public void RePlaceObject()
    {
        if (placementSystem.SelectedObject != null && IsPreview)
        {
            PlacementObject obj = placementSystem.SelectedObject;
            placementSystem.SetPlacementInfo(obj);
            placementSystem.SelectedObject.transform.parent.gameObject.SetActive(true);
            placementSystem.SelectedObject = null;
        }
    }

    public void SetCellIndicator(GameObject placementObj, Vector2Int size)
    {
        var cell = Instantiate(cellIndicator);
        var pos = placementObj.transform.position;
        cell.transform.position = new Vector3(pos.x, pos.y + previewYOffset, pos.z);
        cell.transform.localScale = new Vector3(0.5f * size.x * 5f, 0.5f,
                0.5f * size.y * 5f);
        cell.GetComponentInChildren<Renderer>().material.color = Color.red;
        cell.transform.parent = placementObj.transform;
        cell.transform.GetChild(0).tag = "CellIndicator";
        cell.gameObject.SetActive(true);
    }
    public void OnRotate()
    {
        PreviewObject.transform.GetChild(0).Rotate(new Vector3(0f, 90f, 0f));
    }

    // 이동
    public void UpdatePosition(Vector3 position, bool validity)
    {
        ApplyFeedback(validity);
        MoveCursor(position);
        MovePreview(position);
    }

    // 오브젝트 색상 변화
    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
        previewMaterialsInstance.color = c;
    }

    // 오브젝트 이동
    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }
    private void MovePreview(Vector3 position)
    {
        Debug.Log(position);
        PreviewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    // 첫 오브젝트 이동
    private void PlacePreview()
    {
        inputManager.UpdatePosition();
        Vector3 mousePosition = inputManager.LastPosition;
        Vector3Int gridPosition = placementSystem.GetGrid.WorldToCell(mousePosition);

        bool placementValidity = placementSystem.CheckPlacementValidity(gridPosition, placementSystem.SelectedObjectIndex);

        UpdatePosition(placementSystem.GetGrid.CellToWorld(gridPosition), placementValidity);
    }

    // 이미 설치된 오브젝트 이동
    private void PlacePreview(Vector3Int gridPosition)
    {
        bool placementValidity = placementSystem.CheckPlacementValidity(gridPosition, placementSystem.SelectedObjectIndex);
        UpdatePosition(placementSystem.GetGrid.CellToWorld(gridPosition), placementValidity);
        inputManager.SetLastPos(placementSystem.GetGrid.CellToWorld(gridPosition));
    }

}
