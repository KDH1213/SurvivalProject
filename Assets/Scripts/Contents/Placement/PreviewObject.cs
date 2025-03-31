using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject PreviewObject { get; private set; }
    [SerializeField]
    private Material previewMaterialsPrefeb;
    private Material previewMaterialsInstance;

    private void Start()
    {
        inputManager = placementSystem.GetComponent<PlacementInput>();
        placementUI = placementSystem.GetComponent<PlacementUIController>();
        previewMaterialsInstance = new Material(previewMaterialsPrefeb);
    }

    // ������ ����
    public void StartShowingPlacementPreview(GameObject prefeb, PlacementObject obj = null)
    {
        PreviewObject = Instantiate(prefeb);
        if (obj != null)
        {
            PlacePreview(obj.Position);
        }
        else
        {
            PlacePreview();
        }
        PreparePreview(PreviewObject);
        placementUI.SetPlaceUI(true);
        IsPreview = true;
        inputManager.OnClickPlace += PlacePreview;
    }

    // ������Ʈ ������ ����
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

    // ������ ����
    public void StopShowingPreview()
    {
        Destroy(PreviewObject);
        IsPreview = false;
        placementUI.SetPlaceUI(false);
        placementUI.SetDestoryButton(false);
        inputManager.OnClickPlace -= PlacePreview;
    }

    public void StopShowingPreviewToButton()
    {
        RePlaceObject();
        StopShowingPreview();
    }

    // �̹� ��ġ�� ������Ʈ ���ġ
    public void RePlaceObject()
    {
        if (placementSystem.SelectedObject != null)
        {
            PlacementObject obj = placementSystem.SelectedObject;
            placementSystem.SetPlacementInfo(obj);
            placementSystem.SelectedObject.transform.parent.gameObject.SetActive(true);
            placementSystem.SelectedObject = null;
        }
    }

    // �̵�
    public void UpdatePosition(Vector3 position, bool validity)
    {
        ApplyFeedback(validity);
        MovePreview(position);
    }

    // ������Ʈ ���� ��ȭ
    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        c.a = 0.5f;
        previewMaterialsInstance.color = c;
    }

    // ������Ʈ �̵�
    private void MovePreview(Vector3 position)
    {
        PreviewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    // ù ������Ʈ �̵�
    private void PlacePreview()
    {
        Vector3 mousePosition = inputManager.LastPosition;
        Vector3Int gridPosition = placementSystem.GetGrid.WorldToCell(mousePosition);

        bool placementValidity = placementSystem.CheckPlacementValidity(gridPosition, placementSystem.SelectedObjectIndex);

        UpdatePosition(placementSystem.GetGrid.CellToWorld(gridPosition), placementValidity);
    }

    // �̹� ��ġ�� ������Ʈ �̵�
    private void PlacePreview(Vector3Int gridPosition)
    {
        bool placementValidity = placementSystem.CheckPlacementValidity(gridPosition, placementSystem.SelectedObjectIndex);
        placementUI.SetDestoryButton(true);
        UpdatePosition(placementSystem.GetGrid.CellToWorld(gridPosition), placementValidity);
        inputManager.SetLastPos(placementSystem.GetGrid.CellToWorld(gridPosition));
    }

}
