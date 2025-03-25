using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{

    [SerializeField]
    private PlacementSystem placementSystem;
    private PlacementInput inputManager;
    [SerializeField]
    private GameObject placementUI;

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
        if(IsPreview)
        {
            placementUI.transform.position = inputManager.PlacementCamera.
                WorldToScreenPoint(previewObject.transform.GetChild(0).position);
        }
        
    }
    public void StartShowingPlacementPreview(GameObject prefeb)
    {
        StopShowingPreview();
        previewObject = Instantiate(prefeb);
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
        inputManager.OnClickPlace -= PlacePreview;
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
        if (inputManager.IsPointerOverUi)
        {
            return;
        }
        Vector3 mousePosition = inputManager.LastPosition;
        Vector3Int gridPosition = placementSystem.GetGrid.WorldToCell(mousePosition);

        bool placementValidity = placementSystem.CheckPlacementValidity(gridPosition, placementSystem.SelectedObjectIndex);

        UpdatePosition(placementSystem.GetGrid.CellToWorld(gridPosition), placementValidity);
    }

}
