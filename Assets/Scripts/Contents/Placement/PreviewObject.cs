using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PreviewObject : MonoBehaviour
{

    [SerializeField]
    private PlacementSystem placementSystem;
    private PlacementInput inputManager;
    [SerializeField]
    private GameObject placementUI;
    [SerializeField]
    private GameObject distroyButton;

    [SerializeField]
    private float previewYOffset = 0.06f;
    public bool IsPreview { get; private set; }
    [SerializeField]
    private GameObject previewObject;
    [SerializeField]
    private Material previewMaterialsPrefeb;
    private Material previewMaterialsInstance;

    private void Start()
    {
        inputManager = placementSystem.GetInputManager;
        previewMaterialsInstance = new Material(previewMaterialsPrefeb);
    }

    private void Update()
    {
        if (IsPreview)
        {
            placementUI.transform.position = Camera.main.
                WorldToScreenPoint(previewObject.transform.GetChild(0).position);
        }

        
    }
    public void StartShowingPlacementPreview(GameObject prefeb, PlacementObject obj = null)
    {
        previewObject = Instantiate(prefeb);
        if (obj != null)
        {
            PlacePreview2(obj.Position);
        }
        else
        {
            PlacePreview();
        }
        PreparePreview(previewObject);
        placementUI.SetActive(true);
        IsPreview = true;
        inputManager.OnClickPlace += PlacePreview;
    }

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

    public void StopShowingPreview()
    {
        Destroy(previewObject);
        IsPreview = false;
        placementUI.SetActive(false);
        distroyButton.SetActive(false);
        inputManager.OnClickPlace -= PlacePreview;
    }

    public void StopShowingPreviewToButton()
    {
        RePlaceObject();
        StopShowingPreview();
    }

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

    public void UpdatePosition(Vector3 position, bool validity)
    {
        ApplyFeedback(validity);
        MovePreview(position);
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        c.a = 0.5f;
        previewMaterialsInstance.color = c;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    private void PlacePreview()
    {
        Vector3 mousePosition = inputManager.LastPosition;
        Vector3Int gridPosition = placementSystem.GetGrid.WorldToCell(mousePosition);

        bool placementValidity = placementSystem.CheckPlacementValidity(gridPosition, placementSystem.SelectedObjectIndex);

        UpdatePosition(placementSystem.GetGrid.CellToWorld(gridPosition), placementValidity);
    }

    private void PlacePreview2(Vector3Int gridPosition)
    {

        bool placementValidity = placementSystem.CheckPlacementValidity(gridPosition, placementSystem.SelectedObjectIndex);
        distroyButton.SetActive(true);
        UpdatePosition(placementSystem.GetGrid.CellToWorld(gridPosition), placementValidity);
        inputManager.SetLastPos(placementSystem.GetGrid.CellToWorld(gridPosition));
    }

}
